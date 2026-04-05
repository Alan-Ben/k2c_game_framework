using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 大学座位跟随item
/// </summary>
public class GGUIMonoCollegeFollowItem: ALGGUIMonoCommonFollowItem
{
    [ALHeader("不同状态显示的GoList")]
    public List<NPCommonEnumStatInfo<ECollegePosStatus>> statusList;

    [ALHeader("骑士半身像")]
    public RawImage heroCardImg;

    [ALHeader("点击按钮")]
    public GameObject clickBtn;

    [ALHeader("进修倒计时文本")]
    public NPGGUIMonoCommonCountDown countDownMono;

    [ALHeader("骑士入座播放的特效Id")]
    public long sfxId;

    [ALHeader("骑士学习完成播放的特效Id")]
    public long finishSfxId;

    [ALHeader("骑士播放特效的位置")]
    public Transform sfxParentPos;

    [ALHeader("骑士播放的动画")]
    public Animation ani;

    [ALHeader("骑士入座播放的动画名称")]
    public string aniStr;

    [ALHeader("骑士学习完成的动画要填 否则无法播放特效")]
    [ALHeader("骑士学习完成播放的动画名称")]
    public string finishAniStr;

    [ALHeader("需要缩放的GoList")]
    public List<GameObject> scaleGoList;

    [ALHeader("解锁位置的音效id")]
    public long addPosAudioId;

}