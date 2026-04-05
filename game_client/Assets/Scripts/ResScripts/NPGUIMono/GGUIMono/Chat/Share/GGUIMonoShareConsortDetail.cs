using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 妃子分享详情界面
/// </summary>
public class GGUIMonoShareConsortDetail:_AALBasicUIWndMono
{
    [ALHeader("关闭按钮")] 
    public GameObject btnClose;

    [ALHeader("妃子名")]
    public TextEx txtConsortName;
    
    [ALHeader("妃子简介按钮")]
    public GameObject btnConsortProfile;
    
    [ALHeader("品质显示GO")]
    public GGUISubMonoQualityShowGo monoQualityShowGo;
    
    [ALHeader("妃子形象ShowCase")]
    public GGUIMonoCommonShowCase monoConsortShowCase;
    [ALHeader("妃子形象在td showcase中的index")]
    public int consortActorInShowCaseIndex = 0;
    [ALHeader("背景在td showcase中的index")]
    public int bgInShowCaseIndex = 3;


    [ALHeader("羁绊信息展示窗口")]
    public GGUISubMonoConsortFetterInfo monoFetterInfo;
    
    [ALHeader("妃子魅力值")] 
    public TextEx txtConsortCharm; 
    [ALHeader("妃子亲密度")] 
    public TextEx txtConsortIntimacy; 
    
    [ALHeader("UI显隐动画ani")]
    public Animation uiStatAni; 
    [ALHeader("隐藏UI按钮")]
    public GameObject btnHideUI; 
    [ALHeader("显示UI按钮")] 
    public GameObject btnShowUI;
    [ALHeader("隐藏UI动画")]
    public string hideUIAniName; 
    [ALHeader("显示UI动画")] 
    public string showUIAniName;

    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1322); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1322);} }
}