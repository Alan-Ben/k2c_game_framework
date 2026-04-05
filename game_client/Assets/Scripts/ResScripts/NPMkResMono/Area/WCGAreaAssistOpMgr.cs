using System;
using System.Collections.Generic;
using UnityEngine;

/*****************
 * 辅助操作区域控制对象
 **/
public class WCGAreaAssistOpMgr
{
    private static WCGAreaAssistOpMgr _g_instance = new WCGAreaAssistOpMgr();
    public static WCGAreaAssistOpMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new WCGAreaAssistOpMgr();
            return _g_instance;
        }
    }

    //数据对象列表
    private List<WCGAreaAssistOpObj> _m_lAreaAssistObjList;

    private bool _m_bIsShow;
    //当前是否技能视图
    private bool _m_bCurIsSkillView;

    public bool IsShow { get { return _m_bIsShow; } }

    protected WCGAreaAssistOpMgr()
    {
        _m_lAreaAssistObjList = new List<WCGAreaAssistOpObj>();

        _m_bIsShow = false;
        _m_bCurIsSkillView = false;
    }

    //当展示辅助提示区域时据点变化引起的处理
    public void refreshAllAssistObjWithoutNotChgState(Func<int, bool> _judgeAreaEnableFunc)
    {
        if (!IsShow)
            return;

        if(_m_bCurIsSkillView)
            refreshSkillAllAssistObj(_judgeAreaEnableFunc);
        else
            refreshAllAssistObj(_judgeAreaEnableFunc);
    }

    /***************
     * 根据区域Id获取对应辅助对象控制对象
     **/
    public WCGAreaAssistOpObj getAreaAssistOpObj(int _areaId)
    {
        for(int i = 0; i < _m_lAreaAssistObjList.Count; i++)
        {
            if (_m_lAreaAssistObjList[i].areaId == _areaId)
                return _m_lAreaAssistObjList[i];
        }

        WCGAreaAssistOpObj opObj = new WCGAreaAssistOpObj(_areaId);
        _m_lAreaAssistObjList.Add(opObj);

        return opObj;
    }

    /*************
     * 根据带入的函数遍历所有操作对象进行处理
     **/
    public void refreshAllAssistObj(Func<int, bool> _judgeAreaEnableFunc)
    {
        _m_bIsShow = true;
        _m_bCurIsSkillView = false;

        for (int i = 0; i < _m_lAreaAssistObjList.Count; i++)
        {
            _m_lAreaAssistObjList[i].refreshAllAssistObj(_judgeAreaEnableFunc, false);
        }
    }
    public void refreshSkillAllAssistObj(Func<int, bool> _judgeAreaEnableFunc)
    {
        _m_bIsShow = true;
        _m_bCurIsSkillView = true;

        for (int i = 0; i < _m_lAreaAssistObjList.Count; i++)
        {
            _m_lAreaAssistObjList[i].refreshAllAssistObj(_judgeAreaEnableFunc, true);
        }
    }

    /*************
     * 隐藏所有对象
     **/
    public void hideAll()
    {
        _m_bIsShow = false;

        for (int i = 0; i < _m_lAreaAssistObjList.Count; i++)
        {
            _m_lAreaAssistObjList[i].hideAll();
        }
    }

    /*******
     * 析构函数
     **/
    public void discard()
    {
        for (int i = 0; i < _m_lAreaAssistObjList.Count; i++)
        {
            _m_lAreaAssistObjList[i].discard();
        }
        _m_lAreaAssistObjList.Clear();
    }
}
