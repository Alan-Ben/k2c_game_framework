using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

/// <summary>
/// 聊天频道操作类型
/// </summary>
public enum EChatChannleOperationStat
{
    NORMAL,//可拖拽,可点击
    CAN_SELECTE,//可选择
}

/// <summary>
/// 聊天会话item
/// </summary>
public class GGUIMonoChatListGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("聊天频道item")]
    public GGUIMonoChatChannleItem channleItem;
    [ALHeader("选中的tog")]
    public NPGGUIMonoCommonToggleEx togSelected;
    [ALHeader("点击的go")]
    public GameObject btnClick;
    [ALHeader("滑动的go")]
    public GameObject dragObj;
    [ALHeader("置顶按钮")]
    public GameObject btnUp;
    [ALHeader("取消置顶按钮")]
    public GameObject btnDisUp;
    [ALHeader("移除按钮")]
    public GameObject btnRemove;
    [ALHeader("滑动出现操作菜单的阈值")]
    public float swipeThreshold = 100f;
    [ALHeader("偏移开始真正拖拽的阈值（X轴超过开始拖拽，Y轴超过忽略拖拽）")]
    public float offsetThreshold = 10f;
    [ALHeader("滑动速度（每秒）")]
    public float swipeVelocity = 1080f;
    [ALHeader("用于跟随滑动而移动的go")]
    public RectTransform dragMoveGo;
    [ALHeader("用于跟随滑动而移动的go坐标x限制范围")]
    public ComFloatRange movePosXRange;
    [ALHeader("移动的go的左终点")]
    public RectTransform dragLeftPos;
    [ALHeader("移动的go的右终点")]
    public RectTransform dragRightPos;
    [ALHeader("不同操作状态下的UI配置")]
    public List<NPCommonEnumStatInfo<EChatChannleOperationStat>> statInfos;
    [ALHeader("置顶显示，没有置顶隐藏")]
    public List<GameObject> showUpToTop;
    [ALHeader("没有置顶显示，置顶隐藏")]
    public List<GameObject> hideUpToTop;
    [ALHeader("左滑显示，右滑隐藏")]
    public List<GameObject> showOnDragLeft;
    [ALHeader("右滑显示，左滑隐藏")]
    public List<GameObject> hideOnDragLeft;
}
