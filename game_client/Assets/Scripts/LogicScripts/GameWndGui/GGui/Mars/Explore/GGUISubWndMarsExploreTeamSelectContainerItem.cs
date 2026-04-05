using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;
using System;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamSelectContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsExploreTeamSelectContainerItem>
    {
        private NPGGuiWndTexture _m_firstHeroAvatarWnd;

        private _IMarsExploreTeamSelectDealer _m_target;
        private MarsExploreTeamInfo _m_teamInfo;

        private new bool _m_bIsShow;


        public GGUISubWndMarsExploreTeamSelectContainerItem(GGUIMonoMarsExploreTeamSelectContainerItem _wnd)
            : base(_wnd)
        {            
            initWnd();
        }


        public MarsExploreTeamInfo teamInfo { get { return _m_teamInfo; } }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_firstHeroAvatarWnd?.showWnd();
            
            refreshWnd();
            
            _tryAddEventListeners();
        }
        protected override void _onHideWnd()
        {
            _tryRemoveEventListeners();
            
            _m_firstHeroAvatarWnd?.hideWnd();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_firstHeroAvatarWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnEdit, _onClickEdit);
            ALUGUICommon.uncombineBtnClick(wnd.btnRepair, _onClickRepair);

            _m_firstHeroAvatarWnd?.discard();
            _m_firstHeroAvatarWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnEdit, _onClickEdit);
            ALUGUICommon.combineBtnClick(wnd.btnRepair, _onClickRepair);

            if (wnd.imgFirstHero != null)
                _m_firstHeroAvatarWnd = new NPGGuiWndTexture(wnd.imgFirstHero);
        }


        public void refreshWnd(_IMarsExploreTeamSelectDealer _target, MarsExploreTeamInfo _teamInfo)
        {
            _tryRemoveEventListeners();
            _m_target = _target;
            _m_teamInfo = _teamInfo;
            _tryAddEventListeners();

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
            refreshRemainTime();
        }
        public void refreshHeroData()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            HeroInfo heroInfo = _m_teamInfo.getHeroByIndex(0);
            _m_firstHeroAvatarWnd?.setTexture(heroInfo?.getIcon());
            long myTeamPower = _m_teamInfo.getTeamPower();
            long enemyTeamPower = _m_target?.targetPower ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtPower, myTeamPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            wnd.setPowerShow((float) myTeamPower / enemyTeamPower);
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
        public void refreshUIState()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            wnd.setState(_m_teamInfo.getUIState());
        }

        public void refreshRemainTime()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.stateTimeRemain, TimeUtil.millisecondsToTime_DayHourOrHMS(_m_teamInfo.stateRemainTimeMs));

            if (wnd.sldStateProgress != null)
            {
                wnd.sldStateProgress.minValue = 0;
                wnd.sldStateProgress.maxValue = Math.Max(0, _m_teamInfo.stateEndTime - _m_teamInfo.stateStartTime);
                wnd.sldStateProgress.value = FpsAndPingMgr.instance.serverTimeTag - _m_teamInfo.stateStartTime;
            }
        }


        internal void _simulateClickConfirm()
        {
            _onClickConfirm(null);
        }
        private void _onClickConfirm(GameObject _go)
        {
            if (wnd == null || _m_teamInfo == null || _m_target == null)
                return;

            void sendTeam()
            {
                //直接关闭窗口，发送请求
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT);
                _m_target.closeWnd();

                //处理选择操作
                _m_target.dealSelectTeam(_m_teamInfo.teamId);
            }

            long myTeamPower = _m_teamInfo.getTeamPower();
            long enemyPower = _m_target.targetPower;
            float powerRatio = (float) myTeamPower / enemyPower;
            if (powerRatio > wnd.safeThreshold)
                sendTeam();
            else if (powerRatio > wnd.highRiskyThreshold)
            {
                NPMesMgr.instance.showWarningTipMes(sendTeam, null, ENPWarningType.MARS_EXPLORE_ATTACK_CONFIRM,
                    TransKeyConst.sys_tip_title_none,
                    TransKeyConst.mars_explore_attackDangerousTipContent_none);
            }
            else
            {
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.mars_explore_attackHighRiskyTipContent_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    sendTeam);
            }
        }
        private void _onClickEdit(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;

            GGUIWndMarsExploreTeamHeroEdit.instance.refreshWnd(_m_teamInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamHeroEdit.instance, GGUIWndMarsExploreTeamHeroEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_HERO_EDIT);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT);
        }
        private void _onClickRepair(GameObject _)
        {
            if (_m_teamInfo == null)
                return;

            GGUIWndMarsExploreTeamRepair.instance.refreshWnd(_m_teamInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamRepair.instance, GGUIWndMarsExploreTeamRepair.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_REPAIR);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT);
        }
        private void _tryAddEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;

            _m_teamInfo.onHeroListChg += refreshHeroData;
            _m_teamInfo.onUIStateChg += refreshUIState;
            _m_teamInfo.onSoldierNumChg += refreshSoldierData;
        }
        private void _tryRemoveEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;
            
            _m_teamInfo.onHeroListChg -= refreshHeroData;
            _m_teamInfo.onUIStateChg -= refreshUIState;
            _m_teamInfo.onSoldierNumChg -= refreshSoldierData;
        }
    }
}
