using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;

/// <summary>
/// 商店主界面
/// </summary>
///
public class GGUIMonoShopMain_NoTab : _AALBasicUIWndMono
{

    [ALHeader("page 父节点")]
    public Transform pageParentPos;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3101); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3101); } }
}
