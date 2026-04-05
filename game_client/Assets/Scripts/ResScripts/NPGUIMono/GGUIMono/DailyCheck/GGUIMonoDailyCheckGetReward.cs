using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;
using System;
using UnityEngine.UI;


//每日签到奖励窗口
public class GGUIMonoDailyCheckGetReward : _AALBasicUIWndMono
{
    [ALHeader("甜点挂点")]
    public Transform dessertParent;

    [ALHeader("甜点名称")]
    public Text dessertNameTxt;

    [ALHeader("甜点描述")]
    public Text dessertDescTxt;

    [ALHeader("当前累计签到天数")]
    public Text checkDayTxt;

    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer rewardContainerMono;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    /************
   * 资源加载路径
   */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3503); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3503); } }
}
