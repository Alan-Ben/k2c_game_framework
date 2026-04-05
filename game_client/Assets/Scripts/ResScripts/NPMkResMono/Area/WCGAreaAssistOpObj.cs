using System;
using System.Collections.Generic;
using UnityEngine;

using ALPackage;

/*****************
 * 辅助操作区域控制对象
 **/
public class WCGAreaAssistOpObj
{
    //对应信息对象
    private int _m_iAreaId;
    //对应辅助对象队列
    private List<WCGMapAreaTagMono> _m_lGoList;

    public WCGAreaAssistOpObj(int _areaId)
    {
        _m_iAreaId = _areaId;
        _m_lGoList = new List<WCGMapAreaTagMono>();
    }

    public int areaId { get { return _m_iAreaId; } }

    /****************
     * 注册控制对象
     **/
    public void regGo(WCGMapAreaTagMono _go)
    {
        if (null == _go)
            return;

        _m_lGoList.Add(_go);
    }

    /*************
     * 根据带入的函数遍历所有操作对象进行处理
     **/
    public void refreshAllAssistObj(Func<int, bool> _judgeAreaEnableFunc, bool _isSkillView)
    {
        bool enable = _judgeAreaEnableFunc == null ? false : _judgeAreaEnableFunc(_m_iAreaId);

        WCGMapAreaTagMono tmpObj = null;
        for (int i = 0; i < _m_lGoList.Count; i++)
        {
            tmpObj = _m_lGoList[i];
            if (null == tmpObj)
                continue;

            bool needShow = (tmpObj.isAreaEnableTag == enable);
            ALUGUICommon.setGameObjEnable(tmpObj, needShow);

            //如果显示则处理视图信息
            if(needShow)
            {
                for(int n = 0; n < tmpObj.onSkillViewDisable.Count; n++)
                {
                    ALUGUICommon.setGameObjEnable(tmpObj.onSkillViewDisable[n], !_isSkillView);
                }
            }
        }
    }

    /*************
     * 隐藏所有对象
     **/
    public void hideAll()
    {
        WCGMapAreaTagMono tmpObj = null;
        for (int i = 0; i < _m_lGoList.Count; i++)
        {
            tmpObj = _m_lGoList[i];
            if (null == tmpObj)
                continue;

            ALUGUICommon.setGameObjEnable(tmpObj, false);
        }
    }

    /**************
     * 释放所有数据
     **/
    public void discard()
    {
        _m_lGoList.Clear();
    }
}
