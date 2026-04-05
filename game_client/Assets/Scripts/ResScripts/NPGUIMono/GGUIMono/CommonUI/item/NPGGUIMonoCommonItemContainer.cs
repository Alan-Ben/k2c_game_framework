using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

/// <summary>
/// 通用物品容器container
/// </summary>
public class NPGGUIMonoCommonItemContainer : _ATNPGGUIMonoShowAnimContainer<NPGGUIMonoCommonItem>
{
    [ALHeader("当未超出容器时列表是否居中")]
    public bool needSetCenterWhenNotExceed;
}

