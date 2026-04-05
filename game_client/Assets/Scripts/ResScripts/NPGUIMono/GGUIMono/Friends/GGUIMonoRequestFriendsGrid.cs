using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 好友申请列表容器
/// </summary>

public class GGUIMonoRequestFriendsGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoRequestFriendsGridItem>

{
    // 无物品提示
    public GameObject noneItemsTips;

    [ALHeader("申请数量/申请上限")]
    public Text requestNumTxt;

    [ALHeader("一键同意")]
    public GameObject allAgreeBtn;

    [ALHeader("一键拒绝")]
    public GameObject allRefuseBtn;

    [ALHeader("申请数量达到多少时显示一键按钮")]
    public int showAllBtnMinNum;

    [ALHeader("一键按钮相关显示go list")]
    public List<GameObject> allBtnShowGoList;

}
