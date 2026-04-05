using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class GGUIMonoConsortChatPageMoments : _AALBasicUIWndMono
{
    [ALHeader("点击评论显示列表")]
    public List<GameObject> openCommentShowList;
    [ALHeader("发送评论按钮")]
    public GameObject btnSendComment;
    [ALHeader("评论输入框")]
    public InputFieldEmoji inputComment;
    [ALHeader("关闭发送评论按钮")]
    public GameObject btnCloseComment;
    [ALHeader("评论列表容器")]
    public GGUIMonoConsortMomentsList momentsList;
    [ALHeader("妃子恢复评论详情按钮")]
    public GameObject btnConsortReplyUnread;
    [ALHeader("妃子互动数量")]
    public Text txtConsortInteractionCount;
    [ALHeader("妃子回复评论时需要显示的GO列表")]
    public List<GameObject> unreadConsortReplyShowGos;
    [ALHeader("有新消息时朋友圈列表顶部需要偏移的高度")]
    public float unreadTopPaddingValue;
}