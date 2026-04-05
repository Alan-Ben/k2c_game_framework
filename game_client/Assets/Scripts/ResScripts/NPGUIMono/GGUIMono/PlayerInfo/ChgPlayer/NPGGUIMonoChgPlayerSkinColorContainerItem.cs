using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

/// <summary>
/// 烹饪等级总览 容器item
/// </summary>
public class NPGGUIMonoChgPlayerSkinColorContainerItem : _AALBasicUIWndMono
{
    [ALHeader("皮肤颜色图片")]
    public Image iconImg;

    [ALHeader("选中显示GoList")]
    public List<GameObject> selectShowGoList;

    [ALHeader("点击按钮")]
    public GameObject clickGo;

}
