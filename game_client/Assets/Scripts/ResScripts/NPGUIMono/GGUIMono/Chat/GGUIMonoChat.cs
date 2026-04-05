
using ALPackage;
using ChatPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EChatPageType
    {
        CHAT_INFO_LIST,//聊天列表
        PRIVATE_CHAT_LIST,//私聊总列表
        FRIEND,//好友列表
    }
    
    public class GGUIMonoChat : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;

        [ALHeader("关闭背景")]
        public GameObject closeBgkBtn;
        [ALHeader("设置按钮")]
        public GameObject settingBtn;
        [ALHeader("私聊总列表按钮")]
        public NPGGUIMonoCommonTab tabPrivateChat;
        [ALHeader("好友相关入口按钮")]
        public NPGGUIMonoCommonTab tabFriend;
        [ALHeader("加载聊天信息的页面的父节点")]
        public Transform pageChatInfoParent;
        [ALHeader("加载私聊列表的页面的父节点")]
        public Transform pagePrivateChatParent;
        [ALHeader("加载的好友页面的父节点")]
        public Transform pageFriendParent;
        [ALHeader("聊天会话列表(私聊)")]
        public GGUIMonoChatPrivateChannleItemGrid monoChatInfoGrid;
        [ALHeader("聊天会话列表(私聊以外的)")]
        public GGUIMonoChatChannleContainer chatInfoContainer;
        [ALHeader("页面状态类型配置")]
        public List<NPCommonEnumStatInfo<EChatPageType>> statInfos;
        [ALHeader("私聊会话列表显示数量上限，填-1无上限")]
        public int limitPrivateShowCount = -1;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1300); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1300);} }
    }
}