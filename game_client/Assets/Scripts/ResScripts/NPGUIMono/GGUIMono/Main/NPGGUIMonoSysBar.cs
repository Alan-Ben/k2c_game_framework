using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

/// <summary>
/// 电池电量百分比阶段信息
/// </summary>
[System.Serializable]
public class NPGGUISystemPowerStepInfo
{
    public int stepMinPercent;//该阶段最小百分比
    public int stepMaxPercent;//该阶段最大百分比
    public Color showColor;//该阶段进度条颜色

    public bool isInRange(int _percent)
    {
        return _percent <= stepMaxPercent && _percent > stepMinPercent;
    }
}


/// <summary>
/// 战场界面TopBar脚本对象
/// </summary>
public class NPGGUIMonoSysBar : _AALBasicUIWndMono
{
    public List<Graphic> chgColorGraphicList;

    public Text txtSystemTime;//系统时间（24小时制，时：分：秒）

    public GameObject GoSystemPower;//剩余电量整体对象
    public Image ImgSystemPowerProcess;//剩余电量进度条
    public Text txtSystemPowerPercent;//剩余电量百分比文本
    public GameObject GoCharging;//充电中标识

    public List<NPGGUISystemPowerStepInfo> systemPowerStepList;//剩余电量阶段文本

    //获取对应电量的阶段
    public NPGGUISystemPowerStepInfo getStepInfo(int _power)
    {
        for(int i = 0; i < systemPowerStepList.Count; i++)
        {
            NPGGUISystemPowerStepInfo stepInfo = systemPowerStepList[i];
            if(stepInfo == null)
                continue;

            //百分比在该阶段内
            if(stepInfo.isInRange(_power))
            {
                return stepInfo;
            }
        }

        return null;
    }

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return "gui/battle.unity3d"; } }
    public static string objName { get { return "win_battle_topbar"; } }
}