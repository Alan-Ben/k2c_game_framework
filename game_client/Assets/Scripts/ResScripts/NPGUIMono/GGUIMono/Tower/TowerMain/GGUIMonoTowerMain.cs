using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoTowerMain : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("当前挑战进度")]
    public TextEx txtLevelName;
    
    [ALHeader("总收益加成")]
    public TextEx txtEarnBonus;
    
    [ALHeader("单日收益爬塔币")]
    public TextEx txtDailyCoins;
    
    [ALHeader("挑战按钮")]
    public GameObject btnChallenge;
    
    [ALHeader("战报按钮")]
    public GameObject btnLog;
    [ALHeader("显示当前详细收益加成")]
    public GameObject btnDetail;
    [ALHeader("研究按钮")]
    public GameObject btnResearch;
    [ALHeader("详细收益加成tips Root")]
    public RectTransform toolTipsRoot;
    [ALHeader("界面滚动控制")]
    public ScrollRect viewScrollRect;
    [ALHeader("移动到指定章节的时间")]
    public float moveToTargetChapterTime = 2;
    [ALHeader("章节按钮列表")]
    public List<GGUIMonoTowerMainChapterItem> chapterItems;
    
    [ALHeader("章节显示移动目标")]
    public RectTransform chapterShowMoveTrans;
    
    [ALHeader("进入章节显示，移动时间")]
    public float chapterShowMoveTime = 1;   
    [ALHeader("退出章节显示，移动时间")]
    public float chapterHideMoveTime = 1;
    [ALHeader("进入章节显示，目标Item的移动目标位置")]
    public RectTransform chapterTargetTrans;
    [ALHeader("进入章节显示，隐藏gos的延迟时间")]
    public float chapterStarShowHideGosTime = 1;
    [ALHeader("退出章节显示，显示gos的延迟时间")]
    public float chapterEndShowShowGosTime = 1;
    [ALHeader("进入章节，隐藏gos")]
    public List<GameObject> chapterShowHideGos;
    [ALHeader("进入章节，关闭的RectMask")]
    public RectMask2D chapterShowDisableMask;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(5300); } }
    public static string objName { get { return UIResPathAssistant.getObjName(5300);} }
}