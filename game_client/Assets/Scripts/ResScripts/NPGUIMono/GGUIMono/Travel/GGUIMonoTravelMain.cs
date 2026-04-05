using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;
using UnityEngine.UI;

/// <summary>
/// 情报状态
/// </summary>
public enum ETravelMessStat
{
    NONE,//，
    [InspectorName("没有情报")]
    NO_MESS,//没有情报，
    [InspectorName("有1个情报")]
    HAS_ONE_MESS,//有1个情报
    [InspectorName("有多个情报")]
    HAS_MULTI_MESS,//有多个情报
}

/// <summary>
/// 游历主界面
/// </summary>
///
public class GGUIMonoTravelMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("游历按钮")]
    public GameObject btnTravel;
    // [ALHeader("随机游历音效")]
    // public long randTravelAudioId;
    // [ALHeader("一键游历音效")]
    // public long onekeyTravelAudioId;
    [ALHeader("体力数量显示")]
    public GGUIMonoCommonLazyCDCountResume energyCount;
    [ALHeader("是否一键游历tog")]
    public NPGGUIMonoCommonToggleEx togAllTravel;
    [ALHeader("一键游历解锁状态配置")]
    public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statInfosAllTravel;
    [ALHeader("妃子列表按钮")]
    public GameObject btnConsort;
    [ALHeader("游历的UI动画")]
    public Animation travelAnimation;
    // [ALHeader("开始游历的过度动画名字")]
    // public string enterTravelAniName;
    // [ALHeader("开始游历的过度动画时长")]
    // public float enterTravelAniTime;
    // [ALHeader("结束游历的动画名字")]
    // public string exitTravelAniName;

    [ALHeader("视频播放器")]
    public GGUIMonoSimpleVideo monoVideoPlayer;
    
    [ALHeader("开始游历视频延迟时间(游历地点聚焦完成后)")]
    public float startTravelVideoDelayTimeS;

    [ALHeader("事件center_tip显示延迟时间(落地视频播放后)")]
    public float showEventCenterTipDelayTimeS;

    [ALHeader("首次进入播放的视频")]
    public GVideoClipIndex firstEnterVideoClipIndex;
    [ALHeader("首次进入对话id")]
    public long firstEnterDialogId;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3601); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3601); } }
}
