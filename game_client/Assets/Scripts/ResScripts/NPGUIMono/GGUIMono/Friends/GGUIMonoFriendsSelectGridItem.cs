using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 好友选择容器Item
/// </summary>
/// 
public class GGUIMonoFriendsSelectGridItem : _TALUGUIMonoGridItem
{

    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerIconMono;

    [ALHeader("选择按钮")]
    public GameObject selectBtn;

}
