using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndLongArray : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //参数个数
    private int _m_iEmptyParamNum;
    //对应值
    private List<string> _m_sValue;
    //显示宽度
    private int _m_iShowWidth;

    //设置处理
    private Action<List<long>> _m_dOnSetting;

    public EditorWndLongArray(string _name, int _emptyParamNum, Action<List<long>> _doneAction, int _showWidth = 0)
    {
        _m_sName = _name;
        _m_iEmptyParamNum = _emptyParamNum;
        _m_sValue = new List<string>();

        for (int i = 0; i < _m_iEmptyParamNum; i++)
        {
            _m_sValue.Add("0");
        }

        _m_iShowWidth = _showWidth;
        _m_dOnSetting = _doneAction;
    }

    //是否需要最小宽度
    public bool needMinWidth { get { return true; } }

    public void OnGUI()
    {
        //设置id
        GUILayout.Label(_m_sName);

        GUILayout.BeginHorizontal();

        for (int i = 0; i < _m_sValue.Count; i++)
        {
            _m_sValue[i] = _m_iShowWidth > 0 ? GUILayout.TextField(_m_sValue[i], GUILayout.Width(_m_iShowWidth)) : GUILayout.TextField(_m_sValue[i]);
        }

        //修改按钮
        if (GUILayout.Button(" 修 改 "))
        {
            dealFunc();
        }

        GUILayout.EndHorizontal();
    }

    //处理函数
    public void dealFunc()
    {
        //转化值
        List<long> value = new List<long>(_m_sValue.Count);
        long tmp = 0;
        for (int i = 0; i < _m_sValue.Count; i++)
        {
            if (long.TryParse(_m_sValue[i], out tmp))
            {
                value.Add(tmp);

                UnityEngine.Debug.LogWarning("设置: " + _m_sName + " 为 " + _m_sValue[i] + " 完成!");
            }
            else
            {
                UnityEngine.Debug.LogError("设置的值: " + _m_sValue + " 不是整形！");
                return;
            }
        }

        _m_dOnSetting(value);
    }
}
