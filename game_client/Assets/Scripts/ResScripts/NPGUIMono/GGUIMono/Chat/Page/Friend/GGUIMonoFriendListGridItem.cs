using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;


/// <summary>
/// 好友列表item
/// </summary>
public class GGUIMonoFriendListGridItem : _TALUGUIMonoGridItem
{
    [ALHeader("玩家头像")]
    public NPGGUIMonoPlayerIcon playerIcon;
    [ALHeader("离线时长文本")]
    public TextEx offLineTxt;
    [ALHeader("公会名称")]
    public TextEx txtGuildName;
    [ALHeader("有无联盟的显示")]
    public List<GameObject> listHasGuildShow;
    public List<GameObject> listNoGuildShow;
    [ALHeader("私聊按钮")]
    public GameObject btnChat;
    [ALHeader("滑动的go")]
    public GameObject dragObj;
    [ALHeader("移除按钮")]
    public GameObject btnRemove;
    [ALHeader("滑动出现操作菜单的阈值")]
    public float swipeThreshold = 100f;
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
    [ALHeader("滑入显示操作列表，滑出隐藏")]
    public List<GameObject> dragInShow;
    [ALHeader("滑出显示操作列表，滑入隐藏")]
    public List<GameObject> dragInHide;
    [ALHeader("在线显示，离线隐藏")]
    public List<GameObject> onlineShow;
    [ALHeader("离线显示，在线隐藏")]
    public List<GameObject> onlineHide;
    [ALHeader("展开列表的的时候播放的动画")]
    public string forceShowAniName;
    [ALHeader("收起列表的时候播放的动画")]
    public string forceHideAniName;
    
    
    public void setHasGuildShow(bool _hasGuild) 
    {
        ALUGUICommon.setGameObjEnable(listHasGuildShow, false);
        ALUGUICommon.setGameObjEnable(listNoGuildShow, false);
        ALUGUICommon.setGameObjEnable(_hasGuild ? listHasGuildShow : listNoGuildShow, true);
    }
}
