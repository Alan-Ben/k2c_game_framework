using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

/******************
 * WCG战斗数据调整面板统一操作窗口
 **/
public abstract class _ANPBasicEditorWnd : EditorWindow
{
    //子节点队列
    private List<_IBasicEditorItem> _m_lChildItemList;
    private Vector2 _m_OutScrollPosition = Vector2.zero;

    public _ANPBasicEditorWnd()
    {
        _m_lChildItemList = new List<_IBasicEditorItem>();
    }

    //逐个注册显示对象
    public void regChildItem(_IBasicEditorItem _item)
    {
        _m_lChildItemList.Add(_item);
    }

    protected virtual void OnGUI()
    {
//        GUILayout.BeginArea(_getWinRect());
        _m_OutScrollPosition = GUILayout.BeginScrollView(_m_OutScrollPosition, true, true, GUILayout.MinWidth(700));

        //开始纵向布局
        EditorGUILayout.BeginVertical();
        {
            _initBarGUI();

            //逐个对象展示
            for (int i = 0; i < _m_lChildItemList.Count; i++)
            {
                if(_m_lChildItemList[i].needMinWidth)
                    GUILayout.BeginHorizontal(GUILayout.Width(_minWidth));
                else
                    GUILayout.BeginHorizontal();

                //逐个显示
                _m_lChildItemList[i].OnGUI();

                GUILayout.EndHorizontal();
            }
        }
        //结束最外围纵向布局
        EditorGUILayout.EndVertical();
        GUILayout.EndScrollView();

//        GUILayout.EndArea();
    }

    //处理所有对象操作
    public void dealAllItemFunc()
    {
        for (int i = 0; i < _m_lChildItemList.Count; i++)
        {
            _m_lChildItemList[i].dealFunc();
        }
    }

    public void dealAllItem(Action<_IBasicEditorItem> _action)
    {
        if (_m_lChildItemList == null)
            return;
        
        for (int i = 0; i < _m_lChildItemList.Count; i++)
        {
            _action(_m_lChildItemList[i]);
        }
    }

    //初始栏的GUI处理
    protected abstract void _initBarGUI();
    //获取本窗口大小
    protected abstract Rect _getWinRect();
    //最小宽度
    protected abstract int _minWidth { get; }
}
