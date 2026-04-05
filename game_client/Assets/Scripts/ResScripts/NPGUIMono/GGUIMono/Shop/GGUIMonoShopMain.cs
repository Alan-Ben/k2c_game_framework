using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;


/// <summary>
/// 商店主界面
/// </summary>
///
public class GGUIMonoShopMain : _AALBasicUIWndMono
{

    [ALHeader("页签")]
    public List<GGUIMonoShopTab> monoTabList;

    [ALHeader("page 父节点")]
    public Transform pageParentPos;

    [ALHeader("默认显示的商店主id")]
    public long defaultShopMainRefId;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3100); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3100); } }
}
