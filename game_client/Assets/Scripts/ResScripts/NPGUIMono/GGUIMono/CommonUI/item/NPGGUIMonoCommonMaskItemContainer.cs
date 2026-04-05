using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

/// <summary>
/// 通用物品容器container(item带遮罩)
/// </summary>
public class NPGGUIMonoCommonMaskItemContainer :  _ATNPGGUIMonoShowAnimContainer<NPGGUIMonoCommonMaskItem>
{
    [ALHeader("当未超出容器时列表是否居中")]
    public bool needSetCenterWhenNotExceed;
}

