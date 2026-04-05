using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndEnum : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //对应值
    private int _m_iEnumValue;
    //枚举类型
    private Type _m_tEnumType;
    //头部补充文字
    private Func<string> _m_fAddHeadFunc;
    //获取显示信息的函数
    private Func<int, string> _m_fGetShowValueStr;
    //设置处理
    private Action<int> _m_dOnSetting;

    //是否在选择状态
    private bool _m_bIsSelecting;
    //在选择的结果
    private int _m_iSelectingType;

    public EditorWndEnum(string _name, int _initValue, Type _enumType, Func<string> _addHead, Func<int, string> _getShowStr, Action<int> _doneAction)
    {
        _m_sName = _name;
        _m_iEnumValue = _initValue;
        _m_tEnumType = _enumType;
        _m_fAddHeadFunc = _addHead;
        _m_fGetShowValueStr = _getShowStr;
        _m_dOnSetting = _doneAction;

        _m_bIsSelecting = false;
        _m_iSelectingType = 0;
    }

    public string getRealNam() { return (_m_fAddHeadFunc == null ? "" : _m_fAddHeadFunc()) + _m_sName; }
    //是否需要最小宽度
    public bool needMinWidth { get { return true; } }

    public void OnGUI()
    {
        //设置id
        GUILayout.Label(getRealNam());
        if (_m_bIsSelecting)
        {
            GUILayout.BeginVertical();
            _m_iSelectingType = GUILayout.SelectionGrid(_m_iSelectingType, Enum.GetNames(_m_tEnumType), 1, GUILayout.Width(100));
            if (_m_iSelectingType != _m_iEnumValue)
            {
                _m_bIsSelecting = false;
                _m_iEnumValue = _m_iSelectingType;
            }
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.Label(_m_fGetShowValueStr(_m_iEnumValue));
            if (GUILayout.Button("+", GUILayout.Width(40)))
            {
                _m_bIsSelecting = true;
                _m_iSelectingType = _m_iEnumValue;
            }
        }

        //修改按钮
        if(GUILayout.Button(" 修 改 "))
        {
            dealFunc();
        }
    }
    //处理函数
    public void dealFunc()
    {
        _m_dOnSetting(_m_iEnumValue);

        UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_iEnumValue + " 完成!");
    }
}
