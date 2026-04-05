using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 杰出者详情
/// </summary>
public class GGUIMonoGravePlayerDetail : _AALBasicUIWndMono
{
    [ALHeader("称号")]
    public GGUIMonoSubPlayerTitle monoSubPlayerTitle;
    [ALHeader("关闭")]
    public GameObject btnClose;
    // <AutoGen:MonoDeclaration>
    [ALHeader("荣誉记录")]
    public GameObject btnHonorLog;
    [ALHeader("拜访按钮")]
    public GameObject btnVisit;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("玩家形象显示")]
    public GGUIMonoCommonShowCase playerShowcase;
    [ALHeader("有玩家需要显示的Gos")]
    public List<GameObject> hasPlayerShowGos;
    [ALHeader("无玩家需要显示的Gos")]
    public List<GameObject> noPlayerShowGos;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6601); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6601);} }
}