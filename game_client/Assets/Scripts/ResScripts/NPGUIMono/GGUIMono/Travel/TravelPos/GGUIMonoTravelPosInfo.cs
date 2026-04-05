using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 情报地点信息界面
/// </summary>
public class GGUIMonoTravelPosInfo : _AALBasicUIWndMono
{
    [ALHeader("地点icon")]
    public RawImage icon;
    [ALHeader("地点名")]
    public TextEx txtPosName;
    [ALHeader("地点描述")]
    public TextEx txtPosDesc;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;

    [ALHeader("解锁条件描述")]
    public TextEx txtUnlockConditionDesc;
    [ALHeader("解锁状态展示信息列表")]
    public List<NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>> unlockStatShowInfo;

    [ALHeader("事件容器")]
    public GGUIMonoTravelPosEventContainer monoEventContainer;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3619); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3619); } }
}