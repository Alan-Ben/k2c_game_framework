using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoConsortMomentsListItem : _AALBasicUIWndMono
{
    [ALHeader("默认高度")]
    public float defaultHeight = 500;
    [ALHeader("朋友圈妃子信息")]
    public GGUIMonoConsortIconItem consortIconItem;
    [ALHeader("朋友圈内容文本")]
    public Text txtContent;
    
    [ALHeader("朋友圈时间显示")] 
    public Text txtTime;
    
    [ALHeader("点赞按钮")]
    public NPGGUIMonoCommonToggleEx toggleLike;
    
    [ALHeader("评论按钮")]
    public NPGGUIMonoCommonToggleEx toggleComment;
    
    [ALHeader("点赞人员列表文本")] 
    public Text txtLikePlayers;

    [ALHeader("有人点赞需要显示的go，没有则隐藏")]
    public List<GameObject> hasLikeShowGos;
    
    [ALHeader("有人评论需要显示的go，没有则隐藏")]
    public List<GameObject> hasCommentShowGos;
    
    [ALHeader("评论容器")]
    public GGUIMonoConsortMomentsCommentContainer commentContainer;

    [ALHeader("图片容器")]
    public GGUIMonoMomentsImageItemContainer imageContainer;

    [ALHeader("点击点赞按钮音效")]
    public long clickLikeAudioId;

}
