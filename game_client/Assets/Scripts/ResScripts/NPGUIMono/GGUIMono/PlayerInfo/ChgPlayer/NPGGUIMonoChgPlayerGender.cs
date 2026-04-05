using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;
using NPEnum;

/// <summary>
/// 修改玩家性别界面
/// </summary>
///
public class NPGGUIMonoChgPlayerGender : _AALBasicUIWndMono
{
    [ALHeader("玩家形象")]
    public GGUIMonoCommonShowCase playerShowcase;

    [ALHeader("切换性别Tab - 选中状态为男性")]
    public NPGGUIMonoCommonTab chgGenderTab;

    [ALHeader("消耗")]
    public NPGGUIMonoCommonItemContainer costItemMono;

    [ALHeader("保存按钮")]
    public GameObject saveBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1751); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1751); } }
}
