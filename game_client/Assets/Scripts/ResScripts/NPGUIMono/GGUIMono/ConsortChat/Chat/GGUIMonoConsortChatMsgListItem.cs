using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class  GGUIMonoConsortChatMsgListItem : _AALBasicUIWndMono
{
    [ALHeader("妃子头像")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("高度自适应的文本最大宽度，通过这个宽度来计算高度")]
    public float maxTextWidth = 500f; // 文本最大宽度
    [ALHeader("对话内容")]
    public Text txtText;
    [ALHeader("图片窗口")]
    public GGUIMonoConsortChatImageGroupPreview imageGroupPreview;
    [ALHeader("亲密度奖励数值")]
    public Text txtIntimacy;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer rewardContainer;
    [ALHeader("输入中文本")]
    public Text txtTyping;
    [ALHeader("输入中动画延迟时间,单位秒")]
    public float typingLoopDelay = 0.2f;
    [ALHeader("输入中需要显示的go")]
    public List<GameObject> typingShowGos;
    [ALHeader("输入中需要隐藏的go")]
    public List<GameObject> typingHideGos;
    [ALHeader("时间文本")]
    public Text txtTime;
    [ALHeader("聊天图片预览ui")]
    public long chatImageGroupUIPathId = 6214;
}
