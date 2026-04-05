using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndBool : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //对应值
    private bool _m_sValue;
    //显示宽度
    private int _m_iShowWidth;
    //头部补充文字
    private Func<string> _m_fAddHeadFunc;
    //设置处理
    private Action<bool> _m_dOnSetting;

    public EditorWndBool(string _name, bool _value, Func<string> _addHead, Action<bool> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value;
        _m_fAddHeadFunc = _addHead;
        _m_iShowWidth = 0;
        _m_dOnSetting = _doneAction;
    }
    public EditorWndBool(string _name, bool _value, Func<string> _addHead, int _showWidth, Action<bool> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value;
        _m_fAddHeadFunc = _addHead;
        _m_iShowWidth = _showWidth;
        _m_dOnSetting = _doneAction;
    }

    public string getRealNam() { return (_m_fAddHeadFunc == null ? "" : _m_fAddHeadFunc()) + _m_sName; }
    //是否需要最小宽度
    public bool needMinWidth { get { return true; } }

    public void OnGUI()
    {
        string realName = getRealNam();
        //设置id
        if (_m_iShowWidth > 0)
            GUILayout.Label(realName, GUILayout.Width(_m_iShowWidth));
        else
        {
            Vector2 labelSize = GUI.skin.label.CalcSize(new GUIContent(realName));
            GUILayout.Label(realName, GUILayout.Width(labelSize.x));
        }

        _m_sValue = GUILayout.Toggle(_m_sValue, "");

        //修改按钮
        if (GUILayout.Button(" 修 改 "))
        {
            dealFunc();
        }
    }
    //处理函数
    public void dealFunc()
    {
        //转化值
        _m_dOnSetting(_m_sValue);

        UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_sValue + " 完成!");
    }
}
