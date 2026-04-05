using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [CustomEditor(typeof(TextEx))]
    public class TextExEditor : Editor
    {
        Dictionary<string, string> replaceMap = new Dictionary<string, string>()
        {
            {"加粗", "<b>{0}</b>"},
            {"倾斜", "<i>{0}</i>"},
            {"大小", "<size={1}>{0}</size>"},
            {"颜色", "<color=#{1}>{0}</color>"},
        };
        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        protected static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);

        private int size;
        private Color color = Color.black;

        private void OnEnable()
        {
            _initFontType();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            
            _DrawFontTypeAndUseLanguage();
            base.OnInspectorGUI();
            TextEx widget = target as TextEx;
            if (GUILayout.Button("加粗", GUILayout.Height(30)))
            {
                doReplaceSelectedText("加粗");
            }
            if (GUILayout.Button("倾斜", GUILayout.Height(30)))
            {
                doReplaceSelectedText("倾斜");
            }
            GUILayout.BeginHorizontal();
            size = EditorGUILayout.IntField(size);
            if (GUILayout.Button("大小", GUILayout.Height(30)))
            {
                doReplaceSelectedText("大小", size.ToString());
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            color = EditorGUILayout.ColorField(color);
            if (GUILayout.Button("颜色", GUILayout.Height(30)))
            {
                doReplaceSelectedText("颜色", ColorUtility.ToHtmlStringRGBA(color));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("替换\\n（删除\\r)", GUILayout.Height(30)))
            {
                widget.text = widget.text.Replace("\r", "");//删除\r
                if (widget.text.Contains("\n"))
                {
                    widget.text = widget.text.Replace("\n", "\\n");
                }
                else
                {
                    widget.text = widget.text.Replace("\\n", "\n");
                }
            }
            GUILayout.EndHorizontal();
            
            _CheckFontTypeFontChange();
        }


        void doReplaceSelectedText(string _replaceType, params string[] _params)
        {
            if (tryGetSelectedText(out string _selectedText))
            {
                List<string> list = new List<string>();
                list.Add(_selectedText);
                list.AddRange(_params);
                replaceText(_replaceType, list.ToArray());
            }
        }

        bool tryGetSelectedText(out string _selectedText)
        {
            TextEditor tEditor = typeof(EditorGUI).GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as TextEditor;

            if (tEditor != null)
            {
                _selectedText = tEditor.SelectedText;
                return !string.IsNullOrEmpty(_selectedText);
            }
            _selectedText = null;
            return false;
        }

        void replaceText(string _replaceType, params string[] _text)
        {
            string pattern;
            if (replaceMap.TryGetValue(_replaceType, out pattern))
            {
                TextEditor tEditor = typeof(EditorGUI).GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as TextEditor;
                if (tEditor != null)
                {
                    tEditor.ReplaceSelection(string.Format(pattern, _text));
                    TextEx widget = target as TextEx;
                    widget.text = tEditor.text;//没设置的话，修改没有效果（阿里老司机带飞修复）
                }
            }
            else
            {
                Debug.Log($"没有实现pattern:{_replaceType}");
            }

        }

        #region FontType

        protected SerializedProperty m_FontTypeProperty;
        protected Font _m_initFont;//初始字体
        
        private void _initFontType()
        {
            m_FontTypeProperty = serializedObject.FindProperty("m_fontType");

            var textEx = (Text)target;
            _m_initFont = textEx == null ? null : textEx.font;

            if (!Application.isPlaying)
            {
                _chgFont(target);//在游戏没有运行时enable时, 刷新一次使用的字体
            }
        }

        private void _DrawFontTypeAndUseLanguage()
        {
            if (!Application.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_FontTypeProperty);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    _chgFont(target);
                }
                    
                ENPLanguage textUseLanguage = (ENPLanguage)EditorPrefs.GetInt("textUseLanguage", 0);
                EditorGUI.BeginChangeCheck();
                textUseLanguage = (ENPLanguage)EditorGUILayout.EnumPopup("使用语言", textUseLanguage);
                if (EditorGUI.EndChangeCheck())
                {
                    EditorPrefs.SetInt("textUseLanguage", (int)textUseLanguage);
                    _chgFont(target);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(m_FontTypeProperty);
                EditorGUI.EndDisabledGroup();
            }
        }
        /// <summary>
        ///  检查字体是否被修改
        /// </summary>
        private void _CheckFontTypeFontChange()
        {
            if (!Application.isPlaying)
            {
                var text = target as Text;
                if (text != null && text.font != _m_initFont)
                {
                    Debug.LogError_EditorOnly("TextEx 不允许手动修改Font, 请通过变换FontType或使用语言修改需要使用的Font", text);
                    text.font = _m_initFont;
                    EditorUtility.SetDirty(target);
                }
            }
        }

        private void _chgFont(UnityEngine.Object target)
        {
            if(Application.isPlaying)//游戏运行时不刷新字体
                return;
            
            Text text = target as Text;
            if(text == null || m_FontTypeProperty == null)
                return;
            EFontType fontType = (EFontType) m_FontTypeProperty.enumValueIndex;
            Font font = FontTypeUtil.getLanguageFont(target, fontType);
            _m_initFont = font;
            text.font = font;
            EditorUtility.SetDirty(target);
            Resources.UnloadUnusedAssets();
        }
        #endregion

        protected static Font getDefaultFont()
        {
            return Resources.Load<Font>("GUI/font/font_content");
        }

        [MenuItem("GameObject/UI/TextEx", false, 2001)]
        private static void addTextExComp(MenuCommand _menuCommand)
        {
            GameObject go = UIEditorHelp.CreateUIElementRoot("TextEx", s_ThickElementSize);
        
            TextEx textEx = go.AddComponent<TextEx>();
            if(textEx != null)
            {
                textEx.raycastTarget = false;
                Font defaultFont = getDefaultFont();
                if(defaultFont != null)
                    textEx.font = defaultFont;
            }
            UIEditorHelp.PlaceUIElementRoot(go, _menuCommand);
        }
        
        [MenuItem("Component/UI/TextEx", false, 2001)]
        private static void addComp()
        {
            foreach (var obj  in Selection.objects )
            {
                var go = obj as GameObject;
                if(go != null)
                {
                    TextEx textEx = go.AddComponent<TextEx>();
                    if(textEx != null)
                    {
                        textEx.raycastTarget = false;
                        Font defaultFont = getDefaultFont();
                        if(defaultFont != null)
                            textEx.font = defaultFont;
                    }
                }
            }
        }
        
    }
}