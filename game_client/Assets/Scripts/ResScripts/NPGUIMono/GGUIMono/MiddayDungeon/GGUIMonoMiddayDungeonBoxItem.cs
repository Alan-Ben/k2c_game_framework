using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public enum EMiddayDungeonBoxState
{
    CanOpen = 0,
    HasOpen = 1,
    Invalid = 2,
    [InspectorName("已领完")]
    NonCanOpen = 3,
}
/// <summary>
/// item
/// </summary>
public class GGUIMonoMiddayDungeonBoxItem : _AALBasicUIWndMono
{
    [ALHeader("打开宝箱按钮")]
    public GameObject btnOpenBox;
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("宝箱图标")]
    public RawImage boxIcon;
    [ALHeader("宝箱可领取次数")]
    public Text txtCount;

    [ALHeader("宝箱不同状态显示配置")]
    public List<NPCommonEnumStatInfo<EMiddayDungeonBoxState>> statInfos;
}

