using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//常规通用的枚举状态信息
[System.Serializable]
public class NPCommonEnumStatInfo<T> where T: Enum
{
    [ALHeader("状态类型")]
    public T type;
    [ALHeader("当前状态显示的go列表")]
    public List<GameObject> goListShow;
    [ALHeader("当前状态隐藏的go列表")]
    public List<GameObject> goListHide;

    [ALHeader("置灰列表")]
    public List<MaskableGraphic> grayList;
    [ALHeader("取消置灰列表")]
    public List<MaskableGraphic> disGrayList;

    /// <summary>
    /// 设置状态显示
    /// </summary>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    /// <typeparam name="T"></typeparam>
    public static void setStat<E>(List<NPCommonEnumStatInfo<E>> _statInfoList, E _type)
        where E : Enum
    {
        if(_statInfoList == null)
            return;
        
        NPCommonEnumStatInfo<E> curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide, false);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
#if NP_GAME
                GGameCommonInfo.grayImage(curStatInfo.grayList);
                GGameCommonInfo.disgrayImage(curStatInfo.disGrayList);
#endif
                break;
            }
        }
    }
    public static void setStatEx<E, C>(List<C> _statInfoList, E _type)
        where E : Enum
        where C : NPCommonEnumStatInfo<E>
    {
        if(_statInfoList == null)
            return;
        
        C curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide, false);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
#if NP_GAME
                GGameCommonInfo.grayImage(curStatInfo.grayList);
                GGameCommonInfo.disgrayImage(curStatInfo.disGrayList);
#endif
                break;
            }
        }
    }

    /// <summary>
    /// 获取状态信息的统一函数
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    public static NPCommonEnumStatInfo<E> getStat<E>(List<NPCommonEnumStatInfo<E>> _statInfoList, E _type)
        where E : Enum
    {
        if(_statInfoList == null)
            return null;
        
        NPCommonEnumStatInfo<E> curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                return curStatInfo;
            }
        }

        return null;
    }
    public static C getStatEx<E, C>(List<C> _statInfoList, E _type)
        where E : Enum
        where C : NPCommonEnumStatInfo<E>
    {
        if(_statInfoList == null)
            return null;
        
        C curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                return curStatInfo;
            }
        }

        return null;
    }
}