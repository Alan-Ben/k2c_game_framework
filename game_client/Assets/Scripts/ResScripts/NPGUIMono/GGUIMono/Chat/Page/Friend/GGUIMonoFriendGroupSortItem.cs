using System.Collections.Generic;
using ALPackage;
using UnityEngine;

public class GGUIMonoFriendGroupSortItem : _AALBasicUIWndMono
{
    [ALHeader("分组名字")]
    public TextEx txtGroupName;
    [ALHeader("删除分组按钮")]
    public GameObject btnRemove;
    [ALHeader("监听拖拽的按钮")]
    public GameObject dragGo;
    [ALHeader("拖拽过程中跟随的go")]
    public RectTransform dragFollowGo;
    [ALHeader("拖拽过程中显示的go，反之隐藏")]
    public List<GameObject> onDragShow;
    [ALHeader("拖拽结束后移动的目的地go")]
    public RectTransform dragEndTargetGo;
    [ALHeader("检测需要换位置的范围内的差值Y")]
    public float checkInRectOffSetY = 60;
}