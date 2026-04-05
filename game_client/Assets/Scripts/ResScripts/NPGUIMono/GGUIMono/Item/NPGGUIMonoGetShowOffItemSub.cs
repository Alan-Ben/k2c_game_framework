using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


//炫耀性物品子窗口
public class NPGGUIMonoGetShowOffItemSub : _AALBasicUIWndMono
{
    [ALHeader("基础信息")]
    public NPGGUIMonoCommonItem commonItemMono;

    [ALHeader("穿戴成功显示GoList")]
    public List<GameObject> putOnSucShowGoList;

    [ALHeader("预制体加载父节点")]
    public Transform parentPos;

}
