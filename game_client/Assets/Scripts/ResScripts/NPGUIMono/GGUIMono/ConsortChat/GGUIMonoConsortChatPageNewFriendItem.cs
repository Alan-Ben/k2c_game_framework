using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoConsortChatPageNewFriendItem : _TALUGUIMonoGridItem
{
    [ALHeader("添加按钮")]
    public GameObject btnAdd;
    [ALHeader("妃子头像")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("已添加需要显示的go")]
    public List<GameObject> addedShowGos;
    [ALHeader("等待添加需要显示的go")]
    public List<GameObject> waitAddShowGos;
}
