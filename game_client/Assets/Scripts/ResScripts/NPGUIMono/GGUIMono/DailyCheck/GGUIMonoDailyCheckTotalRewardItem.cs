using ALPackage;
using System.Collections.Generic;
using UnityEngine;


//签到累计奖励item
public class GGUIMonoDailyCheckTotalRewardItem : _AALBasicUIWndMono
{
    [ALHeader("累计签到展示列表")]
    public List<NPCommonEnumStatInfo<ECommonRewardType>> totalDailyCheckRewardStatList;

    [ALHeader("累计签到最好的奖励")]
    public NPGGUIMonoCommonItem bestItemMono;

    [ALHeader("累计签到天数文本")]
    public TextEx needCheckDaysTxt;

    [ALHeader("累计签到天数文本Key")]
    public string needCheckDaysTxtKey;

    [ALHeader("累计奖励领取按钮")]
    public GameObject totalGetRewardBtn;

    [ALHeader("奖励动画")]
    public Animation rewardAni;
    [ALHeader("可领取奖励动画名称")]
    public string rewardAniCanGetStr;
    [ALHeader("隐藏时动画名称")]
    public string rewardAniHideStr;

    [ALHeader("显示时延迟播放动画时间")]
    public float delayTime =1.5f;
}
