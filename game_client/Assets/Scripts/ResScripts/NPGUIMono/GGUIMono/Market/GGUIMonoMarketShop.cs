using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 集市商店界面
/// </summary>
///
public class GGUIMonoMarketShop : _AALBasicUIWndMono
{
    [ALHeader("图片")]
    public RawImage iconImg;

    [ALHeader("名称")]
    public Text nameTxt;

    [ALHeader("等级")]
    public Text lvTxt;

    [ALHeader("未解锁需要展示的go列表 解锁隐藏")]
    public List<GameObject> lockShowGoList;

    [ALHeader("未解锁需要隐藏的go列表 解锁显示")]
    public List<GameObject> lockHideGoList;

    [ALHeader("解锁文本")]
    public Text unlockTxt;

    [ALHeader("当前进度次数")]
    public Text curCountTxt;

    [ALHeader("经营成功特效Id")]
    public long sfxId;
    [ALHeader("经营成功特效播放位置")]
    public Transform sfxParentPos;

    [ALHeader("经营成功暴击倍数特效播放位置")]
    public Transform sfxMulParentPos;

    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer rewardContainerMono;

    [ALHeader("消耗物品")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("有次数显示的goList 没有次数或者在cd中隐藏")]
    public List<GameObject> hasCountShowGoList;

    [ALHeader("没有次数或者在cd中显示的goList 有次数隐藏")]
    public List<GameObject> noCountShowGoList;

    [ALHeader("消耗次数领取奖励按钮")]
    public GameObject getRewardBtn;

    [ALHeader("消耗道具领取奖励按钮")]
    public GameObject getRewardCostItemBtn;

    [ALHeader("没有上一个店铺需要隐藏的GoList")]
    public List<GameObject> noLastHideGoList;

    [ALHeader("没有下一个店铺需要隐藏的GoList")]
    public List<GameObject> noNextHideGoList;

    [ALHeader("上一个按钮")]
    public GameObject lastShopBtn;

    [ALHeader("下一个按钮")]
    public GameObject nextShopBtn;

    [ALHeader("关闭按钮")]
    public GameObject closeBtn;


    public static string assetPath { get { return UIResPathAssistant.getAssetPath(3902); } }
    public static string objName { get { return UIResPathAssistant.getObjName(3902); } }
}
