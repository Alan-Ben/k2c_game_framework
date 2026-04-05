using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamMiniInfoContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsExploreTeamMiniInfoContainerItem>
    {
        [CanBeNull] private MarsExploreTeamInfo _m_teamInfo;
        private NPGGuiWndTexture _m_wndFirstHeroTexture;
        private GGuiWndSprite _m_wndQualityHeadBg;
        private new bool _m_bIsShow;


        public GGUISubWndMarsExploreTeamMiniInfoContainerItem(GGUIMonoMarsExploreTeamMiniInfoContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_wndFirstHeroTexture?.showWnd();
            _m_wndQualityHeadBg?.showWnd();
            refreshWnd();
            _tryAddEventListener();
        }
        protected override void _onHideWnd()
        {
            _tryRemoveEventListener();
            _m_wndFirstHeroTexture?.hideWnd();
            _m_wndQualityHeadBg?.hideWnd();
            
            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_wndFirstHeroTexture?.discardTexture();
            _m_wndQualityHeadBg?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);

            _m_wndFirstHeroTexture?.discard();
            _m_wndFirstHeroTexture = null;

            _m_wndQualityHeadBg?.discard();
            _m_wndQualityHeadBg = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);

            if (wnd.imgFirstHero != null)
                _m_wndFirstHeroTexture = new NPGGuiWndTexture(wnd.imgFirstHero);

            if (wnd.imgQualityHeadBg != null)
                _m_wndQualityHeadBg = new GGuiWndSprite(wnd.imgQualityHeadBg);
        }


        public void refreshWnd([CanBeNull] MarsExploreTeamInfo _teamInfo)
        {
            _tryRemoveEventListener();
            _m_teamInfo = _teamInfo;
            _tryAddEventListener();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_teamInfo.teamId);

            refreshHeroData();
            refreshSoldierData();
            refreshUIState();
        }
        public void refreshUIState()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            EMarsExploreTeamUIState uiState = _m_teamInfo.getUIState();
            wnd.setState(uiState);
            
            ALUGUICommon.setLabelTxt(wnd.stateTimeRemain, TimeUtil.millisecondsToTime_DayHourOrHMS(_m_teamInfo.stateRemainTimeMs));

            if (wnd.sldStateProgress != null)
            {
                wnd.sldStateProgress.minValue = 0;
                wnd.sldStateProgress.maxValue = Math.Max(0, _m_teamInfo.stateEndTime - _m_teamInfo.stateStartTime);
                wnd.sldStateProgress.value = FpsAndPingMgr.instance.serverTimeTag - _m_teamInfo.stateStartTime;
            }
        }
        public void refreshHeroData() 
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            HeroInfo heroInfo = _m_teamInfo.getHeroByIndex(0);
            _m_wndFirstHeroTexture?.setTexture(heroInfo?.getIcon());

            if (heroInfo != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, heroInfo.id);
                _m_wndQualityHeadBg?.setTexture(qualityExtRef?.hero_head_bg);
            }
        }
        public void refreshSoldierData()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            long soldierNum = _m_teamInfo.soldierNum;
            long soldierMax = _m_teamInfo.soldierMax;
            if (wnd.sldSoldierHp != null)
            {
                wnd.sldSoldierHp.minValue = 0;
                wnd.sldSoldierHp.maxValue = soldierMax;
                wnd.sldSoldierHp.value = soldierNum;
            }
            float soldierHpPercent = soldierMax > 0 ? (float)soldierNum / soldierMax : 0f;
            wnd.setSoldierHpPercent(soldierHpPercent);
        }
        public void tick()
        {
            refreshUIState();
        }


        private void _onClickItem(GameObject _go)
        {
            if (wnd == null || _m_teamInfo == null)
                return;

            EMarsExploreTeamUIState state = _m_teamInfo.getUIState();
            switch (state)
            {
                case EMarsExploreTeamUIState.Exploring:
                case EMarsExploreTeamUIState.Back:
                    MarsExploreViewMgr.instance?.focusOnTeam(_m_teamInfo.teamId);
                    break;
                case EMarsExploreTeamUIState.Collecting:
                //     MarsExploreEventCollectInfo eventData = NPPlayer.instance.marsComp.exploreSubComponent.getEventByPosId(_m_teamInfo.statePosId) as MarsExploreEventCollectInfo;
                //     if (eventData == null)
                //         return;
                //     
                //     GGUIWndMarsExploreCollectingDetail.instance.refreshWnd(eventData);
                //     QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreCollectingDetail.instance, GGUIWndMarsExploreCollectingDetail.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_COLLECTING_DETAIL);
                    break;
                case EMarsExploreTeamUIState.Idle:
                case EMarsExploreTeamUIState.IdleSoldierLoss:
                case EMarsExploreTeamUIState.Repairing:
                case EMarsExploreTeamUIState.CanAskHelp:
                case EMarsExploreTeamUIState.Empty:
                case EMarsExploreTeamUIState.Lock:
                    long soldierNum = _m_teamInfo.soldierNum;
                    long soldierMax = _m_teamInfo.soldierMax;
                    float soldierHpPercent = soldierMax > 0 ? (float)soldierNum / soldierMax : 0f;
                    if (soldierHpPercent < wnd.unhealthyPercentThreshold)
                    {
                        GGUIWndMarsExploreTeamRepair.instance.refreshWnd(_m_teamInfo);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamRepair.instance, GGUIWndMarsExploreTeamRepair.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_REPAIR);
                        return;
                    }
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamEdit.instance, GGUIWndMarsExploreTeamEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_EDIT);
                    break;
            }
        }
        private void _tryAddEventListener()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            _m_teamInfo.onUIStateChg += refreshUIState;
            _m_teamInfo.onHeroListChg += refreshHeroData;
            _m_teamInfo.onSoldierNumChg += refreshSoldierData;
        }
        private void _tryRemoveEventListener()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            _m_teamInfo.onUIStateChg -= refreshUIState;
            _m_teamInfo.onHeroListChg -= refreshHeroData;
            _m_teamInfo.onSoldierNumChg -= refreshSoldierData;
        }
    }
}
