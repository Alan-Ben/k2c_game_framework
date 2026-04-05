using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会邀请弹窗
    /// </summary>
    public class GGUIWndDinnerInviteMain : _ATALBasicUIWnd<GGUIMonoDinnerInviteMain>
    {
        private static GGUIWndDinnerInviteMain _g_instance = new GGUIWndDinnerInviteMain();

        public static GGUIWndDinnerInviteMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerInviteMain();
                return _g_instance;
            }
        }

        private GDinnerInfo _m_dinnerInfo;
    
        private EDinnerInvitePageState _m_curPageState = EDinnerInvitePageState.InviteSever;
        private long _m_lSerializeOp; 
        private NPGGUIWndCommonTab _m_tabInviteFriend;
        private NPGGUIWndCommonTab _m_tabInviteGuild;
        private NPGGUIWndCommonTab _m_tabInviteServer;

        private GGUIWndDinnerInviteBasePage _m_wInviteFriendPage;
        private GGUIWndDinnerInviteGuildPage _m_wInviteGuildPage;
        private GGUIWndDinnerInviteServerPage _m_wInviteServerPage;
        

        public GGUIWndDinnerInviteMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerInviteMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerInviteMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _m_curPageState = EDinnerInvitePageState.InviteSever;
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wInviteFriendPage?.hideWnd();
            _m_wInviteGuildPage?.hideWnd();
            _m_wInviteServerPage?.hideWnd();
            _m_lSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wInviteFriendPage?.resetWnd();
            _m_wInviteGuildPage?.resetWnd();
            _m_wInviteServerPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_tabInviteFriend?.discard();
            _m_tabInviteFriend = null;
        
            _m_tabInviteGuild?.discard();
            _m_tabInviteGuild = null;
        
            _m_tabInviteServer?.discard();
            _m_tabInviteServer = null;
            
            _m_wInviteFriendPage?.discard();
            _m_wInviteFriendPage = null;
            
            _m_wInviteGuildPage?.discard();
            _m_wInviteGuildPage = null;
            
            _m_wInviteServerPage?.discard();
            _m_wInviteServerPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        
            if (null != wnd.tabInviteFriend)
            {
                _m_tabInviteFriend = new NPGGUIWndCommonTab(wnd.tabInviteFriend);
                _m_tabInviteFriend.clickDelegate += _onClickTabInviteFriend;
            }
            if (null != wnd.tabInviteGuild)
            {
                _m_tabInviteGuild = new NPGGUIWndCommonTab(wnd.tabInviteGuild);
                _m_tabInviteGuild.clickDelegate += _onClickTabInviteGuild;
            }
            if (null != wnd.tabInviteServer)
            {
                _m_tabInviteServer = new NPGGUIWndCommonTab(wnd.tabInviteServer);
                _m_tabInviteServer.clickDelegate += _onClickTabInviteServer;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onClickClose);
        }


        public void setInfo(GDinnerInfo _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
        }

        private void _onClickTabInviteFriend(bool obj)
        {
            _m_curPageState = EDinnerInvitePageState.InviteFriend;
            _refreshWnd();
        }
    
        private void _onClickTabInviteGuild(bool obj)
        {
            _m_curPageState = EDinnerInvitePageState.InviteGuild;
            _refreshWnd();
        }
    
        private void _onClickTabInviteServer(bool obj)
        {
            _m_curPageState = EDinnerInvitePageState.InviteSever;
            _refreshWnd();
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _hideAllPage();
            switch (_m_curPageState)
            {
                case EDinnerInvitePageState.InviteFriend:
                    _m_tabInviteFriend?.setSelected(true);
                    _m_tabInviteGuild?.setSelected(false);
                    _m_tabInviteServer?.setSelected(false);
                    _refreshFriendPage();
                    break;
                case EDinnerInvitePageState.InviteGuild:
                    _m_tabInviteFriend?.setSelected(false);
                    _m_tabInviteGuild?.setSelected(true);
                    _m_tabInviteServer?.setSelected(false);
                    _refreshGuildPage();
                    break;
                case EDinnerInvitePageState.InviteSever:
                    _m_tabInviteFriend?.setSelected(false);
                    _m_tabInviteGuild?.setSelected(false);
                    _m_tabInviteServer?.setSelected(true);
                    _refreshServerPage();
                    break;
            }
            NPCommonEnumStatInfo<EDinnerInviteStat>.setStat(wnd.statInfos, _m_curPageState);
        }
        //关闭所有页面
        private void _hideAllPage()
        {
            if (_m_wInviteFriendPage != null)
                _m_wInviteFriendPage.hideWnd();
            if (_m_wInviteGuildPage != null)
                _m_wInviteGuildPage.hideWnd();
            if (_m_wInviteServerPage != null)
                _m_wInviteServerPage.hideWnd();
        }
        private void _refreshFriendPage()
        {
            _m_lSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lSerializeOp;
            NPPlayer.instance.friendsComp.getFriendsDataList((_infoList) =>
            {
                if (serializeOp != _m_lSerializeOp)
                    return;
                List<DinnerInviteInfo> inviteInfos = new List<DinnerInviteInfo>();
                if (_infoList != null)
                    foreach (var friend in _infoList)
                    {
                        if (friend != null) inviteInfos.Add(new DinnerInviteInfo(friend.cid));
                    }
                if (_m_wInviteFriendPage != null)
                {
                    _m_wInviteFriendPage.setInfo(_m_dinnerInfo, inviteInfos);
                    _m_wInviteFriendPage.showWnd();
                }
                else
                {
                    _m_wInviteFriendPage = new GGUIWndDinnerInviteBasePage(wnd.pageParent);
                    _m_wInviteFriendPage.setInfo(_m_dinnerInfo, inviteInfos);
                    _m_wInviteFriendPage.load(_m_wInviteFriendPage.showWnd);
                }
            });
        }
    
        private void _refreshGuildPage()
        {
            List<DinnerInviteInfo> inviteInfos = new List<DinnerInviteInfo>();

            bool isJoinGuild = NPPlayer.instance.guildComp.isJoinGuild();
            if (isJoinGuild && NPPlayer.instance.guildComp.guildInfo != null && NPPlayer.instance.guildComp.guildInfo.memberList != null)
            {
                long playerCid = 0;
                if (NPPlayer.instance.playerInfo != null) playerCid = NPPlayer.instance.playerInfo.CID;
                foreach (GuildMemberInfo memberInfo in NPPlayer.instance.guildComp.guildInfo.memberList)
                {
                    if (memberInfo != null && memberInfo.cid != playerCid ) inviteInfos.Add(new DinnerInviteInfo(memberInfo.cid));
                }
            }
            if (_m_wInviteGuildPage != null)
            {
                _m_wInviteGuildPage.setInfo(_m_dinnerInfo, inviteInfos);
                _m_wInviteGuildPage.showWnd();
            }
            else
            {
                _m_wInviteGuildPage = new GGUIWndDinnerInviteGuildPage(wnd.pageParent);
                _m_wInviteGuildPage.setInfo(_m_dinnerInfo, inviteInfos);
                _m_wInviteGuildPage.load(_m_wInviteGuildPage.showWnd);
            }
        }
    
        private void _refreshServerPage()
        {
            _m_lSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lSerializeOp;
            NPPlayer.instance.dinnerComp.reqServerInviteList(wnd.serverInviteCount, _inviteInfos =>
            {    
                if (serializeOp != _m_lSerializeOp)
                    return;
                if (_m_wInviteServerPage != null)
                {
                    _m_wInviteServerPage.setInfo(_m_dinnerInfo, _inviteInfos);
                    _m_wInviteServerPage.showWnd();
                }
                else
                {
                    _m_wInviteServerPage = new GGUIWndDinnerInviteServerPage(wnd.pageParent);
                    _m_wInviteServerPage.setInfo(_m_dinnerInfo, _inviteInfos);
                    _m_wInviteServerPage.load(_m_wInviteServerPage.showWnd);
                }
            });
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_INVITE);
        }
    }
}