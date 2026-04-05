
using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 聊天界面
    /// </summary>
    public class GGUIWndChat : _ANPGGUIBasicWnd<GGUIMonoChat>
    {
        private static GGUIWndChat _g_instance;
        public static GGUIWndChat instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndChat();
                return _g_instance;
            }
        }

        // 当前显示的聊天会话和聊天会话的配置信息
        private _AChatInfo _m_chatInfo;

        // 这个窗口涉及的一些子窗口
        private GGUIWndChatInfoListPage _m_chatInfoListPage;

        //侧边的私聊会话grid
        private GGUIWndChatPrivateChannleItemGrid _m_chatInfoGird;
        //完整私聊列表
        private GGUIWndPrivateChatListPage _m_privateChatListPage;
        //好友列表
        private GGUIWndFriendListPage _m_friendListPage;
        //侧边的除了私聊以外的聊天频道会话Container
        private GGUIWndChatChannleContainer _m_chatInfoContainer;
        private NPGGUIWndCommonTab _m_tabPrivateChat;
        private NPGGUIWndCommonTab _m_tabFriend;
        private EChatPageType _m_curPageType;
        private string _m_lastChatId;//上次显示的聊天id
        private bool _m_needRefreshChatInfoList = false;

        public GGUIWndChat() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoChat.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChat.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            NPPlayer.instance.chatComp.onChatDataCreat += _onChatDataCreat;
            NPPlayer.instance.chatComp.onPrivateChatRemove += _refreshWnd;
            NPPlayer.instance.chatComp.onPrivateChatAdd += _refreshWnd;
            NPPlayer.instance.chatComp.onChatRoomAdd += _onChatRoomAdd;
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_UP_TO_TOP_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _onShieldChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_SENDED_PRIVATE_MSG_DONE, _onPrivateMsgChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_RECEIVE_PRIVATE_MSG, _onPrivateMsgChg);
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.chatComp.onChatDataCreat -= _onChatDataCreat;
            NPPlayer.instance.chatComp.onPrivateChatRemove -= _refreshWnd;
            NPPlayer.instance.chatComp.onPrivateChatAdd -= _refreshWnd;
            NPPlayer.instance.chatComp.onChatRoomAdd -= _onChatRoomAdd;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_UP_TO_TOP_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _onShieldChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_SENDED_PRIVATE_MSG_DONE, _onPrivateMsgChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_RECEIVE_PRIVATE_MSG, _onPrivateMsgChg);
            
            _m_privateChatListPage?.hideWnd();
            _m_friendListPage?.hideWnd();
        }

        protected override void _onReset()
        {
            
            _m_chatInfoGird?.resetWnd();
            _m_chatInfoContainer?.resetWnd();
            _m_privateChatListPage?.resetWnd();
            _m_friendListPage?.resetWnd();
            NPPlayer.instance.chatComp.setCurChatShow(null);
            _m_chatInfo = null;
            _m_lastChatId = null;

        }

        protected override void _onDiscard()
        {
            _m_chatInfoListPage?.discard();
            _m_chatInfoListPage = null;
            
            if (null != _m_chatInfoGird)
            {
                _m_chatInfoGird.itemClickAction -= _itemClickDelegate;
                _m_chatInfoGird.discard();
            }
            _m_chatInfoGird = null;

            if (null != _m_chatInfoContainer)
            {
                _m_chatInfoContainer.itemClickAction -= _itemClickDelegate;
                _m_chatInfoContainer.discard();
            }
            _m_chatInfoContainer = null;

            _m_tabPrivateChat?.discard();
            _m_tabPrivateChat = null;
            _m_tabFriend?.discard();
            _m_tabFriend = null;

            if (null != _m_privateChatListPage)
            {
                _m_privateChatListPage.discard();
                _m_privateChatListPage.itemClickAction -= _itemClickDelegate;
            }
            _m_privateChatListPage = null;

            if (null != _m_friendListPage)
            {
                _m_friendListPage?.discard();
                _m_friendListPage = null;
            }
            _m_chatInfo = null;
            _m_lastChatId = null;
            _m_needRefreshChatInfoList = true;
        }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                if (wnd.monoChatInfoGrid)
                {
                    _m_chatInfoGird = new GGUIWndChatPrivateChannleItemGrid(wnd.monoChatInfoGrid);
                    _m_chatInfoGird.itemClickAction += _itemClickDelegate;
                }

                if (null != wnd.chatInfoContainer)
                {
                    _m_chatInfoContainer = new GGUIWndChatChannleContainer(wnd.chatInfoContainer);
                    _m_chatInfoContainer.itemClickAction += _itemClickDelegate;
                }

                if (null != wnd.tabFriend)
                {
                    _m_tabFriend = new NPGGUIWndCommonTab(wnd.tabFriend);
                    _m_tabFriend.clickDelegate += _clickFriend;
                }

                if (null != wnd.tabPrivateChat)
                {
                    _m_tabPrivateChat = new NPGGUIWndCommonTab(wnd.tabPrivateChat);
                    _m_tabPrivateChat.clickDelegate += _clickPrivateChat;
                }
                ALUGUICommon.combineBtnClick(wnd.closeBtn, _onClickClose);
                ALUGUICommon.combineBtnClick(wnd.closeBgkBtn, _onClickClose);
                ALUGUICommon.combineBtnClick(wnd.settingBtn, _onClickSetting);
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        
        /// <summary>
        /// 点击设置按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onClickSetting(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerInfoDress.instance, GGUIWndPlayerInfoDress.instance.showWnd, UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_DRESS_NODE);
        }
        

        private void _onShieldChg()
        {
            // _m_chatInfo
            if (null != _m_chatInfo && _m_chatInfo is NPPrivateChatInfo privateChatInfo)
            {
                if (NPPlayer.instance.friendsComp.isShield(privateChatInfo.userInfo.cid))
                {
                    setShowData(NPPlayer.instance.chatComp.getRoomChatInfo(ENPChatRoomType.US_SERVER));
                }
                else
                {
                    setShowData(_m_chatInfo);
                }
            }
            else
            {
                setShowData(_m_chatInfo);
            }
        }

        /// <summary>
        /// 私聊消息变化
        /// </summary>
        private void _onPrivateMsgChg()
        {
            //刷新私聊频道列表
            _refreshPrivateChatChannleGrid();
        }
        
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <remarks>
        /// 需要指定聊天会话和对应的聊天会话的配置
        /// </remarks>
        public void setShowData(_AChatInfo _chatInfo)
        {
            if (null == _chatInfo || !(_chatInfo is _INPChatInfo))
                return;
            if(null != _m_chatInfo)
            {
                _m_chatInfo.onReceiveMsg -= _onReceiveMsg;
            }

            _m_needRefreshChatInfoList = true;
            _m_chatInfo = _chatInfo;
            _m_chatInfo.onReceiveMsg += _onReceiveMsg;
            _refreshCurPrivateChatInfo();
            NPPlayer.instance.chatComp.setCurChatShow(_m_chatInfo);
            NPPlayer.instance.chatComp.setCurChatReaded(_m_chatInfo);
            refreshWnd();
        }
        
        private void _onChatRoomAdd(NPRoomChatInfo _info)
        {
            if (null == _info)
                return;
            if (null == _m_chatInfo || (_m_chatInfo is  NPRoomChatInfo && _info.id == _m_chatInfo.id))
            {
                //断开room再重新加入的时候，——info是重新创建的，
                setShowData(_info);
            }
        }

        /// <summary>
        /// 刷新全部显示
        /// </summary>
        public void refreshWnd()
        {
            if (null == wnd)
                return;
            _refreshChatInfoList();
            _refreshWnd();
        }
        
        private void _clickPrivateChat(bool _isOn)
        {
            setCurPageType(EChatPageType.PRIVATE_CHAT_LIST);
        }

        private void _clickFriend(bool _isOn)
        {
            setCurPageType(EChatPageType.FRIEND);
        }

        /// <summary>
        /// 设置当前显示的界面
        /// </summary>
        /// <param name="_type"></param>
        public void setCurPageType(EChatPageType _type)
        {
            _m_curPageType = _type;
            if (_m_bIsShow)
                _refreshPageStat();

            if (_type == EChatPageType.FRIEND)
            {
                //用于触发引导
                QueueMgr.instance.addNode_OnlyOp(UINodeTagConst_Friends.C_PAGE_FRIEND_LIST);
            }
        }

        /// <summary>
        /// 刷新选中
        /// </summary>
        private void _refreshSelected()
        {
            _m_tabFriend?.setSelected(_m_curPageType == EChatPageType.FRIEND);
            _m_tabPrivateChat?.setSelected(_m_curPageType == EChatPageType.PRIVATE_CHAT_LIST);
            _m_chatInfoGird?.setSelectItem((null == _m_chatInfo || _m_curPageType != EChatPageType.CHAT_INFO_LIST) ? null : _m_chatInfo as _INPChatInfo);
            _m_chatInfoContainer?.setSelectItem((null == _m_chatInfo || _m_curPageType != EChatPageType.CHAT_INFO_LIST) ? null : _m_chatInfo as _INPChatInfo);
        }

        /// <summary>
        /// 收到消息的时候，当前频道设置为已读
        /// </summary>
        /// <param name="_obj"></param>
        private void _onReceiveMsg(MsgInfo _obj)
        {
            if (!isShow)
                return;
            if (null == _m_chatInfo)
                return;
            if(_m_curPageType != EChatPageType.CHAT_INFO_LIST)//当前不是在聊天界面，就返回
                return;
            NPPlayer.instance.chatComp.setCurChatReaded(_m_chatInfo);
        }

        /// <summary>
        /// 聊天数据创建的时候
        /// </summary>
        private void _onChatDataCreat()
        {
            if (null == _m_chatInfo)
                _m_chatInfo = NPPlayer.instance.chatComp.chatRoomList.GetFirst();

            if (null == _m_chatInfo)
            {
                ALLog.Error("[NPGGUISubWndMiniChat._onBtnOpenClick Error] : 当前不存在 chatInfo 无法进入聊天界面");
                return;
            }

            _refreshCurPrivateChatInfo();

            if (_m_bIsShow)
                refreshWnd();
        }

        /// <summary>
        /// 刷新私聊频道的玩家信息
        /// </summary>
        private void _refreshCurPrivateChatInfo()
        {
            //当前频道为空或者不是私聊频道不处理
            if (null == _m_chatInfo || !(_m_chatInfo is NPPrivateChatInfo))
                return;
            NPPrivateChatInfo privateChatInfo = _m_chatInfo as NPPrivateChatInfo;
            privateChatInfo?.regPlayerInfo(_refreshWnd);
        }

        /// <summary>
        /// 刷新窗口，不包含聊天信息列表
        /// 聊天信息列表选中聊天频道或者初次进入聊天才会刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;
            _refreshPrivateChatChannleGrid();
            _refreshChatChannleContainer();
            //刷新状态显示
            _refreshPageStat();
        }

        /// <summary>
        /// 刷新好友列表显示
        /// </summary>
        private void _refreshFriendList()
        {
            if(null == wnd)
                return;

            if (null != _m_friendListPage)
            {
                _m_friendListPage.showWnd();
            }
            else
            {
                _m_friendListPage = new GGUIWndFriendListPage(wnd.pageFriendParent);
                _m_friendListPage.regLoadDoneDelegate(() =>
                {
                    _m_friendListPage.showWnd();
                });
                _m_friendListPage.load();
            }
        }

        /// <summary>
        /// 刷新私聊列表
        /// </summary>
        private void _refreshPrivateChatList()
        {
            if(null == wnd)
                return;

            if (null != _m_privateChatListPage)
            {
                _m_privateChatListPage.showWnd();
                _m_privateChatListPage.refreshWnd();
            }
            else
            {
                _m_privateChatListPage = new GGUIWndPrivateChatListPage(wnd.pagePrivateChatParent);
                _m_privateChatListPage.itemClickAction += _itemClickDelegate;
                _m_privateChatListPage.regLoadDoneDelegate(() =>
                {
                    _m_privateChatListPage.showWnd();
                    _m_privateChatListPage.refreshWnd();
                });
                _m_privateChatListPage.load();
            }
        }

        /// <summary>
        /// 刷新聊天信息列表显示
        /// </summary>
        private void _refreshChatInfoList()
        {
            if(null == wnd)
                return;
            
            if(null == _m_chatInfo)
                return;


            if (!_m_needRefreshChatInfoList && null != _m_lastChatId && _m_lastChatId == _m_chatInfo.id) //同一个聊天不需要刷新
            {
                return;
            }
            _m_lastChatId = _m_chatInfo.id;
            _m_needRefreshChatInfoList = false;

            // 设置消息列表
            if (null == _m_chatInfoListPage)
            {
                _m_chatInfoListPage = new GGUIWndChatInfoListPage(wnd.pageChatInfoParent);
                _m_chatInfoListPage.regLoadDoneDelegate(() =>
                {
                    _m_chatInfoListPage.showWnd();
                    _m_chatInfoListPage.setInfo(_m_chatInfo);
                });
                _m_chatInfoListPage.load();
            }
            else
            {
                _m_chatInfoListPage.showWnd();
                _m_chatInfoListPage.setInfo(_m_chatInfo);
            }
        }

        /// <summary>
        /// 刷新私聊频道列表
        /// </summary>
        private void _refreshPrivateChatChannleGrid()
        {
            if (wnd == null)
                return;

            List<_INPChatInfo> infoList = GChatUtil.getPrivateeChatChannleList();
            List<_INPChatInfo> targetList = infoList.Count > 0 && wnd.limitPrivateShowCount > 0
                ? infoList.GetRange(0, Math.Min(wnd.limitPrivateShowCount, infoList.Count))
                : infoList;
            _m_chatInfoGird?.showWnd();
            _m_chatInfoGird?.refresh(targetList);
        }

        /// <summary>
        /// 刷新频道列表
        /// </summary>
        private void _refreshChatChannleContainer()
        {
            List<_AChatInfo> chatInfoList = new List<_AChatInfo>();
            NPPlayer.instance.chatComp.getAllChatInfo(chatInfoList);
            List<_INPChatInfo> infoList = new List<_INPChatInfo>();
            for (int i = 0; i < chatInfoList.Count; i++)
            {
                _AChatInfo temp = chatInfoList[i];
                if (null == temp)
                    continue;
                
                if (temp is _APrivateChatInfo privateChatInfo)//剔除私聊频道
                    continue;

                if (temp is NPRoomChatInfo {needShow: false}) //剔除不需要显示的频道
                    //不需要显示
                    continue;

                infoList.Add(temp as _INPChatInfo);
            }
            infoList.Sort(_sortNormalChannle);
            _m_chatInfoContainer?.showWnd();
            _m_chatInfoContainer?.refresh(infoList);
        }

        /// <summary>
        /// 常规排序规则
        /// </summary>
        private int _sortNormalChannle(_INPChatInfo _x, _INPChatInfo _y)
        {
            //排序规则
            return -_x.getSortId().CompareTo(_y.getSortId());
        }

        private void _refreshPageStat()
        {
            NPCommonEnumStatInfo<EChatPageType>.setStat(wnd.statInfos, _m_curPageType);
            
            //刷新选中状态
            _refreshSelected();
            switch (_m_curPageType)
            {
                case EChatPageType.CHAT_INFO_LIST:
                    _m_privateChatListPage?.hideWnd();
                    _m_friendListPage?.hideWnd();
                    break;
                case EChatPageType.PRIVATE_CHAT_LIST:
                    _m_friendListPage?.hideWnd();
                    _refreshPrivateChatList();
                    break;
                case EChatPageType.FRIEND:
                    _m_privateChatListPage?.hideWnd();
                    _refreshFriendList();
                    break;
            }
        }

        /// <summary>
        /// 侧边会话点击事件回调
        /// </summary>
        /// <param name="_info"></param>
        private void _itemClickDelegate(_INPChatInfo _info)
        {
            if(null == _info || !(_info is _AChatInfo chatInfo))
                return;

            if (null != _m_chatInfo && chatInfo.id == _m_chatInfo.id && _m_curPageType == EChatPageType.CHAT_INFO_LIST)
            {
                return;
            }

            setShowData(chatInfo);
            setCurPageType(EChatPageType.CHAT_INFO_LIST);
        }

        /// <summary>
        /// 获取聊天信息列表的显示区域
        /// </summary>
        /// <returns></returns>
        public RectTransform getChatInfoListViewport()
        {
            
            if (null != _m_chatInfoListPage && null != _m_chatInfoListPage.wnd.monoMsgList
                                                 && null != _m_chatInfoListPage.wnd.monoMsgList.scrollRect
                                                 && null != _m_chatInfoListPage.wnd.monoMsgList.scrollRect.viewport)
            {
                return _m_chatInfoListPage.wnd.monoMsgList.scrollRect.viewport;
            }

            return Game.instance.mainCamera.uiRootRectTrans;
        }
    }
}