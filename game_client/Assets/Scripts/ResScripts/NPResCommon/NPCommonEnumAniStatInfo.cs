using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

//常规通用的枚举状态信息
[System.Serializable]
public class NPCommonEnumAniStatInfo<T> where T: Enum
{
    [ALHeader("状态类型")]
    public T type;
    [ALHeader("当前状态显示的go列表")]
    public List<GameObject> goListShow;
    [ALHeader("当前状态隐藏的go列表")]
    public List<GameObject> goListHide;
    [ALHeader("当前状态动画")]
    public Animation animation;
    [ALHeader("当前状态播放的动画名")]
    public string animationName;


    /// <summary>
    /// 设置状态显示
    /// </summary>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    /// <typeparam name="T"></typeparam>
    public static void setStat<E>(List<NPCommonEnumAniStatInfo<E>> _statInfoList, E _type, Action _dealDone = null)
        where E : Enum
    {
        if(_statInfoList == null)
            return;
        
        NPCommonEnumAniStatInfo<E> curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide, false);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
                if (null != curStatInfo.animation && !string.IsNullOrEmpty(curStatInfo.animationName))
                {
                    curStatInfo.animation.Play(curStatInfo.animationName, _dealDone);
                }
                else
                {
                    _dealDone?.Invoke();
                }
                return;
            }
        }
        _dealDone?.Invoke();
    }
    public static void setStatEx<E, C>(List<C> _statInfoList, E _type, Action _dealDone = null)
        where E : Enum
        where C : NPCommonEnumAniStatInfo<E>
    {
        if(_statInfoList == null)
        {
            _dealDone?.Invoke();
            return;
        }
        
        C curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type.Equals(_type))
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide, false);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
                if (null != curStatInfo.animation)
                {
                    curStatInfo.animation.Play(curStatInfo.animationName, _dealDone);
                }
                else
                {
                    _dealDone?.Invoke();
                }
                return;
            }
        }
        _dealDone?.Invoke();
    }

    /// <summary>
    /// 获取状态信息的统一函数
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="_statInfoList"></param>
    /// <param name="_type"></param>
    public static NPCommonEnumAniStatInfo<E> getStat<E>(List<NPCommonEnumAniStatInfo<E>> _statInfoList, E _type)
        where E : Enum
    {
        if(_statInfoList == null)
            return null;
        
        NPCommonEnumAniStatInfo<E> curStatInfo = null;
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
        where C : NPCommonEnumAniStatInfo<E>
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