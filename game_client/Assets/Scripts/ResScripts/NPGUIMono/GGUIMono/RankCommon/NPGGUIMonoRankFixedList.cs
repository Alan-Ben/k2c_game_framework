using GOE;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常驻排行榜列表
/// </summary>
///
public class NPGGUIMonoRankFixedList : _ANPBasicUIWndResBarMono
{
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("常驻排行榜本服列表")]
    public List<NPGGUIMonoRankFixedItem> localFixedItemList;

    [ALHeader("可以一键点赞显示的GoList")]
    public List<GameObject> canAKeyLikeShowGoList;

    [ALHeader("不可以一键点赞显示的GoList")]
    public List<GameObject> noAKeyLikeShowGoList;

    [ALHeader("一键点赞按钮")]
    public GameObject aKeyLikeBtn;

    public static string assetPath { get { return UIResPathAssistant.getAssetPath(4300); } }
    public static string objName { get { return UIResPathAssistant.getObjName(4300); } }
}
