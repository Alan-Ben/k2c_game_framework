using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 杰出者大厅主界面
/// </summary>
public class GGUIMonoGraveMain : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("庆祝按钮")]
    public GameObject btnCelebrate;
    [ALHeader("新晋者按钮")]
    public GameObject btnNewCommer;
    [ALHeader("祝福效果按钮")]
    public GameObject btnBless;
    [ALHeader("可以庆祝需要显示的Gos")]
    public List<GameObject> canCelebrateShowGos;
    [ALHeader("可以庆祝需要隐藏的Gos")]
    public List<GameObject> canCelebrateHideGos;
    [ALHeader("有新晋者需要显示")]
    public List<GameObject> hasNewCommerShowGos;
    [ALHeader("无新晋者需要显示")]
    public List<GameObject> hasNewCommerHideGos;
    // </AutoGen:MonoDeclaration>
    [ALHeader("杰出者列表")]
    public List<GGUIMonoGraveMainItem> graveShowItems = new List<GGUIMonoGraveMainItem>();
    
    [ALHeader("庆祝按钮")]
    public GameObject btnNextGraveMain;
    [ALHeader("庆祝按钮")]
    public GameObject btnPreGraveMain;
    [ALHeader("存在下一个大厅需要显示")]
    public List<GameObject> hasNextGraveMainShowGos;
    [ALHeader("存在上一个大厅需要显示")]
    public List<GameObject> hasPreGraveMaineGos;
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6600); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6600);} }
}