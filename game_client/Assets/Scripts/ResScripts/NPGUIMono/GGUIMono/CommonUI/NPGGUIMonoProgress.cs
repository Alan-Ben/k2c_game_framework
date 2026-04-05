
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;



/// <summary>
/// 通用进度条
/// </summary>
public class NPGGUIMonoProgress : _AALBasicUIWndMono
{
    [ALHeader("进度条")]
    public Slider progressSlider;
    [ALHeader("进度文本  {0}/{1}")]
    public Text progressTxt;
    [ALHeader("进度条满时显示的GO列表")]
    public List<GameObject> goFullShowList;
    [ALHeader("进度条满时隐藏的GO列表")]
    public List<GameObject> goFullHideList;
    [ALHeader("是否使用特殊的文本颜色")]
    public bool useTextColor;
    public Color colorComplete;
    public Color colorUnComplete;
}
