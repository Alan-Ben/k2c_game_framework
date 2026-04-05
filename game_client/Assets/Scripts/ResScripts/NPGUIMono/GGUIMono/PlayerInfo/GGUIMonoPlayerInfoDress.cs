using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;


//装扮界面页签类型
public enum ENPDressTabType
{
    ICON,//头像
    ICON_BGK,//头像框
    BUBBLE,//气泡框
}

[System.Serializable]
public class GGUIDressTabMono
{
    public ENPDressTabType tabType;//页签类型
    public NPGGUIMonoCommonTab monoTab;//页签脚本
    public long assestPathInfoId;//子窗口路径id
}

/// <summary>
/// 玩家装扮
/// </summary>
///
public class GGUIMonoPlayerInfoDress : _AALBasicUIWndMono
{
    [ALHeader("标题")]
    public Text txtTitle;
    [ALHeader("物品页签")]  
    public List<GGUIDressTabMono> monoTabList;

    [ALHeader("page 父节点")]
    public Transform pageParentPos;

    [ALHeader("默认显示的页签")]
    public ENPDressTabType defaultShow;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(1702); } }
    public static string objName { get { return UIResPathAssistant.getObjName(1702); } }
}
