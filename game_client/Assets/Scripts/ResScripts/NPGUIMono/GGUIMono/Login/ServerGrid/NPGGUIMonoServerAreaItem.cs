using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine.UI;

/// <summary>
/// 大区选择item
/// </summary>
public class NPGGUIMonoServerAreaItem : _AALBasicUIWndMono
{
    [ALHeader("图标")]
    public Image icon;
    [ALHeader("名称")]
    public Text areaName;
    [ALHeader("点击对象")]
    public GGUIMonoCommonSetColorTab clickGo;
}
