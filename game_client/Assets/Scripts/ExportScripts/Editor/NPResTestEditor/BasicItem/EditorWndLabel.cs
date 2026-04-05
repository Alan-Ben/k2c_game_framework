using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndLabel : _IBasicEditorItem
{
    //对应值
    private string _m_sValue;
    //显示宽度
    private int _m_iShowHeight;

    public EditorWndLabel(string _value, int _height)
    {
        _m_sValue = _value;
        _m_iShowHeight = _height;
    }

    //是否需要最小宽度
    public bool needMinWidth { get { return true; } }

    public void OnGUI()
    {
        //设置id
        GUILayout.Label(_m_sValue, GUILayout.Height(_m_iShowHeight));
    }
    //处理函数
    public void dealFunc()
    {
    }
}
