using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 规则列表弹窗
/// </summary>
///
public class NPGGUIMonoRuleList : _AALBasicUIWndMono
{
    [ALHeader("关闭按钮")]
    public GameObject closeBtn;
    [ALHeader("item列表")]
    public NPGGUIMonoRuleListItemGrid itemGrid;


    public static string assetPath { get { return UIResPathAssistant.getAssetPath(300); } }
    public static string objName { get { return UIResPathAssistant.getObjName(300); } }
}
