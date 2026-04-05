using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace GOE
{
    public class TMP_FontAssetMaterialObj
    {
        public static char[] materialBelongToTMPFontAssetTagSplit = new char[] {' '};//material从属于的TMP_FontAsset的tag分隔符(从TMP_EditorUtility.FindMaterialReferences方法中可以知道是用' '分割TMP_FontAsset.name取第一个字符串)

        public static char[] materialTagSplit = new char[] {' '};//material的tag分隔符, 取material的name最后一个空格后的字符串作为tag
        
        private TMP_FontAsset _m_FontAsset;
        private string _m_sAssetPath;
        
        [NotNull] private List<Material> _m_lMaterialResList = new List<Material>();
        [NotNull] private Dictionary<string, Material> _m_dMaterialInstantiateDic = new Dictionary<string, Material>();

        private bool _m_bisInited = false;
        private long _m_lInitSerialize;
        private List<Action> _m_lInitDelegateList;

        public TMP_FontAssetMaterialObj([NotNull] TMP_FontAsset _fontAsset, string _assetPath)
        {
            _m_FontAsset = _fontAsset;
            _m_sAssetPath = _assetPath;

            _m_bisInited = false;
            _m_lInitSerialize = 0;

            _init();
        }

        public void regInitedDelegate(Action _action)
        {
            if (_m_bisInited)
            {
                _action?.Invoke();
                return;
            }
            
            if (_m_lInitDelegateList == null)
                _m_lInitDelegateList = new List<Action>(1);
            _m_lInitDelegateList.Add(_action);
        }
        
        private void _callInitDelegate()
        {
            if (_m_lInitDelegateList == null)
                return;
            foreach (Action action in _m_lInitDelegateList)
            {
                action?.Invoke();
            }
            _m_lInitDelegateList.Clear();
        }

        private void _init()
        {
            if (_m_FontAsset == null || _m_FontAsset.material == null)
            {
                _m_bisInited = true;
                _callInitDelegate();
                return;
            }
            
            long serialize = ++_m_lInitSerialize;
            
            _unloadMaterialResList();

            Material fontAssetMaterial = _m_FontAsset.material;
            _m_lMaterialResList.Add(fontAssetMaterial);
            
            Texture fontAssetMatMainTexture = fontAssetMaterial.GetTexture(TMPro.ShaderUtilities.ID_MainTex);

            string materialBelongToTMPFontAssetTag = _m_FontAsset.name.Split(materialBelongToTMPFontAssetTagSplit).FirstOrDefault()?.Replace("(Clone)", "");
            Func<string, bool> checkAssetCanLoad = (string assetPath) =>
             {
                 if(string.IsNullOrEmpty(assetPath))
                     return false;
                     
                 string splitAssetName = assetPath.Split( new char[]{'/', '\\'}).LastOrDefault();//获取文件名
                 if(string.IsNullOrEmpty(splitAssetName) || !splitAssetName.EndsWith(".mat"))//只加载mat文件, 不加载其他类型文件(因为TMP_FontAsset资源也能作为mat加载出来)
                     return false;
                 
                 splitAssetName = splitAssetName.Split(materialBelongToTMPFontAssetTagSplit).FirstOrDefault();//取文件名的第一个空格前字符串与materialBelongToTMPFontAssetTag进行比较, 必须完全相同
                 if(string.IsNullOrEmpty(splitAssetName) || String.Compare(splitAssetName, materialBelongToTMPFontAssetTag, StringComparison.Ordinal) != 0)
                     return false;

                 return true;
             };
            
#if UNITY_EDITOR//在Editor下的查找方法, 仿造TMP_EditorUtility.FindMaterialReferences
            // Get materials matching the search pattern.
            string searchPattern = $"t:Material {materialBelongToTMPFontAssetTag}";
            string[] materialAssetGUIDs = UnityEditor.AssetDatabase.FindAssets(searchPattern);
            
            for (int i = 0; i < materialAssetGUIDs.Length; i++)
            {
                string materialPath = UnityEditor.AssetDatabase.GUIDToAssetPath(materialAssetGUIDs[i]);
                if (!checkAssetCanLoad(materialPath))//只加载mat文件, 不加载其他类型文件(虽然上面用了t:Material进行过滤, 但是TMP_FontAsset资源也能作为mat加载出来)
                    continue;
                
                Material targetMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if(targetMaterial == null)
                    continue;
            
                Texture targetMaterialMainTexture = targetMaterial.GetTexture(TMPro.ShaderUtilities.ID_MainTex);
                
                if (targetMaterial.HasProperty(TMPro.ShaderUtilities.ID_MainTex))//这里不判断贴图是否相同, 因为通过代码加载出来的MaMaterial贴图和FontAsset上的MaMaterial上的贴图不同, 后面_makeMaterialInstantiate会去做替换
                {
                    if (!_m_lMaterialResList.Contains(targetMaterial))
                        _m_lMaterialResList.Add(targetMaterial);
                }
                else
                {
                    // TODO: Find a more efficient method to unload resources.
                    targetMaterialMainTexture = null;
                    Resources.UnloadAsset(targetMaterial.GetTexture(TMPro.ShaderUtilities.ID_MainTex));
                }
            }

            _makeMaterialInstantiate();
            
            Resources.UnloadUnusedAssets();
            _m_bisInited = true;
            _callInitDelegate();
            return;

#else       //不在editor下的查找方法

            GameResCore.instance.loadSynAsset(_m_sAssetPath, (_isSuc, _assetObj) =>
            {
                if(serialize != _m_lInitSerialize)
                    return;
                
                if (!_isSuc || _assetObj ==  null || _assetObj.assetBundle == null)
                {
                    _m_bisInited = true;
                    _callInitDelegate();
                    return;
                }
            
                string[] assetObjNameList = _assetObj.assetBundle.GetAllAssetNames();
                for (int i = 0; i < assetObjNameList.Length; i++)
                {
                    string assetName = assetObjNameList[i];
                    if(!checkAssetCanLoad(assetName))
                        continue;
                    
                    Material targetMaterial = _assetObj.load<Material>(assetName);
                    if(targetMaterial == null)
                        continue;
                    
                    if (targetMaterial.HasProperty(TMPro.ShaderUtilities.ID_MainTex))//这里不判断贴图是否相同, 因为通过代码加载出来的MaMaterial贴图和FontAsset上的MaMaterial上的贴图不同, 后面_makeMaterialInstantiate会去做替换
                    {
                        if (!_m_lMaterialResList.Contains(targetMaterial))
                            _m_lMaterialResList.Add(targetMaterial);
                    }
                    else
                    {
                        // TODO: Find a more efficient method to unload resources.
                        Resources.UnloadAsset(targetMaterial.GetTexture(TMPro.ShaderUtilities.ID_MainTex));
                    }
                }

                _makeMaterialInstantiate();
                
                Resources.UnloadUnusedAssets();
                _m_bisInited = true;
                _callInitDelegate();
                return;
                
            }, null);
#endif
        }

        private void _makeMaterialInstantiate()
        {
            _unloadMaterialInstantiateList();

            if(_m_FontAsset == null || _m_FontAsset.material == null)
                return;
            
            Material fontAssetMaterial = _m_FontAsset.material;
            Texture fontAssetMatMainTexture = fontAssetMaterial.GetTexture(TMPro.ShaderUtilities.ID_MainTex);
            
            for (int i = _m_lMaterialResList.Count - 1; i >= 0; i--)
            {
                Material materialRes = _m_lMaterialResList[i];
                if(materialRes == null)
                    continue;
                
                string tag = getTMPMaterialTag(materialRes);
                if (string.IsNullOrEmpty(tag))
                {
                    Debug.LogError($"[TMP_FontAssetMaterialObj _makeMaterialInstantiate] Material的name:{materialRes.name}不符合规范, material名称中应该包含该material的类型标识(名称中最后一个空格后的字符串为标识)", materialRes);
                    continue;
                }

                if (_m_dMaterialInstantiateDic.TryGetValue(tag, out Material materialInstantiate) && materialInstantiate != null)
                {
                    Debug.LogError($"[TMP_FontAssetMaterialObj _makeMaterialInstantiate] FontAsset:{_m_FontAsset} 找到了两个相同tag的Material : {materialRes.name} 和 {materialInstantiate.name}", _m_FontAsset);
                    continue;
                }

                materialInstantiate = Material.Instantiate(materialRes);
                materialInstantiate.SetTexture(TMPro.ShaderUtilities.ID_MainTex, fontAssetMatMainTexture);//这里把加载出来的Material的贴图替换成了FontAsset上的MaMaterial上的贴图, 因为通过代码加载出来的MaMaterial贴图和FontAsset上的MaMaterial上的贴图

                _m_dMaterialInstantiateDic[tag] = materialInstantiate;
            }
        }
        
        public Material getMaterialInstantiate(string _tag)
        {
            if (_m_dMaterialInstantiateDic.TryGetValue(_tag, out Material materialInstantiate))
            {
                return materialInstantiate;
            }

            return null;
        }
        
        /// <summary>
        /// 获取_material对应的tag
        /// </summary>
        /// <param name="_material"></param>
        /// <returns></returns>
        public static string getTMPMaterialTag(Material _material)
        {
            if(_material == null)
                return "";

            return _material.name.Split(materialTagSplit).LastOrDefault()?.Replace("(Clone)", "");
        }

        public void discard()
        {
            _m_bisInited = false;
            _m_lInitSerialize++;
            _m_FontAsset = null;
            
            _m_lInitDelegateList?.Clear();
            _m_lInitDelegateList = null;
            
            _unloadMaterialInstantiateList();
            _unloadMaterialResList();

            Resources.UnloadUnusedAssets();
        }
        
        private void _unloadMaterialInstantiateList()
        {
            foreach (Material material in _m_dMaterialInstantiateDic.Values)
            {
                if (material != null)
                {
                    Resources.UnloadAsset(material.GetTexture(ShaderUtilities.ID_MainTex));
                    Material.Destroy(material);
                }
            }
            _m_dMaterialInstantiateDic.Clear();
        }
        
        private void _unloadMaterialResList()
        {
            foreach (Material material in _m_lMaterialResList)
            {
                if (material != null)
                {
                    Resources.UnloadAsset(material.GetTexture(ShaderUtilities.ID_MainTex));
                    Resources.UnloadAsset(material);
                }
            }
            _m_lMaterialResList.Clear();
        }
    }
}