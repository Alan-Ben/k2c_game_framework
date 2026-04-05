using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟申请界面
    /// </summary>
    public class GGUIWndGuildApply : _ANPGGUIBasicResBarWnd<GGUIMonoGuildApply>
    {
        private static GGUIWndGuildApply _g_instance = new GGUIWndGuildApply();
        public static GGUIWndGuildApply instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildApply();
                return _g_instance;
            }
        }

        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;
        //加入联盟倒计时
        private NPGGUIWndCommonCountDown _m_wJoinCountDownWnd;

        public GGUIWndGuildApply() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildApply.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildApply.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wRewardContainer?.hideWnd();
            _m_wJoinCountDownWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
            _m_wJoinCountDownWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;

            _m_wJoinCountDownWnd?.discard();
            _m_wJoinCountDownWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnQuickJoin, _onClickQuickJoin);
            ALUGUICommon.uncombineBtnClick(wnd.btnCreate, _onClickCreate);
            ALUGUICommon.uncombineBtnClick(wnd.btnMore, _onClickMore);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoItemContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            if (wnd.monoJoinCD != null)
                _m_wJoinCountDownWnd = new NPGGUIWndCommonCountDown(wnd.monoJoinCD);

            ALUGUICommon.combineBtnClick(wnd.btnQuickJoin, _onClickQuickJoin);
            ALUGUICommon.combineBtnClick(wnd.btnCreate, _onClickCreate);
            ALUGUICommon.combineBtnClick(wnd.btnMore, _onClickMore);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshReward();
            _refreshCD();
        }

        //刷新首次奖励列表
        private void _refreshReward()
        {
            if (wnd == null)
                return;

            //是否有首次奖励
            bool haveFirstReward = NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.JOIN_GUILD_TIMES) == 0;
            ALUGUICommon.setGameObjEnable(wnd.goHaveFirstJoinRewardShowList, haveFirstReward);
            ALUGUICommon.setGameObjEnable(wnd.goHaveFirstJoinRewardHideList, !haveFirstReward);

            if (haveFirstReward && _m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(GRefdataCoreMgr.instance.npGeneral.guild_first_time_join_reward_list);
            }
        }

        //刷新加入联盟CD
        private void _refreshCD()
        {
            //获取倒计时
            long cdTimeSec = TimeUtil.msToSecCeiling(NPPlayer.instance.guildComp.joinGuildCdEndTimeMs - FpsAndPingMgr.instance.serverTimeTag);

            if (_m_wJoinCountDownWnd != null)
            {
                _m_wJoinCountDownWnd.showWnd();
                _m_wJoinCountDownWnd.setInfo(cdTimeSec, null);
            }
        }

        #region 点击事件
        
        //点击快速加入按钮
        private void _onClickQuickJoin(GameObject _go)
        {
            //加入联盟的倒计时
            long cdTimeMs = NPPlayer.instance.guildComp.joinGuildCdEndTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            //如果还在倒计时，上浮提示
            if (cdTimeMs > 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_applymentCding_tip,TimeUtil.millisecondsToTime_Two(cdTimeMs)));
                return;
            }

            //请求随机加入联盟
            NPPlayer.instance.guildComp.reqRandomJoinGuild(null);
        }

        //点击创建联盟按钮
        private void _onClickCreate(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeGuildCreate());
        }

        //点击更多联盟按钮
        private void _onClickMore(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(new GGUIWndGuildRank(4909, true, true), UINodeTagConst_Guild.C_GUILD_RANK, 0);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPLY);
        }

        #endregion
    }
}