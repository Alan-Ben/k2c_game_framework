using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择登录方式的选项
/// </summary>
public class NPGGUIMonoLoginWayItem : _TALUGUIMonoGridItem
{
    [ALHeader("图标")]
    public RawImage icon;
    [ALHeader("名称")]
    public Text wayName;
    [ALHeader("已登录名称")]
    public Text curWayName;
    [ALHeader("点击对象")]
    public GameObject clickGo;
    [ALHeader("是当前登录方式时需要展示的GO列表")]
    public List<GameObject> goCurLoginShowList;
    [ALHeader("是当前登录方式时需要隐藏的GO列表")]
    public List<GameObject> goCurLoginHideList;
}
