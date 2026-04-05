using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 宴会席位的跟随窗口
/// </summary>
public class GGUIMonoDinnerSeatFollowItem : ALGGUIMonoCommonFollowItem
{
    [ALHeader("点击按钮")]
    public GameObject btnClick;
    [ALHeader("名字")]
    public TextEx textName;
    [ALHeader("半身像")]
    public RawImage imgCardIcon;
    [ALHeader("玩家详情加载的父节点")]
    public RectTransform playerDetailParent;
    [ALHeader("好友需要显示的go")]
    public List<GameObject> friendShowGos;
}