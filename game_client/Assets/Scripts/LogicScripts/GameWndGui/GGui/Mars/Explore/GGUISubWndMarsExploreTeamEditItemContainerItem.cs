using System;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamEditItemContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsExploreTeamEditItemContainerItem>
    {
        private GGUISubWndMarsExploreTeamHeroContainer _m_subWndHeroContainer;
        private MarsExploreTeamInfo _m_teamInfo;
        private new bool _m_bIsShow;
        private int _m_iItemIndex;


        public GGUISubWndMarsExploreTeamEditItemContainerItem(GGUIMonoMarsExploreTeamEditItemContainerItem _wnd)
            : base(_wnd)
        {
            _m_iItemIndex = -1;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_subWndHeroContainer?.showWnd();
            
            refreshWnd();

            _tryAddEventListeners();
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX, _onSimulateClickEdit);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshState);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_EDIT_ITEM_EDIT_BY_INDEX, _onSimulateClickEdit);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshState);
            
            _tryRemoveEventListeners();
            
            _m_subWndHeroContainer?.hideWnd();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_subWndHeroContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEdit, _onClickEdit);
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onClickBack);
            ALUGUICommon.uncombineBtnClick(wnd.btnRepair, _onClickRepair);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnlockJump, _onClickUnlockJump);
            ALUGUICommon.uncombineBtnClick(wnd.btnHelp, _onClickHelp);

            _m_subWndHeroContainer?.discard();
            _m_subWndHeroContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnEdit, _onClickEdit);
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onClickBack);
            ALUGUICommon.combineBtnClick(wnd.btnRepair, _onClickRepair);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onClickSpeedUp);
            ALUGUICommon.combineBtnClick(wnd.btnUnlockJump, _onClickUnlockJump);
            ALUGUICommon.combineBtnClick(wnd.btnHelp, _onClickHelp);

            if (wnd.monoHeroContainer != null)
                _m_subWndHeroContainer = new GGUISubWndMarsExploreTeamHeroContainer(wnd.monoHeroContainer, null);
        }

        public void setItemIndex(int _index)
        {
            _m_iItemIndex = _index;
        }

        public void refreshWnd(MarsExploreTeamInfo _teamInfo)
        {
            _tryRemoveEventListeners();
            _m_teamInfo = _teamInfo;
            _tryAddEventListeners();
            
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_teamInfo.teamId);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockCondition, _m_teamInfo.getUnlockConditionDesc());
            refreshTeamName();
            refreshSoldierNum();
            refreshRemainTime();
            refreshHeroData();
            refreshState();
        }
        public void refreshState()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            wnd.setState(_m_teamInfo.getUIState());
        }
        public void refreshSoldierNum()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtSoldierNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, 
                _m_teamInfo.soldierNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), 
                _m_teamInfo.soldierMax.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtSoldierLossNum, _m_teamInfo.soldierLoss.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
        public void refreshTeamName()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_teamInfo.name);
        }
        public void refreshHeroData()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;
            
            _m_subWndHeroContainer?.refreshWnd(_m_teamInfo);
            ALUGUICommon.setLabelTxt(wnd.txtPowerAddPercent, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _m_teamInfo.getPowerAddPer() / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtTeamPower, _m_teamInfo.getTeamPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
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


        private void _onSimulateClickEdit(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] is not long targetIndex)
                return;

            if (targetIndex == _m_iItemIndex)
                _onClickEdit(null);
        }

        private void _onClickEdit(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            GGUIWndMarsExploreTeamHeroEdit.instance.refreshWnd(_m_teamInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamHeroEdit.instance, GGUIWndMarsExploreTeamHeroEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_HERO_EDIT);
        }
        private void _onClickBack(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            NPPlayer.instance.marsComp.exploreSubComponent.reqTeamBack(_m_teamInfo.teamId, null);
        }
        private void _onClickRepair(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            GGUIWndMarsExploreTeamRepair.instance.refreshWnd(_m_teamInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamRepair.instance, GGUIWndMarsExploreTeamRepair.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_REPAIR);
        }
        private void _onClickSpeedUp(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            GGUIWndMarsTimeSpeedUp.addNode(_m_teamInfo, _m_teamInfo);
        }
        private void _onClickUnlockJump(GameObject _go)
        {
            _m_teamInfo?.refObj?.unlock_jump?.dealEffect();
        }
        private void _onClickHelp(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;

            bool canAskHelp = NPPlayer.instance.guildMarsHelpComp.canAskGuildHelp(EGuildMarsHelpObjType.TEAM_REPAIR, _m_teamInfo.teamId);
            if (!canAskHelp) return;
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.TEAM_REPAIR, _m_teamInfo.teamId,
                _suc =>
                {
                    refreshState();
                });
        }
        private void _tryAddEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;

            _m_teamInfo.onNameChg += refreshTeamName;
            _m_teamInfo.onSoldierNumChg += refreshSoldierNum;
            _m_teamInfo.onHeroListChg += refreshHeroData;
            _m_teamInfo.onUIStateChg += refreshState;
            _m_teamInfo.onTeamPowerChg += refreshHeroData;
        }
        private void _tryRemoveEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;

            _m_teamInfo.onNameChg -= refreshTeamName;
            _m_teamInfo.onSoldierNumChg -= refreshSoldierNum;
            _m_teamInfo.onHeroListChg -= refreshHeroData;
            _m_teamInfo.onUIStateChg -= refreshState;
            _m_teamInfo.onTeamPowerChg -= refreshHeroData;
        }
    }
}
