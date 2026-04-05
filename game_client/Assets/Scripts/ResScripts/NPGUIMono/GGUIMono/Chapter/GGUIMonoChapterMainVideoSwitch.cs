using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using CommonEnum;


public enum EChapterVideoForwardState
{
    [InspectorName("默认状态")]
    NORMAL,
    [InspectorName("单次前进")]
    FORWARD,
    [InspectorName("快速前进")]
    QUICK_FORWARD,
    [InspectorName("BOSS状态")]
    BOSS,
}

[Serializable]
public class ChapterVideoAnimatorController
{
    public int id;
    public RuntimeAnimatorController controller;
}

/// <summary>
/// 关卡界面视频切换界面
/// </summary>
///
public class GGUIMonoChapterMainVideoSwitch : _AALBasicUIWndMono
{
    [ALHeader("不同关卡视频动画控制器列表")]
    public List<ChapterVideoAnimatorController> videoAnimControllerList;
    
    [ALHeader("关卡视频展示A")]
    public GGUIMonoCommonShowCase monoShowcaseA;
    [ALHeader("关卡视频展示B")]
    public GGUIMonoCommonShowCase monoShowcaseB;
    
    [ALHeader("特效挂点A")]
    public Transform sfxParentA;
    [ALHeader("特效挂点B")]
    public Transform sfxParentB;
    
    [ALHeader("切换动画")]
    public Animation switchAnim;
    [ALHeader("切换动画名称2B")]
    public string switchAnimName2B;
    [ALHeader("切换动画名称2A")]
    public string switchAnimName2A;
    [ALHeader("放大动画名称A")]
    public string enlargeAnimNameA;
    [ALHeader("放大动画名称B")]
    public string enlargeAnimNameB;
    [ALHeader("切换Boss动画名称2B")]
    public string switchBossAnimName2B;
    [ALHeader("切换Boss动画名称2A")]
    public string switchBossAnimName2A;
    
    [ALHeader("不同前进状态显示A")]
    public MultiStateShow<EChapterVideoForwardState> forwardStateAnimA;
    [ALHeader("不同前进状态显示B")]
    public MultiStateShow<EChapterVideoForwardState> forwardStateAnimB;
    
    [ALHeader("受击震动的根节点")]
    public RectTransform transShakeRoot;
    [Header("受击震动的剧烈程度")]
    public float fShakeIntensity;
    [Header("受击震动的持续时间")]
    public float fShakeDuration;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2106); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2106); } }
}