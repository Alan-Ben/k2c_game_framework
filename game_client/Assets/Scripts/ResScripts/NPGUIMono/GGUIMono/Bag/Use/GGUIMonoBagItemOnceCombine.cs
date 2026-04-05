using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 一键合成弹窗
/// </summary>
public class GGUIMonoBagItemOnceCombine : _AALBasicUIWndMono
{
    [ALHeader("目标物品列表")]
    public NPGGUIMonoCommonItemContainer monoTargetItemContainer;

    [ALHeader("合成原料物品列表")]
    public NPGGUIMonoCommonItemContainer monoOriItemContainer;

    [ALHeader("合成资源物品列表")]
    public NPGGUIMonoCommonItemContainer monoResItemContainer;

    [ALHeader("合成按钮")]
    public GameObject btnCombine;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1914); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1914); } }
}
