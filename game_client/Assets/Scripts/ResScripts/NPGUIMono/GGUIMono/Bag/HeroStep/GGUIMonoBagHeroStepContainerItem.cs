using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 背包骑士阶段item
public class GGUIMonoBagHeroStepContainerItem : _AALBasicUIWndMono
{
    [ALHeader("阶段图片")]
    public RawImage stepImg;

    [ALHeader("阶段名称")]
    public Text stepNameTxt;

    [ALHeader("合成列表")]
    public GGUIMonoBagHeroStepSubContainer containerMono;

    [ALHeader("最后一个需要隐藏的GoList 其他下标显示")]
    public List<GameObject> lastHideGoList;
}
