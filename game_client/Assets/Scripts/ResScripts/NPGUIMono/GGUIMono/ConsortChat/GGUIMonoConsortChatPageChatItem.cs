using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoConsortChatPageChatItem : _TALUGUIMonoGridItem
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;
    [ALHeader("妃子头像")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("聊天消息")]
    public Text chatContent;
    [ALHeader("未读需要显示的go")]
    public List<GameObject> unreadShowGos = new List<GameObject>();
}
