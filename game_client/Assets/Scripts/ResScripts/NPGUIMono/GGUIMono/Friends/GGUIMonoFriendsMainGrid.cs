using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 好友界面容器
/// </summary>

public class GGUIMonoFriendsMainGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoFriendsMainGridItem>
{
    [ALHeader("好友数量/好友上线")]
    public Text friendNumTxt;

    // 无物品提示
    public GameObject noneItemsTips;
}
