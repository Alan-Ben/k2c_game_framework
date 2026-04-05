using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndFreeDealer : _IBasicEditorItem
{
    private Action _m_onGUI;

    public EditorWndFreeDealer(Action _onGUI)
    {
        _m_onGUI = _onGUI;
    }

    public void OnGUI()
    {
        if (_m_onGUI != null)
        {
            _m_onGUI();
        }
    }

    public bool needMinWidth { get { return true; } }
    public void dealFunc() { }
}
