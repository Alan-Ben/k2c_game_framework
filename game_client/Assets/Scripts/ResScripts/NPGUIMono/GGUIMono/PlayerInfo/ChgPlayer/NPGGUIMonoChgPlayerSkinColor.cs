using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using NPEnum;

/// <summary>
/// 修改玩家肤色界面
/// </summary>
///
public class NPGGUIMonoChgPlayerSkinColor : _AALBasicUIWndMono
{
    [ALHeader("玩家形象")]
    public GGUIMonoCommonShowCase playerShowcase;

    [ALHeader("肤色列表")]
    public NPGGUIMonoChgPlayerSkinColorContainer skinColorContainerMono;

    [ALHeader("消耗列表")]
    public NPGGUIMonoCommonItemContainer costItemMono;

    [ALHeader("保存按钮")]
    public GameObject saveBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1750); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1750); } }
}
