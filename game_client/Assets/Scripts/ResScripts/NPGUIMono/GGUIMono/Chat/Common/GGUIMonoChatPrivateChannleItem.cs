using System.Collections.Generic;
using ALPackage;
using UnityEngine;


/// <summary>
/// 聊天会话item
/// </summary>
public class GGUIMonoChatPrivateChannleItem : _TALUGUIMonoGridItem
{
    [ALHeader("聊天频道item")]
    public GGUIMonoChatChannleSimpleItem channleItem;
    [ALHeader("置顶标志")]
    public List<GameObject> flagUpToTop;
}
