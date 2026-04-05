using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// item
/// </summary>
public class GGUIMonoTowerResearchItem : _AALBasicUIWndMono
{
    [ALHeader("有加成关卡列表")]
    public GGUIMonoTowerResearchLevelItemContainer levelContainer;

    [ALHeader("是否显示关卡列表")]
    public NPGGUIMonoCommonTab toggleShowLevel;
    [ALHeader("领奖按钮")]
    public GameObject btnGetReward;
    [ALHeader("奖励提示弹窗位置")]
    public RectTransform toolTipsRoot;
    [ALHeader("背景图片")]
    public RawImage texBg;

    [ALHeader("第一个Item Slider计算的高度")]
    public float firstItemHeight = 90;
    [ALHeader("后续ItemSlider计算的高度")]
    public float commonItemHeight = 180;
    [ALHeader("进度滑动条")]
    public Slider progressSlider;
    [ALHeader("进度文本")]
    public Text txtProgress;
    [ALHeader("名称")]
    public Text txtName;
    [ALHeader("奖励容器")]
    public GGUIMonoCommonRewardContainer rewardContainer;
    
    [ALHeader("不同奖励状态显示的Go List")]
    public List<NPCommonEnumStatInfo<ECommonRewardType>> rewardStatList;
    [ALHeader("全部激活需要显示GO列表")]
    public List<GameObject> activateAllShowGos;
    [ALHeader("可激活需要显示GO列表")]
    public List<GameObject> canActivateShowGos;
}

