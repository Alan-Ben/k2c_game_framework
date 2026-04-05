using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndString : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //对应值
    private string _m_sValue;
    //显示宽度
    private int _m_iShowWidth;
    //头部补充文字
    private Func<string> _m_fAddHeadFunc;
    //设置处理
    private Action<string> _m_dOnSetting;

    public EditorWndString(string _name, string _value, Func<string> _addHead, Action<string> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value;
        _m_fAddHeadFunc = _addHead;
        _m_iShowWidth = 0;
        _m_dOnSetting = _doneAction;
    }
    public EditorWndString(string _name, string _value, Func<string> _addHead, int _showWidth, Action<string> _doneAction)
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
        //设置id
        GUILayout.Label(getRealNam());

        if (GUILayout.Button("粘贴", GUILayout.Width(60)))
        {
            _m_sValue = GUIUtility.systemCopyBuffer;
        }

        if (_m_iShowWidth > 0)
            _m_sValue = GUILayout.TextField(_m_sValue, GUILayout.Width(_m_iShowWidth));
        else
            _m_sValue = GUILayout.TextField(_m_sValue);

        //修改按钮
        if (GUILayout.Button(" 修 改 "))
        {
            dealFunc();
        }
    }
    //处理函数
    public void dealFunc()
    {
        _m_dOnSetting(_m_sValue);

        UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_sValue + " 完成!");
    }
}
