using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;



/// <summary>
/// 聊天频道item
/// </summary>
public class GGUIMonoChatChannleItem : _AALBasicUIWndMono
{

    [ALHeader("玩家名称/聊天频道名称")]
    public Text playerNameTxt;

    [ALHeader("玩家头像/聊天频道图标")]
    public RawImage playerIconImg;

    [ALHeader("最后一条消息文本")]
    public Text contentTxt;

    [ALHeader("消息时间戳")]
    public Text msgTimeS;

    [ALHeader("点击按钮")]
    public GameObject clickGo;
    
    [ALHeader("红点go")]
    public GameObject redTipGo;
    
    [ALHeader("选中显示的go列表")]
    public List<GameObject> selectedShowList;
    [ALHeader("没有历史消息的时候隐藏的go列表，有消息显示")]
    public List<GameObject> noMsgHideList;
}
