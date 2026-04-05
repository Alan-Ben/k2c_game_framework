using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// item
/// </summary>
public class GGUIMonoStepRewardTabItem : _AALBasicUIWndMono
{
    [ALHeader("通用页签脚本")]
    public NPGGUIMonoCommonTab monoTab;
    [ALHeader("名字")]
    public Text txtName;
    [ALHeader("名字")]
    public Text txtName2;
    [ALHeader("是首个页签时显示的GO列表")]
    public List<GameObject> goFirstShowList;
    [ALHeader("是首个页签时隐藏的GO列表")]
    public List<GameObject> goFirstHideList;
}

