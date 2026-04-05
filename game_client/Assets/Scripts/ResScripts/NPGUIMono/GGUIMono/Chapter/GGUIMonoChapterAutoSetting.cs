using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using CommonEnum;

/// <summary>
/// 关卡boss战
/// </summary>
public class GGUIMonoChapterAutoSetting : _AALBasicUIWndMono
{
    [ALHeader("金币鼓舞限制item")]
    public NPGGUIMonoCommonItemNumSlider goldInfo;
    [ALHeader("砖石鼓舞限制item")]
    public NPGGUIMonoCommonItemNumSlider crystalInfo;
    [ALHeader("道具鼓舞限制item")]
    public NPGGUIMonoCommonItemNumSlider itemInfo;

    [ALHeader("确认按钮")]
    public GameObject btnSure;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("屏幕关闭按钮")]
    public GameObject screenBtnClose;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2103); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2103); } }
}