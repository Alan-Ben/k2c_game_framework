using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 页签类型
/// </summary>
public enum EConsortChatType
{
    CHAT,//聊天
    AI,//AI聊天
}

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatPageChat : _AALBasicUIWndMono
{
    [ALHeader("添加好友列表")]
    public GGUIMonoConsortChatPageNewFriendItemGrid addFriendItemGrid;
    [ALHeader("聊天列表")]
    public GGUIMonoConsortChatPageChatItemGrid chatItemGrid;
    [ALHeader("聊天按钮")]
    public NPGGUIMonoCommonTab tabChat;
    [ALHeader("AI聊天按钮")]
    public NPGGUIMonoCommonTab tabAI;

    [ALHeader("没有可聊天对象时显示空状态")] 
    public List<GameObject> noChatItemShow;
}