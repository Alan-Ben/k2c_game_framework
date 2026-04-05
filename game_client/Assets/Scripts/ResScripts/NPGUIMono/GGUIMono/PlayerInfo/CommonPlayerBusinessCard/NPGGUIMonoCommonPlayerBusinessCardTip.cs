using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// /玩家的个人详情信息弹窗
    /// </summary>
    public class NPGGUIMonoCommonPlayerBusinessCardTip:NPGGUIMonoCommonToolTip
    {
        [ALHeader("玩家头像Mono")]
        public NPGGUIMonoPlayerIcon playerIcon;

        [ALHeader("联盟名称")]
        public TextEx txtAllinceName;
        [ALHeader("性别图标")]
        public RawImage texGender;

        [ALHeader("自己显示的go列表")]
        public List<GameObject> selfShowGoList;
        [ALHeader("自己隐藏的go列表")]
        public List<GameObject> selfHideGoList;
        
        [ALHeader("加好友")]
        public GameObject btnAddFriend;
        [ALHeader("删除好友")]
        public GameObject btnDeleteFriend;

        [ALHeader("是好友显示的GoList")]
        public List<GameObject> isFriendShowGoList;
        [ALHeader("不是好友显示的GoList")]
        public List<GameObject> noFriendShowGoList;

        [ALHeader("私聊")]
        public GameObject btnPersonalChat;
        [ALHeader("举报")]
        public GameObject btnReport;
        [ALHeader("屏蔽")]
        public GameObject btnShield;
        [ALHeader("取消屏蔽")]
        public GameObject btnUnShield;
        [ALHeader("已屏蔽显示的GoList")]
        public List<GameObject> isShieldShowGoList;
        [ALHeader("未屏蔽显示的GoList")]
        public List<GameObject> noShieldShowGoList;
        [ALHeader("个人空间")]
        public GameObject btnDetailInfo;
        [ALHeader("自己信息的窗口宽")]
        public float selfWndWidth;
        [ALHeader("自己信息的窗口高")]
        public float selfWndHeight;
        [ALHeader("其他人信息的窗口宽")]
        public float otherWndWidth;
        [ALHeader("其他人信息的窗口高")]
        public float otherWndHeight;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_PLAYER_BUSINESS_CARD_TIP); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_PLAYER_BUSINESS_CARD_TIP);} }
    }
}