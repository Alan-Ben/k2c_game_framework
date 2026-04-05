using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;


//搜索结果枚举
public enum ENPFriendsSearchResultTabType
{
    NONE,//未开始
    HASCID,//搜索到玩家
    NOCID,//未搜索到玩家
}

[System.Serializable]
public class GGUIFriendsSearchResultTabMono
{
    public ENPFriendsSearchResultTabType tabType;
    public List<GameObject> goList;
}

/// <summary>
/// 搜索好友界面
/// </summary>
public class GGUIMonoSearchFriend : _AALBasicUIWndMono
{
    [ALHeader("输入框")]
    public InputField cidInputField;

    [ALHeader("搜索按钮")]
    public GameObject searchBtn;

    [ALHeader("搜索结果显示")]
    public List<GGUIFriendsSearchResultTabMono> resultShowGoList;

    [ALHeader("举报按钮")]
    public GameObject reportBtn;

    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerIconMono;

    [ALHeader("申请好友按钮")]
    public GameObject requestBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(2602); } }
    public static string objName { get { return UIResPathAssistant.getObjName(2602); } }

}
