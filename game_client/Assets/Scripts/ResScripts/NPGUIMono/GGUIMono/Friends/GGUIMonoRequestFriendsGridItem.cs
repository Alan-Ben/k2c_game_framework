using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 好友申请列表Item
/// </summary>
/// 
public class GGUIMonoRequestFriendsGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("头像相关")]
    public NPGGUIMonoPlayerIcon playerIconMono;

    [ALHeader("同意按钮")]
    public GameObject agreeBtn;

    [ALHeader("拒绝按钮")]
    public GameObject refuseBtn;
    [ALHeader("在线显示，离线隐藏")]
    public List<GameObject> onlineShow;
    [ALHeader("离线显示，在线隐藏")]
    public List<GameObject> onlineHide;
    [ALHeader("离线时长文本")]
    public TextEx offLineTxt;
    [ALHeader("成功添加好友的音效id")]
    public long addSuccAudioId = 12101;
}
