using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

// 大学骑士多选弹窗
public class GGUIMonoCollegeHeroChooseMul : _AALBasicUIWndMono
{
    [ALHeader("骑士多选列表")]
    public GGUIMonoCollegeHeroChooseMulGrid heroMulContainerMono;

    [ALHeader("确定按钮")]
    public GameObject confirmBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1805); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1805); } }
}
