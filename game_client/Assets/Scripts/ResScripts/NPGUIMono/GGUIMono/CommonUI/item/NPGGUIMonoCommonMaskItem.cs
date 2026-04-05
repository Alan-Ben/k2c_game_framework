using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 物品控件组合，包含物品显示常用需求(item带遮罩可置灰)
/// </summary>
public class NPGGUIMonoCommonMaskItem : NPGGUIMonoCommonItem
{
    [ALHeader("显示的遮罩列表")]
    public List<GameObject> maskList;
    [ALHeader("需要置灰的列表")]
    public List<MaskableGraphic> grayList;
}

