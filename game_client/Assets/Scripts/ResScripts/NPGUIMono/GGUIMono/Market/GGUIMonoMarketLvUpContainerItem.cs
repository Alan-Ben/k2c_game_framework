using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//集市升级item
public class GGUIMonoMarketLvUpContainerItem : _AALBasicUIWndMono
{
    [ALHeader("图片")]
    public RawImage iconImg;

    [ALHeader("名称")]
    public Text nameTxt;

    [ALHeader("等级")]
    public Text lvTxt;

    [ALHeader("奖励翻倍当前概率文本")]
    public Text niubiProTxt;

    [ALHeader("奖励升级后翻倍概率文本")]
    public Text niubiProAfterTxt;

    [ALHeader("奖励当前倍数文本")]
    public Text niubiCountTxt;

    [ALHeader("奖励升级后倍数文本")]
    public Text niubiCountAfterTxt;

    [ALHeader("升级成功特效Id")]
    public long sfxId;
    [ALHeader("升级成功特效播放位置")]
    public Transform sfxParentPos;

    [ALHeader("满级显示的GoList")]
    public List<GameObject> lvMaxShowGoList;

    [ALHeader("满级隐藏的GoList")]
    public List<GameObject> lvMaxHideGoList;

    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonItemContainer rewardContainerMono;

    [ALHeader("升级消耗")]
    public NPGGUIMonoCommonItem costItemMono;

    [ALHeader("升级按钮")]
    public GameObject lvUpBtn;

    [ALHeader("满级时奖励倍数文本")]
    public Text txtMaxLevelMultiple;
    [ALHeader("满级时奖励翻倍概率文本")]
    public Text txtMaxLevelProbability;
    [ALHeader("查看详情按钮")]
    public GameObject btnDetail;
    [ALHeader("详情弹窗间隔")]
    public float toolTipInterval = 0f;
}
