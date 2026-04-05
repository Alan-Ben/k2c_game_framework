using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoCommonRoundSlider : _AALBasicUIWndMono
{
    [ALInfo("半圆进度条,图片和背景需要设置\n" +
        "Pivot为0.5  0  \n" +
        "ImageType为Filled  " + "Fill Method为Radial 180  " + "Fill Origin为Bottom")]
    [ALHeader("")]
    [ALHeader("进度条半圆图片")] 
    public Image imageProcess;
    [ALHeader("进度条半圆图片背景")]
    public Image imageProcessBg;

    [ALInfo("对应半圆的fill Amount")]
    [ALHeader("进度条最小值")] 
    [Range(0f,1f)]
    public float minFillAmount = 0f;
    [Range(0f, 1f)]
    [ALHeader("进度条最大值")] 
    public float maxFillAmount = 1f;

    [ALHeader("进度条第一个位置需要增加的偏移值")]
    public float firstMargin = 0f;
    [ALHeader("进度条最后一个位置需要增加的偏移值")]
    public float lastMargin = 0f;
}