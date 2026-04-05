using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 集市店铺跟随item
/// </summary>
public class GGUIMonoMarketFollowItem : ALGGUIMonoCommonFollowItem
{
    [ALHeader("店铺名字")]
    public TextEx textName;

    [ALHeader("未解锁需要展示的go列表 解锁隐藏")]
    public List<GameObject> lockShowGoList;

    [ALHeader("未解锁需要隐藏的go列表 解锁显示")]
    public List<GameObject> lockHideGoList;

    [ALHeader("第一个未解锁需要展示的go列表")]
    public List<GameObject> firstLockShowGoList;

    [ALHeader("解锁后可领取奖励时显示的go列表")]
    public List<GameObject> unlockCanGetShowGoList;

    [ALHeader("解锁后不可领取奖励时显示的go列表")]
    public List<GameObject> unlockCanNotGetShowGoList;

    [ALHeader("不可领取奖励时 在冷却 cd 中需要展示的go列表")]
    public List<GameObject> inCDShowGoList;

    [ALHeader("不可领取奖励时 使用次数为0时需要展示的go列表")]
    public List<GameObject> noCountShowGoList;

    [ALHeader("解锁文本")]
    public Text unlockTxt;

    [ALHeader("当前进度次数")]
    public Text curCountTxt;

    [ALHeader("冷却倒计时时间")]
    public NPGGUIMonoCommonCountDown countDownMono;

    [ALHeader("经营成功特效Id")]
    public long sfxId;
    [ALHeader("经营成功特效播放位置")]
    public Transform sfxParentPos;

    [ALHeader("经营成功特效播放位置")]
    public Transform sfxMulParentPos;

    [ALHeader("未解锁时店铺名称颜色")]
    public Color lockNameColor = Color.gray;
    [ALHeader("已解锁时店铺名称颜色")]
    public Color unLockNameColor = Color.yellow;

}