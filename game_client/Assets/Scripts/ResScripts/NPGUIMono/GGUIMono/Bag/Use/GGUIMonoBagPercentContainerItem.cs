using ALPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 概率物品
public class GGUIMonoBagPercentContainerItem : _AALBasicUIWndMono
{

    [ALHeader("物品")]
    public NPGGUIMonoCommonItem itemWnd;
    
    [ALHeader("百分比")]
    public Text percentTxt;
}
