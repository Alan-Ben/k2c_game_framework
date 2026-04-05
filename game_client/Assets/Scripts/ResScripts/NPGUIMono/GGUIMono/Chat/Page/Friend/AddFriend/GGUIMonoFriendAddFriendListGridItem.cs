using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 添加界面好友列表item
/// </summary>
public class GGUIMonoFriendAddFriendListGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("添加按钮")]
    public GameObject btnAdd;
    [ALHeader("已添加显示列表")]
    public List<GameObject> addedShowList;
    [ALHeader("已添加置灰列表")]
    public List<MaskableGraphic> addedGrayList;
    [ALHeader("在线显示，离线隐藏")]
    public List<GameObject> onlineShow;
    [ALHeader("离线显示，在线隐藏")]
    public List<GameObject> onlineHide;
    [ALHeader("离线时长文本")]
    public TextEx offLineTxt;
}
