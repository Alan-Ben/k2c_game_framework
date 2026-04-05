using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndFloat : _IBasicEditorItem
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
    private Action<float> _m_dOnSetting;

    public EditorWndFloat(string _name, float _value, Func<string> _addHead, Action<float> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value.ToString();
        _m_fAddHeadFunc = _addHead;
        _m_iShowWidth = 0;
        _m_dOnSetting = _doneAction;
    }
    public EditorWndFloat(string _name, float _value, Func<string> _addHead, int _showWidth, Action<float> _doneAction)
    {
        _m_sName = _name;
        _m_sValue = _value.ToString();
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
        //转化值
        float value = 0;
        if (float.TryParse(_m_sValue, out value))
        {
            _m_dOnSetting(value);

            UnityEngine.Debug.LogWarning("设置: " + getRealNam() + " 为 " + _m_sValue + " 完成!");
        }
        else
        {
            UnityEngine.Debug.LogError("设置的值: " + _m_sValue + " 不是浮点型！");
        }
    }
}
