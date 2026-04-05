using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGUIMonoPlayerLvPreviewGrid : _TALUGUIMonoGridWnd<GGUIMonoPlayerLvPreviewGridItem>
{

    [ALInfo("这里的ScrollRect要挂ScrollRectEx脚本")]

    [ALHeader("item 的宽度")]
    public int itemW;


    [ALHeader("上一页按钮")]
    public GameObject lastBtn;

    [ALHeader("下一页按钮")]
    public GameObject nextBtn;

}
