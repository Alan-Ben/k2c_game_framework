using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;
using GOE;



/// <summary>
/// 玩家等级预览item
/// </summary>
public class GGUIMonoPlayerLvPreviewGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("条目列表")]
    public NPGGUIMonoCommonTextItemGrid  textItemGridMono;

    [ALHeader("普通条目显示的文本颜色")]
    public Color normalColor;

    [ALHeader("新增条目显示的文本颜色")]
    public Color addColor;

    [ALHeader("特殊条目显示的文本颜色")]
    public Color specialColor;
}
