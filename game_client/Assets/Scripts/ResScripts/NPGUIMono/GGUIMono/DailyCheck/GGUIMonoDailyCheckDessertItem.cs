using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine.UI;


//签到甜点item
public class GGUIMonoDailyCheckDessertItem : _AALBasicUIWndMono
{
    [ALHeader("签到前后显示的Go List")]
    public List<NPCommonEnumStatInfo<EDailyCheckType>> dailyCheckStatList;

    [ALHeader("需要播放的动画")]
    public Animation selectAni;

    [ALHeader("选择甜点播放动画")]
    public string dailyCheckSelectAniStr;

    [ALHeader("甜点详情播放的动画名称")]
    public string dailyCheckDetailAniStr;

    [ALHeader("甜点循环播放的动画")]
    public Animation groupAni;

    [ALHeader("甜点循环播放动画名称")]
    public string groupAniStr;

    [ALHeader("点击按钮")]
    public GameObject clickGo;
}
