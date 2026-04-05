using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用奖励预览界面
/// </summary>
public class NPGGUIMonoCommonRewardPreviewTowList : NPGGUIMonoCommonRewardPreview
{
    [ALHeader("第二个列表的描述")]
    public TextEx txtSecondDesc;
    [ALHeader("第二个奖励列表")]
    public NPGGUIMonoCommonItemContainer itemSecondContainer;
    [ALHeader("有第二个列表的时候显示，没有隐藏的列表")]
    public List<GameObject> towListShowHide;
    
}