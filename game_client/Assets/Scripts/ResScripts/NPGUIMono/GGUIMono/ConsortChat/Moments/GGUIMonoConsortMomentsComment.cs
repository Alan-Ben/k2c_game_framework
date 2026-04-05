using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoConsortMomentsComment : _AALBasicUIWndMono
{
    [ALHeader("妃子头像")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("发送者")]
    public Text txtSender;
    [ALHeader("评论内容")]
    public Text txtContent;
    [ALHeader("发送者为玩家需要显示的go")]
    public List<GameObject> playerSendShow;
    [ALHeader("发送者为妃子需要显示的go")]
    public List<GameObject> consortSendShow;
}

