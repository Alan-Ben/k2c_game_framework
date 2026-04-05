using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 杰出者庆祝
/// </summary>
public class GGUIMonoGraveCongrats : _AALBasicUIWndMono
{

    // <AutoGen:MonoDeclaration>
    [ALHeader("玩家形象")]
    public GGUIMonoCommonShowCase playerInfo;
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("钻石奖励")]
    public NPGGUIMonoCommonItem gemCostItem;
    [ALHeader("奖励打字机文本")]
    public GGUIMonoTextTypewriter txtTypewriter;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerIcon;
    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6605); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6605);} }
}