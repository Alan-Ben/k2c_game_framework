using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 联盟宝箱
/// </summary>
public class GGUIMonoGuildBoxGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("倒计时")]
    public Text txtTime;
    [ALHeader("倒计时剩余多少时改变颜色(秒)")]
    public long redColorTextTimeS = 5 * 60 * 60 ;
    public Color txtTimeColorCommon = Color.black;
    public Color txtTimeColorRed = Color.red;
    [ALHeader("活跃积分")]
    public Text txtScore;
    [ALHeader("宝箱名字")]
    public Text txtName;
    [ALHeader("宝箱详情")]
    public Text txtDetail;
    [ALHeader("领取按钮")]
    public GameObject btnGet;
    // </AutoGen:MonoDeclaration>
    [ALHeader("宝箱详情按钮")]
    public GameObject btnBoxDetail;
    [ALHeader("宝箱图标")]
    public RawImage imgBoxIcon;
    [ALHeader("物品")]
    public NPGGUIMonoCommonItem itemWnd;
    [ALHeader("已领取显示的GO列表")]
    public List<GameObject> alreadyGetShowList;
    [ALHeader("已领取隐藏的GO列表")]
    public List<GameObject> alreadyGetHideList;

    [ALInfo("====领奖表现相关配置====")]
    [ALHeader("道具粒子id")]
    public long itemParticleId;
    [ALHeader("活跃点粒子id")]
    public long activePointParticleId;
    [ALHeader("道具粒子开始位置")]
    public RectTransform itemParticleStartRectTransform;
    [ALHeader("活跃点粒子开始位置")]
    public RectTransform activePointParticleStartRectTransform;
    [ALHeader("延时显示列表滚动时间")]
    public float delayShowListScroll = 0.5f;
}

