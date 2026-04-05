using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EDinnerInviteStat
    {
        [InspectorName("已邀请")]
        HAS_INVITED,//已邀请
        [InspectorName("未邀请")]
        WAIT_INVITE,//等待邀请
        [InspectorName("已加入")]
        ALREADY_JOIN,//已加入
    }
    
    /// <summary>
    /// 宴会好友邀请item
    /// </summary>
    public class GGUIMonoDinnerInviteFriendItem : _TALUGUIMonoGridItem
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon playerIcon;
        [ALHeader("邀请按钮")]
        public GameObject btnInvite;
        // [ALHeader("参加我的宴会次数")]
        // public TextEx txtJoinMyNum;
        [ALHeader("在线显示，离线隐藏")]
        public List<GameObject> onlineShow;
        [ALHeader("离线显示，在线隐藏")]
        public List<GameObject> onlineHide;
        [ALHeader("离线时长文本")]
        public TextEx offLineTxt;
        [ALHeader("不同状态配置")]
        public List<NPCommonEnumStatInfo<EDinnerInviteStat>> statInfos;
    }
}
