using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 生成跨域继承适配器
    /// </summary>
    public class ILRuntimeAdapterEditorWnd : EditorWindow
    {
        private string _m_inputNameSpaceText;//输入的命名空间
        private string _m_inputText;//输入的类名
        private string _m_result;//输出结果
        public ILRuntimeAdapterEditorWnd()
        {
            _m_inputNameSpaceText = "GOE";
        }

        protected virtual void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 20;
            GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);
            textFieldStyle.fontSize = 20;
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 20;

            GUIStyle resultStyle = new GUIStyle(GUI.skin.label);
            resultStyle.richText = true;

            EditorGUILayout.BeginVertical();

            GUILayout.Space(15);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("命名空间：", labelStyle, GUILayout.Width(100), GUILayout.Height(30));
                _m_inputNameSpaceText = EditorGUILayout.TextField(_m_inputNameSpaceText, textFieldStyle, GUILayout.Width(380), GUILayout.Height(30));
                EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("类名：", labelStyle, GUILayout.Width(60), GUILayout.Height(30));
                _m_inputText = EditorGUILayout.TextField(_m_inputText, textFieldStyle, GUILayout.Width(420), GUILayout.Height(30));
                EditorGUILayout.EndHorizontal();


            GUILayout.Space(15);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("生成", buttonStyle, GUILayout.Width(490), GUILayout.Height(60)))
                {
                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter($"Assets/Scripts/LogicScripts/ILRuntime/Adapters/{_m_inputText}Adapter.cs"))
                        {
                            Assembly ab = Assembly.Load("Assembly-CSharp");
                            Type targetType = ab.GetType($"{(!string.IsNullOrEmpty(_m_inputNameSpaceText)? (_m_inputNameSpaceText + "."):"")}{_m_inputText}" );
                            if (targetType == null)
                                _m_result= $"<color=red>生成出错，获取到类为空</color>";
                            else
                            {
                                sw.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(targetType, "GOE"));
                                _m_result = $"<color=green>完成！已生成{_m_inputText}Adapter.cs</color>";
                            }
                        }
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        _m_result = $"<color=red>生成出错，e:{e.ToString()}</color>";
                        Debug.LogError(_m_result);
                    }
                }
                EditorGUILayout.EndHorizontal();


            GUILayout.Space(15);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30);
                GUILayout.Label(_m_result, resultStyle);
                EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}
