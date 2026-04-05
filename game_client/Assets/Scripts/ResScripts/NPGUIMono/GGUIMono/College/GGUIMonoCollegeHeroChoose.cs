using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 大学骑士单选弹窗
public class GGUIMonoCollegeHeroChoose : _AALBasicUIWndMono
{
    [ALHeader("进修名额文本")]
    public Text learnNumTxt;

    [ALHeader("骑士列表")]
    public GGUIMonoCollegeHeroChooseGrid heroContainerMono;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1803); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1803); } }
}
