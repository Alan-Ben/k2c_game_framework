using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndOptions : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //选项字符串数组
    private string[] options;
    //显示宽度
    //private int _m_iShowWidth;
    //头部补充文字
    private Func<string> _m_fAddHeadFunc;
    //设置处理
    private Action<string> _m_dOnSetting;

    private int index = 56;

    public EditorWndOptions(string _name, Func<string[]> _optionsList, Func<string> _addHead, Action<string> _doneAction)
    {
        _m_sName = _name;
        _m_fAddHeadFunc = _addHead;
        //_m_iShowWidth = 0;
        _m_dOnSetting = _doneAction;

        options = _optionsList();
    }
    public EditorWndOptions(string _name, Func<string[]> _optionsList, Func<string> _addHead, int _showWidth, Action<string> _doneAction)
    {
        _m_sName = _name;
        options = _optionsList();
        _m_fAddHeadFunc = _addHead;
        //_m_iShowWidth = _showWidth;
        _m_dOnSetting = _doneAction;
    }

    public string getRealNam() { return (_m_fAddHeadFunc == null ? "" : _m_fAddHeadFunc()) + _m_sName; }
    //是否需要最小宽度
    public bool needMinWidth { get { return true; } }

    string argValue = "2200";//参数
    string cmd = string.Empty;
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        //设置id
        GUILayout.Label(getRealNam());

        GUILayout.BeginHorizontal();
        index = EditorGUILayout.Popup(index, options);

        argValue = GUILayout.TextField(argValue, GUILayout.Width(200));

        cmd = string.Format("{0}:{1}", options[index], argValue);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.Label("完整的tutorial effect cmd");
        GUILayout.Label(cmd);
        //修改按钮
        if (GUILayout.Button(" 修 改 "))
        {
            dealFunc();
        }
        GUILayout.EndVertical();
    }

    //处理函数
    public void dealFunc()
    {
        _m_dOnSetting(cmd);
        UnityEngine.Debug.LogWarning("执行: " + cmd);
        //UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_sValue + " 完成!");
    }
}
