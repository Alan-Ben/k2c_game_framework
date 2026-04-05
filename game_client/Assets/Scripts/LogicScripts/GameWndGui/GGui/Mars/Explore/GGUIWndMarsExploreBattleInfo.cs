using ALPackage;
using GC2GS.p041_MarsExploreOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreBattleInfo : _ANPGGUIBasicWnd<GGUIMonoMarsExploreBattleInfo>, _IMarsExploreTeamSelectDealer
    {
        [NotNull] public static GGUIWndMarsExploreBattleInfo instance { get { return _g_instance ??= new GGUIWndMarsExploreBattleInfo(); } }
        private static GGUIWndMarsExploreBattleInfo _g_instance;

        private NPGGuiWndTexture _m_bannerWnd;
        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndCommonItemContainer _m_rewardContainerWnd;
        private MarsExploreBattleEventInfo _m_eventInfo;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreBattleInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreBattleInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public long targetPower { get { return _m_eventInfo?.getPower() ?? 0; } }


        public GGUIWndMarsExploreBattleInfo() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_bannerWnd?.showWnd();
            _m_iconWnd?.showWnd();
            _m_rewardContainerWnd?.showWnd();
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_BATTLE_GO_EXPLORE, _onSimulateClickGoExplore);
        }
        protected override void _onHideWnd()
        {
            _m_bannerWnd?.hideWnd();
            _m_iconWnd?.hideWnd();
            _m_rewardContainerWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_BATTLE_GO_EXPLORE, _onSimulateClickGoExplore);
        }
        protected override void _onReset()
        {
            _m_bannerWnd?.discardTexture();
            _m_iconWnd?.discardTexture();
            _m_rewardContainerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGoExplore, _onClickGoExplore);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

            _m_bannerWnd?.discard();
            _m_bannerWnd = null;

            _m_iconWnd?.discard();
            _m_iconWnd = null;

            _m_rewardContainerWnd?.discard();
            _m_rewardContainerWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnGoExplore, _onClickGoExplore);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

            if (wnd.imgBanner != null)
                _m_bannerWnd = new NPGGuiWndTexture(wnd.imgBanner);

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoRewardContainer != null)
                _m_rewardContainerWnd = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
        }
        
        
        public void refreshWnd(MarsExploreBattleEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_eventInfo == null)
                return;
        
            _m_bannerWnd?.setTexture(_m_eventInfo.typeRefObj.banner);
            _m_iconWnd?.setTexture(_m_eventInfo.typeRefObj.icon);
            _m_rewardContainerWnd?.showItemList(_m_eventInfo.getRewardItems());
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_eventInfo.typeRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_eventInfo.exploreLvl));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_eventInfo.typeRefObj.desc));
            ALUGUICommon.setLabelTxt(wnd.txtTypeDesc, TextTranslate.instance.getLanguage(_m_eventInfo.typeRefObj.type_desc));
            ALUGUICommon.setLabelTxt(wnd.txtDistance, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreDistance_num, _m_eventInfo.posRefObj?.distance ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtTimeTakes, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreTimeTake_num, TimeUtil.millisecondsToTime_DayHourOrHMS(_m_eventInfo.posRefObj?.march_time * 1000 ?? 0)));
            ALUGUICommon.setLabelTxt(wnd.txtPower, _m_eventInfo.getPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            if (wnd.anim != null)
                wnd.anim.Sample(wnd.teamSelectCancelAnimName, 1f);
            
            //  二次确认（有模糊）弹窗打开会导致 本窗口移动到最前面，GGUIWndMarsExploreTeamSelect这个窗口到后面去了，不好优化模糊逻辑，临时这么处理
            // 【优化-0】火星探索-当目标对象战力大于玩家派遣队伍时，需要提示玩家是否派遣。如：敌方战力值高于你，是否确认派遣队伍？ https://www.teambition.com/task/695e7f7e90ace14bfe464d72
            if (_m_isSelectTeamShow && GGUIWndMarsExploreTeamSelect.instance.isShow)
            {
                //将窗口移到最前
                GCommon.moveTransformToLastAndRefreshLayer(GGUIWndMarsExploreTeamSelect.instance.getGameObj());
                if (wnd.anim != null) wnd.anim.Sample(wnd.teamSelectStartAnimName, 1f);
            }
        }
        
        
        private void _onClickGoExplore(GameObject _go)
        {
            if (_m_eventInfo == null)
                return;

            // 判断是否已经有队伍在前往该事件地点
            if (NPPlayer.instance.marsComp.exploreSubComponent.isTeamMarchTarget(_m_eventInfo.instanceId))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreAlreadySendingTeamTip_none);
                return;
            }
            
            GGUIWndMarsExploreTeamSelect.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamSelect.instance, GGUIWndMarsExploreTeamSelect.instance.showWnd, EUIQueueStageType.MAIN,
                UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT, false, false);
        }
        private void _onSimulateClickGoExplore()
        {
            if (wnd == null) return;
            _onClickGoExplore(wnd.btnGoExplore);
        }
        private void _onClickClose(GameObject _go)
        {
            closeWnd();
        }

        /// <summary>
        /// 选择完成队伍之后的处理
        /// </summary>
        public void dealSelectTeam(long _teamId)
        {
            //发送请求派遣队伍
            NPGSClientListener.sendMsgByLog(new GC2GS_041_005_ReqStartDealExploreEvent(_teamId, _m_eventInfo.instanceId));
        }
        private bool _m_isSelectTeamShow = false;

        public void onTeamSelectWndShow()
        {
            if (wnd == null || wnd.anim == null)
                return;
            _m_isSelectTeamShow = true;
            wnd.anim.ForcePlay(wnd.teamSelectStartAnimName);
        }
        public void onTeamSelectWndHide()
        {
            if (wnd == null || wnd.anim == null)
                return;
            _m_isSelectTeamShow = false;
            wnd.anim.ForcePlay(wnd.teamSelectCancelAnimName);
        }
        public void closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_BATTLE_INFO);
        }
    }
}
