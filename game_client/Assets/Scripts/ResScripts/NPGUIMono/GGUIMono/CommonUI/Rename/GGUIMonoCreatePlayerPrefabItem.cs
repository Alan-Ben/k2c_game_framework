using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using GOE;
using System;

/// <summary>
/// 创角预设界面
/// /// </summary>
public  class GGUIMonoCreatePlayerPrefabItem :  _AALBasicUIWndMono
{
    [ALHeader("头像")]
    public RawImage monoIcon;
    [ALHeader("半身像")]
    public RawImage imgCard;
    [ALHeader("选择按钮")]
    public GameObject btnSelect;
    [ALHeader("选中时 显示的物体")]
    public List<GameObject> goListShowOnSelect;
    [ALHeader("选中时 隐藏的物体")]
    public List<GameObject> goListHideOnSelect;
}
