using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * 基础数据调整窗口
 **/
public class EditorWndObjectArray : _IBasicEditorItem
{
    //名称
    private string _m_sName;
    //对应值
    private List<string> _m_sValue;
    //显示宽度
    private int _m_iShowWidth;

    //参数类型数组
    private List<Type> _m_tpParams;
    //设置处理
    private Action<List<object>> _m_dOnSetting;

    public EditorWndObjectArray(string _name, List<Type> _tpParam, Action<List<object>> _doneAction, int _showWidth = 0)
    {
        _m_sName = _name;
        _m_tpParams = _tpParam;
        _m_sValue = new List<string>();
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

        string tmpValue;
        for (int i = 0; i < _m_tpParams.Count; i++)
        {
            tmpValue = _m_iShowWidth > 0 ? GUILayout.TextField("0", _m_iShowWidth) : GUILayout.TextField("0");
            _m_sValue.Add(tmpValue);
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
        List<object> value = new List<object>(_m_sValue.Count);
        long tmp = 0;
        for (int i = 0; i < _m_sValue.Count; i++)
        {
            if (long.TryParse(_m_sValue[i], out tmp))
            {
                value[i] = tmp;

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
