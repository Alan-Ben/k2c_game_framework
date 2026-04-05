using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndTextArea : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //对应值
    private string _m_sValue;
    //显示宽度
    private int _m_iShowHeight;
    //头部补充文字
    private Func<string> _m_fAddHeadFunc;
    //设置处理
    private Action<string> _m_dOnSetting;

    public EditorWndTextArea(string _name, string _value, Func<string> _addHead, Action<string> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value;
        _m_fAddHeadFunc = _addHead;
        _m_iShowHeight = 0;
        _m_dOnSetting = _doneAction;
    }
    public EditorWndTextArea(string _name, string _value, Func<string> _addHead, int _showHeight, Action<string> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value;
        _m_fAddHeadFunc = _addHead;
        _m_iShowHeight = _showHeight;
        _m_dOnSetting = _doneAction;
    }

    public string getRealNam() { return (_m_fAddHeadFunc == null ? "" : _m_fAddHeadFunc()) + _m_sName; }
    //是否需要最小宽度
    public bool needMinWidth { get { return false; } }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                //设置id
                GUILayout.Label(getRealNam(), GUILayout.MinWidth(100));

                if (GUILayout.Button("粘贴", GUILayout.MinWidth(60)))
                {
                    _m_sValue = GUIUtility.systemCopyBuffer;
                }

                //修改按钮
                if (GUILayout.Button(" 修 改 ", GUILayout.MinWidth(100)))
                {
                    dealFunc();
                }
            }
            GUILayout.EndHorizontal();

            if (_m_iShowHeight > 0)
                _m_sValue = GUILayout.TextArea(_m_sValue, GUILayout.Height(_m_iShowHeight));
            else
                _m_sValue = GUILayout.TextArea(_m_sValue);
        }
        GUILayout.EndVertical();
    }
    //处理函数
    public void dealFunc()
    {
        _m_dOnSetting(_m_sValue);

        UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_sValue + " 完成!");
    }
}
