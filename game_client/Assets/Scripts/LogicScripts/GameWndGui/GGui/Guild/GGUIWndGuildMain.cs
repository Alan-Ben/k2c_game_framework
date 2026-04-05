using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟主界面
    /// </summary>
    public class GGUIWndGuildMain : _ANPGGUIBasicWnd<GGUIMonoGuildMain>
    {
        private static GGUIWndGuildMain _g_instance = new GGUIWndGuildMain();
        public static GGUIWndGuildMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildMain();
                return _g_instance;
            }
        }

        //盟主形象展示
        private NPGGUIWndCommonShowCase _m_wLeaderShowcase;
        //联盟基础信息
        private GGUIWndGuildSubBaseInfo _m_wSubBaseInfo;

        private NPGGUISubWndMiniChat _m_chatMiniWnd; // 聊天入口
        
        public GGUIWndGuildMain() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildMain.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildMain.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            _checkImpeach();
            
            _m_wSubBaseInfo?.showWnd();
            _m_chatMiniWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wLeaderShowcase?.hideWnd();
            _m_wSubBaseInfo?.hideWnd();
            _m_chatMiniWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLeaderShowcase?.resetWnd();
            _m_wSubBaseInfo?.resetWnd();
            _m_chatMiniWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wLeaderShowcase?.discard();
            _m_wLeaderShowcase = null;
            _m_wSubBaseInfo?.discard();
            _m_wSubBaseInfo = null;
            _m_chatMiniWnd?.discard();
            _m_chatMiniWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnLevelPreview, _onClickLevelPreview);//点击等级预览
            ALUGUICommon.uncombineBtnClick(wnd.btnConstruct, _onClickGuildConstruct);//点击联盟建设
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildInfo, _onClickGuildInfo);//点击联盟信息
            ALUGUICommon.uncombineBtnClick(wnd.btnMember, _onClickGuildMember);//点击联盟成员
            // ALUGUICommon.uncombineBtnClick(wnd.btnShop, _onClickGuildShop);//点击联盟商店
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickGuildRank);//点击联盟排行
            ALUGUICommon.uncombineBtnClick(wnd.btnEntrust, _onClickGuildEntrust);//点击联盟委托
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildGift, _onClickGuildGift);//点击联盟礼物
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildDispatch, _onClickDispatch);//点击联盟派遣
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildCooperate, _onClickCooperate);//点击联盟协作
            ALUGUICommon.uncombineBtnClick(wnd.btnHelp, _onClickHelp);//点击联盟协作
            ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnExpandAnnouncement, _onClickExpandAnnouncement);//点击展开公告
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLeaderShowcase != null)
                _m_wLeaderShowcase = new NPGGUIWndCommonShowCase(wnd.monoLeaderShowcase);

            if (wnd.monoBaseInfo != null)
                _m_wSubBaseInfo = new GGUIWndGuildSubBaseInfo(wnd.monoBaseInfo);

            if (wnd.chatMiniWndMono != null)
                _m_chatMiniWnd = new NPGGUISubWndMiniChat(wnd.chatMiniWndMono);
            
            ALUGUICommon.combineBtnClick(wnd.btnLevelPreview, _onClickLevelPreview);//点击等级预览
            ALUGUICommon.combineBtnClick(wnd.btnConstruct, _onClickGuildConstruct);//点击联盟建设
            ALUGUICommon.combineBtnClick(wnd.btnGuildInfo, _onClickGuildInfo);//点击联盟信息
            ALUGUICommon.combineBtnClick(wnd.btnMember, _onClickGuildMember);//点击联盟成员
            // ALUGUICommon.combineBtnClick(wnd.btnShop, _onClickGuildShop);//点击联盟商店
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickGuildRank);//点击联盟排行
            ALUGUICommon.combineBtnClick(wnd.btnEntrust, _onClickGuildEntrust);//点击联盟委托
            ALUGUICommon.combineBtnClick(wnd.btnGuildGift, _onClickGuildGift);//点击联盟礼物
            ALUGUICommon.combineBtnClick(wnd.btnGuildDispatch, _onClickDispatch);//点击联盟派遣
            ALUGUICommon.combineBtnClick(wnd.btnGuildCooperate, _onClickCooperate);//点击联盟协作
            ALUGUICommon.combineBtnClick(wnd.btnHelp, _onClickHelp);//点击联盟协作
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnExpandAnnouncement, _onClickExpandAnnouncement);//点击展开公告
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBaseInfo();
        }

        //刷新基础信息
        private void _refreshBaseInfo()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (_m_wSubBaseInfo != null)
            {
                _m_wSubBaseInfo.showWnd();
                _m_wSubBaseInfo.setBaseInfo(guildInfo);
                _m_wSubBaseInfo.setWealth(guildInfo.guildWealth);
            }

            ALUGUICommon.setLabelTxt(wnd.txtAnnouncement, guildInfo.announcement);

            //重置动画
            bool isExpand = !AccountSettingMgr.instance.accountSetting.guildAnnouncementIsClose;
            wnd.aniAnnouncement?.sample(isExpand ? EGuildAnnouncementExpandAniType.EXPAND : EGuildAnnouncementExpandAniType.CLOSE, 1);
        }


        //检查是否被弹劾
        private void _checkImpeach()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;

            //如果盟主不是自己则不用处理
            if (guildInfo == null || guildInfo.leaderId != NPPlayer.instance.playerInfo.CID)
                return;

            //如果没有弹劾事件则不处理
            Guild_EventInfo eventInfo = NPPlayer.instance.guildComp.getEventInfo(EGuildEventType.IMPEACH_LEADER);
            if (eventInfo == null)
                return;

            GuildEvent_ImpeachLeader impeachLeader = new GuildEvent_ImpeachLeader();
            impeachLeader.readPackage(eventInfo.getData());

            //如果正在弹劾中
            if (impeachLeader.getState() == EGuildImpeachLeaderEventState.IN_IMPEACH)
            {
                //您多日未上线，联盟疏于管理，联盟成员有所不满，是否继续兼任盟主职位？
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_leaderImpeachTip_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.guild_resignationPosition_none),//卸任职位
                    () =>
                    {
                        NPPlayer.instance.guildComp.reqApproveLeaderImpeachEvent(eventInfo.getDbId(), null);
                    },
                    TextTranslate.instance.getLanguage(TransKeyConst.guild_serveAsLeader_none),//担任盟主
                    () =>
                    {
                        NPPlayer.instance.guildComp.reqCancelLeaderImpeachEvent(eventInfo.getDbId(), null);
                    });
            }
        }

        #region 点击事件

        //点击等级预览按钮
        private void _onClickLevelPreview(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(
                GGUIWndGuildLevelPreview.instance, 
                GGUIWndGuildLevelPreview.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst_Guild.C_GUILD_LEVEL_PREVIEW,
                false, false);
        }

        //点击联盟信息按钮
        private void _onClickGuildInfo(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(
                GGUIWndGuildSelfInfo.instance,
                UINodeTagConst_Guild.C_GUILD_INFO);
        }

        //点击联盟建设按钮
        private void _onClickGuildConstruct(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeGuildDonate());
        }

        //点击联盟成员按钮
        private void _onClickGuildMember(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(
                GGUIWndGuildMemberList.instance,
                UINodeTagConst_Guild.C_GUILD_MEMBER_LIST);
        }

        //点击联盟商店按钮
        private void _onClickGuildShop(GameObject _go)
        {

        }

        //点击联盟排行按钮
        private void _onClickGuildRank(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(new GGUIWndGuildRank(4916, false, true), UINodeTagConst_Guild.C_GUILD_RANK, 0);
        }

        /// <summary>
        /// 点击联盟委托按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGuildEntrust(GameObject _go)
        {
            // QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_ENTRUST, true, false
            //     , false, GMainGUIAddSceneGuildMain.instance, GGUIWndGuildEntrust.instance, false, false,
            //     () =>
            //     {
            //         
            //     }, null, () =>
            //     {
            //         GGUIWndGuildEntrust.instance.hideWnd();
            //     }));
            
            QueueMgr.instance.AddNode(new GNodeGuildEntrust());
        }
        
        /// <summary>
        /// 点击联盟礼物按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGuildGift(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildBoxMain.instance, UINodeTagConst_Guild.C_GUILD_BOX, null, null, 0);
        }

        /// <summary>
        /// 点击联盟派遣按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDispatch(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeGuildDispatch());
        }

        /// <summary>
        /// 点开联盟协作
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCooperate(GameObject _go)
        {
            // 判断是否解锁
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_simple_unlock_id, true))
                return;

            QueueMgr.instance.AddNode(new GNodeGuildCooperate());
        }
        
        /// <summary>
        /// 点开联盟互助
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickHelp(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildMarsHelp.instance, UINodeTagConst.C_GUIlD_MARS_HELP, null, null, 0);
        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_MAIN);
        }

        /// <summary>
        /// 点击展开关闭公告按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickExpandAnnouncement(GameObject _go)
        {
            bool isExpand = !AccountSettingMgr.instance.accountSetting.guildAnnouncementIsClose;
            wnd?.aniAnnouncement?.forcePlay(isExpand ? EGuildAnnouncementExpandAniType.CLOSE : EGuildAnnouncementExpandAniType.EXPAND);
            AccountSettingMgr.instance.accountSetting.setGuildAnnouncementIsClose(isExpand);
        }

        #endregion
    }
}