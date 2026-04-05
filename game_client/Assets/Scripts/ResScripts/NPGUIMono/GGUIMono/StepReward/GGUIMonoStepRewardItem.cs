using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public enum EStepRewardState
{
    [InspectorName("未达成，显示GoTo")]
    None = 0,
    [InspectorName("已达成，显示领取")]
    CanGet = 1,
    [InspectorName("已领取")]
    AlreadyGet = 2,
}
/// <summary>
/// item
/// </summary>
public class GGUIMonoStepRewardItem : _TALUGUIMonoGridItem
{
    [ALHeader("领取按钮")]
    public GameObject btnGet;
    [ALHeader("前往按钮")]
    public GameObject btnGoTo;
    [ALHeader("奖励描述")]
    public Text textDesc;
    [ALHeader("成就步骤进度文本(可领取时展示)")]
    public Text txtProcess;
    [ALHeader("成就步骤进度文本(不可领取时展示)")]
    public Text txtCanNotGetProcess;
    [ALHeader("奖励列表")]
    public NPGGUIMonoCommonMaskItemContainer itemContainer;
    [ALHeader("不同状态显示配置")]
    public List<NPCommonEnumStatInfo<EStepRewardState>> statInfos;
    [ALHeader("GoTo显隐Go,再状态显示之后更新")]
    public List<GameObject> showGoToGos;
}
