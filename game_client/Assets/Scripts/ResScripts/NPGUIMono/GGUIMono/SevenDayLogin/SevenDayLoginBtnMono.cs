
using System.Collections.Generic;
using GOE;
using UnityEngine;

public class SevenDayLoginBtnMono : MonoBehaviour
{
    [ALHeader("这个按钮是第几天的登录奖励按钮")]
    public int day;
    [ALHeader("领取按钮")]
    public GameObject btn;
    [ALHeader("提示UI")]
    public long tipsUIPathId = UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT;
    [ALHeader("显示物品提示")]
    public GameObject btnShowTips;
    [ALHeader("物品详情偏移值X")]
    public float detailIntervalX;
    [ALHeader("物品详情偏移值Y")]
    public float detailIntervalY;
    [ALHeader("展示奖励物品,同一个item,用于做多状态显示")]
    public List<NPGGUIMonoCommonItem> rewardItems; 
    [ALHeader("不同状态显示配置")]
    public List<NPCommonEnumStatInfo<ESevenDayLoginBtnState>> statInfos;
}
