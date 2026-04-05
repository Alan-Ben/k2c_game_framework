using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using CommonEnum;

public enum EChapterWndShowState
{
    [InspectorName("默认状态")]
    NORMAL,
    [InspectorName("boss战")]
    BOSS,
    [InspectorName("处理对话")]
    DEAL_DIALOG,
    [InspectorName("处理事件")]
    DEAL_EVENT,
    [InspectorName("boss战对话")]
    BOSS_DIALOG,
}


/// <summary>
/// 关卡主界面
/// </summary>
///
public class GGUIMonoChapterMain : _ANPBasicUIWndResBarMono
{
    [ALHeader("关卡进度条")]
    public Slider sldChapter;
    [ALHeader("关卡进度文本")]
    public Text txtSld;
    
    [ALHeader("进度条上的气泡容器")]
    public GGUIMonoChapterMainDialogSliderTipContainer dialogSliderTipContainer;
    
    [ALHeader("章节名")]
    public Text txtChapterName;
    [ALHeader("关卡标题")]
    public Text txtTitle;
    [ALHeader("当前关卡消耗描述")]
    public Text txtDesc;
    [ALHeader("当前战力")]
    public Text txtPower;
    [ALHeader("大臣经验展示")]
    public NPGGUIMonoCommonItem heroExpItem;
    [ALHeader("boos头像（关卡icon）")]
    public RawImage imgBossIcon;
    
    [ALHeader("boos战前tip头像")]
    public RawImage imgTipBossIcon;
    [ALHeader("boos战前tip名字")]
    public Text txtBossName;
    
    [ALHeader("窗口动画")]
    public Animation wndAni;
    [ALHeader("默认动画名称")]
    public string defaultWndAnimName;
    [ALHeader("boss战开始动画名称")]
    public string startBossAnimName;
    [ALHeader("node切换动画名称起始")]
    public string nodeTurnAnimNameStart;
    
    [ALHeader("boss战对话按钮")]
    public GameObject btnBossDialog;
    
    [ALHeader("暴击动画动画")]
    public Animation wndCritAni;
    [ALHeader("暴击动画名字")]
    public string critAniName;
    
    [ALHeader("前进消耗")]
    public NPGGUIMonoCommonItem costItem;
    
    [ALHeader("前进按钮")]
    public GameObject btnForward;
    [ALHeader("快速前进复选框")]
    public NPGGUIMonoCommonToggleEx btnQuickForward;
    
    [ALHeader("无消耗时候显示的go列表")]
    public List<GameObject> noneCostShowGoList;
    [ALHeader("无消耗时候隐藏的go列表")]
    public List<GameObject> noneCostHideGoList;
    
    [ALHeader("暴击特效父节点")]
    public Transform sfxParent;
    [ALHeader("非暴击特效id")]
    public long defaultSfxId;
    [ALHeader("暴击特效id")]
    public long critSfxId;
    
    [ALHeader("粒子上漂起点")]
    public RectTransform particleRectTransform;
    [ALHeader("前进屏幕特效父节点")]
    public Transform screenSfxParent;
    [ALHeader("视频倍速时间(秒)")]
    public float videoSpeedTime;

    [ALHeader("鼓舞子窗口")]
    public GGUIMonoChapterMain_SubWndInspire subWndInspire;
    
    [ALHeader("不同状态显示隐藏的的go列表")]
    public List<NPCommonEnumStatInfo<EChapterWndShowState>> wndShowState;
    [ALHeader("粒子id")]
    public long specialParticleId;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
 
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2100); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2100); } }
}