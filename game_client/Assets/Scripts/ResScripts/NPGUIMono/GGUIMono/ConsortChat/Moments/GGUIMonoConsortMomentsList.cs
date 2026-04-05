
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

/// <summary>
/// item容器
/// </summary>
public class GGUIMonoConsortMomentsList: ALUGUIMonoVerticalMultiSizeLayout
{
    [ALHeader("判断是否在底部的数值")]
    public float bottomCheckPosition = 100;

    [ALHeader("初始化时显示的消息数量")]
    public int initMsgCount = 15;

    [ALHeader("每次获取历史消息时获取多少条历史消息")]
    public int getHistoryMsgCount = 10;
    

    [ALHeader("没有朋友圈显示空状态")] 
    public List<GameObject> noMomentsItemShow;
}

