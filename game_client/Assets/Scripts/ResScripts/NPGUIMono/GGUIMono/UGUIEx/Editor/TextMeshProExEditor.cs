using System;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(TextMeshProEx))]
    public class TextMeshProExEditor : TMP_EditorPanel
    {
        static readonly GUIContent k_FontTypeLabel = new GUIContent("Font Type", "字体类型");

        protected SerializedProperty m_FontTypeProp;
        protected SerializedProperty m_InitMaterialTagProp;//初始化时的材质tag(这个一般来说是由配置的FontType决定的, 切换语言不会导致所需材质变化
        protected TextMeshProEx _m_TextMeshProEx;
        protected TMP_FontAsset _m_initFontAsset;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            m_FontTypeProp = serializedObject.FindProperty("_m_fontType");
            m_InitMaterialTagProp = serializedObject.FindProperty("_m_sInitMaterialTag");
            
            _m_TextMeshProEx = target as TextMeshProEx;
            _m_initFontAsset = _m_TextMeshProEx == null ? null : _m_TextMeshProEx.font;

            if (!Application.isPlaying)
            {
                _chgFont();//在游戏没有运行时enable时, 刷新一次使用的字体
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            DrawFontTypeAndUseLanguage();
            
            base.OnInspectorGUI();
            
            // 在游戏没有运行时
            if (!Application.isPlaying)
            {
                // 检查字体是否被修改, 若被修改则恢复
                if (_m_TextMeshProEx != null && _m_TextMeshProEx.font != _m_initFontAsset)
                {
                    Debug.LogError_EditorOnly("TextMeshProEx 不允许手动修改FontAsset, 请通过变换FontType或使用语言修改需要使用的FontAsset", _m_TextMeshProEx);
                    _setFontAssetWithChgInitFont(_m_initFontAsset);//恢复原始字体
                    EditorUtility.SetDirty(target);
                }
                
                // 检查材质是否被修改, 若被修改同步改动InitMaterialTag
                if (m_InitMaterialTagProp != null && _m_TextMeshProEx != null && _m_TextMeshProEx.fontSharedMaterial != null)
                {
                    string initMaterialTag = m_InitMaterialTagProp.stringValue;
                    if (string.IsNullOrEmpty(initMaterialTag) || !_m_TextMeshProEx.fontSharedMaterial.name.EndsWith(initMaterialTag))
                    {
#if NP_GAME
                        m_InitMaterialTagProp.stringValue = TMP_FontAssetMaterialObj.getTMPMaterialTag(_m_TextMeshProEx.fontSharedMaterial);
#else
                        _m_TextMeshProEx.fontSharedMaterial.name.Split(new char[] {' '}).LastOrDefault()?.Replace("(Clone)", "");
#endif
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }
        
        protected void DrawFontTypeAndUseLanguage()
        {
            if (!Application.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_FontTypeProp, k_FontTypeLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    _chgFont();
                }
                
                ENPLanguage textUseLanguage = (ENPLanguage)EditorPrefs.GetInt("textUseLanguage", 0);
                EditorGUI.BeginChangeCheck();
                textUseLanguage = (ENPLanguage)EditorGUILayout.EnumPopup("使用语言", textUseLanguage);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetInt("textUseLanguage", (int)textUseLanguage);
                    _chgFont();
                    _resetMaterialToInit();//只有变化语言时需要重置材质为原类型
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(m_FontTypeProp, k_FontTypeLabel);
                EditorGUI.EndDisabledGroup();
            }
        }
        
        /// <summary>
        /// 更换FontAsset资源
        /// </summary>
        private void _chgFont()
        {
            if(Application.isPlaying)//游戏运行时不刷新字体
                return;
            
            if (m_FontTypeProp == null || _m_TextMeshProEx == null)
            {
                Debug.LogError_EditorOnly("_m_TextMeshProEx or m_FontTypeProp is null", _m_TextMeshProEx);
                return;
            }
            
            EFontType fontType = (EFontType) m_FontTypeProp.enumValueIndex;
            ENPLanguage textUseLanguage = (ENPLanguage)EditorPrefs.GetInt("textUseLanguage", 0);

            //加载NPGSOGameCommonInfo资源
            NPGSOGameCommonInfo commonInfo = EditorUtil.loadPrefab<NPGSOGameCommonInfo>(NPGSOGameCommonInfo.assetPath, NPGSOGameCommonInfo.objName, ".asset", null);
            if (commonInfo == null || commonInfo.TMPFontTypeAssetPathList == null)
            {
                Debug.LogError_EditorOnly($"没有找到{NPGSOGameCommonInfo.objName}或者配置的TMPFontTypeAssetPathList为空, 将使用TMP_Settings中配置的默认字体", _m_TextMeshProEx);
                
                _setFontAssetWithChgInitFont(TMP_Settings.defaultFontAsset);
                EditorUtility.SetDirty(target);
                commonInfo = null;
                Resources.UnloadUnusedAssets();
                return;
            }
            
            // 获取字体
            LanguageFontTypeAssetPath fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.TMPFontTypeAssetPathList, textUseLanguage);
            if (fontTypeAssetPath == null)//若找不到语言对应的LanguageFontTypeAssetPath
            {
                Debug.LogError_EditorOnly($"没有找到语言:{textUseLanguage}对应的LanguageFontTypeAssetPath, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会默认使用{ENPLanguage.NONE}配置", _m_TextMeshProEx);
                fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.TMPFontTypeAssetPathList, ENPLanguage.NONE);
            }
            if (fontTypeAssetPath == null)//若找不到语言对应的ENPLanguage.NONE配置
            {
                Debug.LogError_EditorOnly($"没有找到:{ENPLanguage.NONE}配置的默认字体, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会使用{ENPLanguage.EN_US}的字体配置", _m_TextMeshProEx);
                fontTypeAssetPath = LanguageFontTypeAssetPath.FindLanguageFontTypeAssetPath(commonInfo.TMPFontTypeAssetPathList, ENPLanguage.EN_US);
            }
            if (fontTypeAssetPath == null)//英文配置也找不到的话, 直接默认使用第一个
            {
                Debug.LogError_EditorOnly($"没有找到语言:{ENPLanguage.EN_US}对应的LanguageFontTypeAssetPath, 请检查{NPGSOGameCommonInfo.objName}配置, 这里将会默认使用TMPFontTypeAssetPathList列表第一个元素数据", _m_TextMeshProEx);
                fontTypeAssetPath = commonInfo.TMPFontTypeAssetPathList.GetFirst();
            }

            if (fontTypeAssetPath == null)
            {
                Debug.LogError_EditorOnly($"没有可用的字体配置, 请检查{NPGSOGameCommonInfo.objName}配置", _m_TextMeshProEx);
                
                _setFontAssetWithChgInitFont(null);
                EditorUtility.SetDirty(target);
                commonInfo = null;
                Resources.UnloadUnusedAssets();
                return;
            }
            
            FontTypeAssetPath fontTypeAsset = FontTypeAssetPath.FindFontTypeAssetPath(fontTypeAssetPath.fontAssetPathList, fontType);
            NPCommonAssetPathInfo fontAssetPath = fontTypeAsset != null ? fontTypeAsset.fontAssetPath : fontTypeAssetPath.defaultFontAssetPath;
            if (fontAssetPath == null || !fontAssetPath.enable)
            {
                Debug.LogError_EditorOnly($"获取Font资源路径失败, 请检查{NPGSOGameCommonInfo.objName}配置", _m_TextMeshProEx);
                
                _setFontAssetWithChgInitFont(null);
                EditorUtility.SetDirty(target);
                commonInfo = null;
                Resources.UnloadUnusedAssets();
                return;
            }
            
            TMP_FontAsset fontAsset = EditorUtil.loadPrefab<TMP_FontAsset>(fontAssetPath.asset_path, fontAssetPath.obj_name, ".asset", null);
            if (fontAsset == null)
            {
                Debug.LogError_EditorOnly($"加载TMP_FontAsset资源:{fontAssetPath}失败", _m_TextMeshProEx);
                
                _setFontAssetWithChgInitFont(null);
                EditorUtility.SetDirty(target);
                commonInfo = null;
                Resources.UnloadUnusedAssets();
                return;
            }

            _setFontAssetWithChgInitFont(fontAsset);
            EditorUtility.SetDirty(target);
            commonInfo = null;
            Resources.UnloadUnusedAssets();
        }
        
        /// <summary>
        /// 设置FontAsset的同时改变记录的初始字体
        /// </summary>
        /// <param name="_fontAsset"></param>
        private void _setFontAssetWithChgInitFont(TMP_FontAsset _fontAsset)
        {
            if(_m_TextMeshProEx == null)
                return;

            _m_TextMeshProEx.font = _fontAsset;
            _m_initFontAsset = _m_TextMeshProEx.font;//因为set_font方法中当font为null时有赋默认值操作, 所以这里初始值要使用get_font获取
        }

        /// <summary>
        /// 将Material重置为InitMaterial同类型材质
        /// </summary>
        private void _resetMaterialToInit()
        {
            if(Application.isPlaying)//游戏运行时不会通过这个方法进行重置
                return;
            
            if (_m_TextMeshProEx == null || m_InitMaterialTagProp == null)
            {
                Debug.LogError_EditorOnly($"_m_TextMeshProEx == null || m_InitMaterialTagProp == null", _m_TextMeshProEx);
                return;
            }
            
            string initMaterialTag = m_InitMaterialTagProp.stringValue;
            if (string.IsNullOrEmpty(initMaterialTag))
            {
                Debug.LogError_EditorOnly($"string.IsNullOrEmpty(initMaterialTag)", _m_TextMeshProEx);
                return;
            }

            // 若当前材质已经是InitMaterial同类型材质, 则不需要重置, 通过fontSharedMaterial获取材质, 不会创建新的材质实例
            if (_m_TextMeshProEx.fontSharedMaterial != null && _m_TextMeshProEx.fontSharedMaterial.name.EndsWith(initMaterialTag))
                return;
            
            Material[] materials = TMP_EditorUtility.FindMaterialReferences(_m_TextMeshProEx.font);//获取所有可用材质
            if (materials == null)
            {
                Debug.LogError_EditorOnly($"[TextMeshProExEditor _resetMaterialToInit] 想要重置材质为初始类型:{initMaterialTag}, 但是FontAsset:{_m_TextMeshProEx.font} 中找不到对应类型的材质", _m_TextMeshProEx);
                return;
            }
            
            foreach (Material material in materials)
            {
                if (material != null && material.name.EndsWith(initMaterialTag))
                {
                    _m_TextMeshProEx.fontMaterial = material;//通过fontMaterial属性设置材质, 不会实例化新的材质
                    EditorUtility.SetDirty(target);
                    return;
                }
            }
            
            Debug.LogError_EditorOnly($"[TextMeshProExEditor _resetMaterialToInit] 想要重置材质为初始类型:{initMaterialTag}, 但是FontAsset:{_m_TextMeshProEx.font} 中找不到对应类型的材质", _m_TextMeshProEx);
        }
    }
}