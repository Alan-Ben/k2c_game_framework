using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 分享icon的item
/// </summary>
public class GGUIMonoShareIconItem : _TALUGUIMonoGridItem
{
    [ALHeader("头像")]
    public RawImage texIcon;
    [ALHeader("头像背景")]
    public Image imgIconBg;
    [ALHeader("选中按钮")]
    public GameObject btnSelected;
    [ALHeader("选中显示")]
    public List<GameObject> selectedList;
    [ALHeader("特殊展示GO列表")]
    public List<GameObject> goSpecialList;
}
