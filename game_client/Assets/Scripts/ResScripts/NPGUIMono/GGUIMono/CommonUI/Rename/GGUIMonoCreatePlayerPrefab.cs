using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

/// <summary>
/// 创角预设界面
/// /// </summary>
public  class GGUIMonoCreatePlayerPrefab :  _AALBasicUIWndMono
{
    [ALHeader("玩家形象")] 
    public GGUIMonoCommonShowCase showCaseMono;
    [ALHeader("头像容器")]
    public GGUIMonoCreatePlayerPrefabContainer iconContainer;
    [ALHeader("确认按钮")]
    public GameObject selectBtn;
    
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3800); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3800);} }
}
