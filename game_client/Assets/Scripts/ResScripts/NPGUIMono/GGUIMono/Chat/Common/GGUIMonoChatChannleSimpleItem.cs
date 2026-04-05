using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;



/// <summary>
/// 聊天频道简单的item
/// </summary>
public class GGUIMonoChatChannleSimpleItem : _AALBasicUIWndMono
{
    [ALHeader("玩家头像/聊天频道图标")]
    public RawImage playerIconImg;
    [ALHeader("名称")]
    public Text txtName;
    [ALHeader("选中的妃子头像颜色")]
    public Color selectedIconColor;
    [ALHeader("未选中的妃子头像颜色")]
    public Color unSelectedIconColor;
    
    [ALHeader("点击按钮")]
    public GameObject clickGo;
    
    [ALHeader("红点go")]
    public GameObject redTipGo;
    
    [ALHeader("选中显示的go列表")]
    public List<GameObject> selectedShowList;
    [ALHeader("选中隐藏的go列表")]
    public List<GameObject> selectedHideList;
}
