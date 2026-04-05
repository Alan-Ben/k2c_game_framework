using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 其他人申请加入联盟的申请列表界面
    /// </summary>
    public class GGUIWndGuildOthersApplyList : _ANPGGUIBasicWnd<GGUIMonoGuildOthersApplyList>
    {
        private static GGUIWndGuildOthersApplyList _g_instance = new GGUIWndGuildOthersApplyList();
        public static GGUIWndGuildOthersApplyList instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildOthersApplyList();
                return _g_instance;
            }
        }

        //申请列表
        private GGUIWndGuildOthersApplyListGrid _m_wApplyListGrid;

        public GGUIWndGuildOthersApplyList() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildOthersApplyList.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildOthersApplyList.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);
            _m_wApplyListGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wApplyListGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wApplyListGrid?.discard();
            _m_wApplyListGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeyRefuse, _onClickOneKeyRefuse);
            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeyAgree, _onClickOneKeyAgree);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoApplyListGrid != null)
                _m_wApplyListGrid = new GGUIWndGuildOthersApplyListGrid(wnd.monoApplyListGrid);

            ALUGUICommon.combineBtnClick(wnd.btnOneKeyRefuse, _onClickOneKeyRefuse);
            ALUGUICommon.combineBtnClick(wnd.btnOneKeyAgree, _onClickOneKeyAgree);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshMemberCount();
            _refreshList();
        }

        //刷新成员数量
        private void _refreshMemberCount()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
            if (guildLevelRef == null)
                return;

            //设置成员数量
            ALUGUICommon.setLabelTxt(wnd.txtMemberCount, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, guildInfo.memberCount, guildLevelRef.member_limit));
        }

        //刷新申请列表
        private void _refreshList()
        {
            if (_m_wApplyListGrid != null)
            {
                _m_wApplyListGrid.showWnd();
                _m_wApplyListGrid.setShowData();
            }
        }

        //联盟展示信息变更
        private void _onGuildShowInfoChg()
        {
            _refreshMemberCount();
        }

        //点击一键拒绝
        private void _onClickOneKeyRefuse(GameObject _go)
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (guildInfo.joinRequestList == null || guildInfo.joinRequestList.Count == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_noJoinRequest_none);
                return;
            }

            NPPlayer.instance.guildComp.reqGuildJoinRequestAKeyDeal(false, () =>
            {
                //已一键拒绝所有申请
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_oneKeyRefuseAllRequest_none);
            });
        }

        //点击一键同意
        private void _onClickOneKeyAgree(GameObject _go)
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (guildInfo.joinRequestList == null || guildInfo.joinRequestList.Count == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_noJoinRequest_none);
                return;
            }

            NPPlayer.instance.guildComp.reqGuildJoinRequestAKeyDeal(true, () =>
            {
                if (guildInfo.joinRequestList == null || guildInfo.joinRequestList.Count == 0)
                {
                    //已一键同意所有申请
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_oneKeyAgreeAllRequest_none);
                }
                else
                {
                    //同意完后还有剩余请求
                    //联盟人数达到上限
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_oneKeyAgreeMemberLimit_none);
                }
            });
        }

        //点击关闭窗口
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_OTHERS_APPLY_LIST);
        }
    }
}