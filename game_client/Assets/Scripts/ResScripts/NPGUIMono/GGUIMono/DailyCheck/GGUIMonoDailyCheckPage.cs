using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;
using System;
using UnityEngine.UI;

/// <summary>
///签到类型
/// </summary>
public enum EDailyCheckType
{
    NO_CHECK,//未签到
    IS_CHECK,//已签到
}

//签到窗口
public class GGUIMonoDailyCheckPage : _AALBasicUIWndMono
{
    [ALHeader("妃子形象")]
    public GGUIMonoCommonShowCase consortShowCase;

    [ALHeader("气泡文本")]
    public Text dialogTxt;

    [ALHeader("签到前后显示的Go List")]
    public List<NPCommonEnumStatInfo<EDailyCheckType>> dailyCheckStatList;

    [ALHeader("签到前播放单个甜品动画时间")]
    public float groupPerAniTime = 5.5f;

    [ALHeader("签到前的甜点挂点")]
    public List<Transform> dailyCheckBeforeDessertParent;

    [ALHeader("签到后的甜点挂点")]
    public Transform dailyCheckAfterDessertParent;

    [ALHeader("累计奖励列表")]
    public List<GGUIMonoDailyCheckTotalRewardItem> totalRewardItemList;

    [ALHeader("累计签到天数文本")]
    public TextEx totalDailyCheckDayTxt;

    [ALHeader("累计奖励进度条")]
    public GGUIMonoCommonRoundSlider roundSlider;

    [ALHeader("累计奖励预览pathid")]
    public long rewardPreviewResPathId;

    [ALHeader("累计奖励列表组动画")]
    public Animation totalRewardGroupAni;

    [ALHeader("累计奖励列表组出现动画名称")]
    public string totalRewardGroupAniShowStr;

    [ALHeader("累计奖励列表组移除动画名称")]
    public string totalRewardGroupAniHideStr;

    [ALHeader("显示动画")]
    public Animation showAni;

    [ALHeader("显示动画名称")]
    public string showAniStr;
}
