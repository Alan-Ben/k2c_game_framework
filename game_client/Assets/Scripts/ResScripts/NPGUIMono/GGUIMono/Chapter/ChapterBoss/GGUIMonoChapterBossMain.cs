using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

public enum EChapterBossWndType
{
    [InspectorName("无状态，异常状态，不需要配")]
    NONE,//无状态
    [InspectorName("开始大臣表现状态")]
    SHOW,//准备开启状态
    [InspectorName("表现完成状态")]
    END,//表现完成状态
    [InspectorName("等待服务器响应状态，不需要配")]
    LOADING,//等待服务器响应状态
    [InspectorName("奖励状态")]
    REWARD,//奖励状态
    [InspectorName("异常状态，不需要配")]
    FAIL,//异常状态
}

[System.Serializable]
public class GGUIMonoChapterBossMain_settings
{
    [ALHeader("血条跳字动画名字")]
    public string hpTipAniName;
        
    [ALHeader("大臣出现间隔(秒)")]
    public float heroInspireInterval = 1f;
    [ALHeader("2阶段大臣出现间隔(秒)")]
    public float heroInspireIntervalSecond = 0.5f;
    [ALHeader("最后一击持续时间(秒)")]
    public float lastHitTime = 1f;

    [ALHeader("大臣出现动画名字左")]
    public string heroInspireShowAniName;
    [ALHeader("大臣出现动画名字右")]
    public string heroInspireShowAniNameRight;
    
    [ALHeader("最后一击大臣出现动画名字左")]
    public string lastHitHeroInspireShowAniName;
    [ALHeader("最后一击大臣出现动画名字右")]
    public string lastHitHeroInspireShowAniNameRight;
    
    [ALHeader("表现结束状态持续事件(秒)")]
    public float endBubbleTime = 1f;
    [ALHeader("奖励状态持续事件(秒)")]
    public float rewardStateTime = 1f;
}


/// <summary>
/// 
/// </summary>
public class GGUIMonoChapterBossMain : _AALBasicUIWndMono
{
    [ALHeader("初始视频动画名字(好像不需要了)")]
    public string videoNameStr;
    [ALHeader("结束视频动画名字")]
    public string endVideoNameStr;
    [ALHeader("boss2阶段视频动画名字")]
    public string bossSecondVideoNameStr;
    
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer monoRewardContainer;
    
    [ALHeader("跳过按钮")]
    public GameObject btnJump;
    
    [ALHeader("boos头像")]
    public RawImage imgBossIcon;
    [ALHeader("boss战力")]
    public Text txtBossPower;
    [ALHeader("boss名字")]
    public Text txtBossName;
    
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("伙伴总战力")]
    public TextEx txtPlayerPower;
    
    [ALHeader("boss血量总文本")]
    public Text txtBossHp;
    [ALHeader("Boss血条")]
    public GGUIMonoCommonBlood bossHPSlider;
    [ALHeader("Boss血条2阶段")]
    public GGUIMonoCommonBlood bossHPSliderSecond;
    [ALHeader("一阶段受击表现时间(秒)")]
    public float hitTimeFirstS;
    [ALHeader("二阶段受击表现时间(秒)")]
    public float hitTimeSecondS;
    
    [ALHeader("血跳字预制体")]
    public GGUIMonoChapterBossHPTip hpTipPrefab;
    [ALHeader("血跳字chache父节点")]
    public Transform hpTipCacheRoot;
    [ALHeader("血跳字父节点")]
    public Transform hpTipParent;

    [ALHeader("大臣预制体")]
    public GGUIMonoChapterBossHeroInspire heroInspirePrefab;
    [ALHeader("大臣预制体Cache父节点")]
    public Transform inspireCacheRoot;
    [ALHeader("大臣表现父节点")]
    public Transform heroShowParent;
  
    [ALHeader("表现配置（默认）")]
    public GGUIMonoChapterBossMain_settings defaultSettings;
    
    [ALHeader("表现结束状态要播放的窗口动画")]
    public Animation endAnimation;
    [ALHeader("表现结束状态要播放的窗口动画名字")]
    public string endAnimationName;
    [ALHeader("2阶段过场状态要播放的窗口动画名字")]
    public string secondAnimationName;

    [ALHeader("不同状态显示的go列表")]
    public List<NPCommonEnumStatInfo<EChapterBossWndType>> stateShow;
    [ALHeader("领奖状态下的关闭按钮")]
    public GameObject btnClose;
    
    [ALHeader("开始战斗的延迟时间(秒)")]
    public float startVBattleDelayS = 1f;
    [ALHeader("1阶段大臣数占比")]
    public float firstStageHeroCountFade = 0.5f;
    
    [ALHeader("受击震动的根节点")]
    public RectTransform transShakeRoot;
    [Header("受击震动的剧烈程度")]
    public float fShakeIntensity;
    [Header("受击震动的持续时间")]
    public float fShakeDuration;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2102); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2102);} }
}