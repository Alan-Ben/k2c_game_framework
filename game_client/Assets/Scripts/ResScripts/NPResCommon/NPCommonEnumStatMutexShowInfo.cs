using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

//常规通用的枚举状态信息
[System.Serializable]
public class NPCommonEnumStatMutexShowInfo<T> where T: Enum
{
    [ALHeader("状态类型")]
    public T type;
    [ALHeader("当前状态显示的go列表")]
    public List<GameObject> goListShow;

    [ALHeader("置灰列表")]
    public List<MaskableGraphic> grayList;

    /// <summary>
    /// 设置状态显示
    /// </summary>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    /// <typeparam name="T"></typeparam>
    public static void setStat<E>(List<NPCommonEnumStatMutexShowInfo<E>> _statInfoList, E _type)
        where E : Enum
    {
        if(_statInfoList == null)
            return;
        
        NPCommonEnumStatMutexShowInfo<E> curStatInfo = null;
        NPCommonEnumStatMutexShowInfo<E> tmpStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            tmpStatInfo = _statInfoList[i];
            if (tmpStatInfo.type.Equals(_type))
            {
                curStatInfo = tmpStatInfo;
            }
            else
            {
                ALUGUICommon.setGameObjEnable(tmpStatInfo.goListShow, false);
#if NP_GAME
                GGameCommonInfo.grayImage(tmpStatInfo.grayList, false);
#endif
            }
        }

        if (curStatInfo != null)
        {
            ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
#if NP_GAME
            GGameCommonInfo.grayImage(curStatInfo.grayList, true);
#endif
        }
    }
    public static void setStatEx<E, C>(List<C> _statInfoList, E _type)
        where E : Enum
        where C : NPCommonEnumStatMutexShowInfo<E>
    {
        if(_statInfoList == null)
            return;
        
        C curStatInfo = null;
        C tmpStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            tmpStatInfo = _statInfoList[i];
            if (tmpStatInfo.type.Equals(_type))
            {
                curStatInfo = tmpStatInfo;
            }
            else
            {
                ALUGUICommon.setGameObjEnable(tmpStatInfo.goListShow, false);
#if NP_GAME
                GGameCommonInfo.grayImage(tmpStatInfo.grayList, false);
#endif
            }
        }

        if (curStatInfo != null)
        {
            ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
#if NP_GAME
            GGameCommonInfo.grayImage(curStatInfo.grayList, true);
#endif
        }
    }

    /// <summary>
    /// 获取状态信息的统一函数
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    public static NPCommonEnumStatMutexShowInfo<E> getStat<E>(List<NPCommonEnumStatMutexShowInfo<E>> _statInfoList, E _type)
        where E : Enum
    {
        if(_statInfoList == null)
            return null;
        
        NPCommonEnumStatMutexShowInfo<E> curStatInfo = null;
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
        where C : NPCommonEnumStatMutexShowInfo<E>
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