using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreTeamHeroEdit : _ANPGGUIBasicWnd<GGUIMonoMarsExploreTeamHeroEdit>
    {
        [NotNull] public static GGUIWndMarsExploreTeamHeroEdit instance { get { return _g_instance ??= new GGUIWndMarsExploreTeamHeroEdit(); } }
        private static GGUIWndMarsExploreTeamHeroEdit _g_instance;
        

        private GGUISubWndMarsExploreTeamHeroContainer _m_subWndHeroContainer;
        private GGUISubWndMarsExploreTeamHeroSelectGrid _m_subWndHeroSelectGrid;
        
        private MarsExploreTeamInfo _m_teamInfo;

        private new bool _m_bIsShow;
        

        public GGUIWndMarsExploreTeamHeroEdit()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreTeamHeroEdit.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreTeamHeroEdit.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_subWndHeroContainer?.showWnd();
            _m_subWndHeroSelectGrid?.showWnd();
            
            refreshWnd();

            _tryAddEventListeners();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_EDIT_CONFIRM, _onSimulateClickConfirm);
        }
        protected override void _onHideWnd()
        {
            _tryRemoveEventListeners();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_EDIT_CONFIRM, _onSimulateClickConfirm);
            
            _m_subWndHeroContainer?.hideWnd();
            _m_subWndHeroSelectGrid?.hideWnd();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_subWndHeroContainer?.resetWnd();
            _m_subWndHeroSelectGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_subWndHeroContainer?.discard();
            _m_subWndHeroContainer = null;

            _m_subWndHeroSelectGrid?.discard();
            _m_subWndHeroSelectGrid = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnRename, _onClickRename);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroContainer != null)
                _m_subWndHeroContainer = new GGUISubWndMarsExploreTeamHeroContainer(wnd.monoHeroContainer, _onClickHeroInfo);

            if (wnd.monoHeroSelectGrid != null)
                _m_subWndHeroSelectGrid = new GGUISubWndMarsExploreTeamHeroSelectGrid(wnd.monoHeroSelectGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnRename, _onClickRename);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
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
            _m_subWndHeroSelectGrid?.refreshWnd(_m_teamInfo, _refreshHeroData);
            refreshName();
            _refreshHeroData(_m_subWndHeroSelectGrid.curSelectedHeroList);
        }
        public void refreshName()
        {
            if (wnd == null || !_m_bIsShow || _m_teamInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_teamInfo.name);
        }
        private void _refreshHeroData(List<HeroInfo> _heroList)
        {
            if (wnd == null || _heroList == null || _m_teamInfo == null)
                return;

            //刷新伙伴列表
            _m_subWndHeroContainer?.refreshWnd(_heroList);

            //重新计算士兵数量
            long soldierMax = MarsUtil.calculateMarsExploreTeamSoldierMax(_heroList);
            long soldierNum = Math.Max(0, soldierMax - _m_teamInfo.soldierLoss);
            //显示士兵数量
            ALUGUICommon.setLabelTxt(wnd.txtSoldierNum, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,
                soldierNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                soldierMax.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtSoldierLossNum, _m_teamInfo.soldierLoss.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            //实力加成百分比
            long teamPowerAddPer = MarsUtil.calculateMarsExploreTeamPowerAddPer(_heroList);
            ALUGUICommon.setLabelTxt(wnd.txtPowerAddPercent, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, teamPowerAddPer / 100f));

            //重新计算队伍实力
            long teamPower = MarsUtil.calculateMarsExploreTeamPower(soldierNum, teamPowerAddPer);
            ALUGUICommon.setLabelTxt(wnd.txtTeamPower, teamPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_HERO_EDIT);
        }
        private void _onClickRename(GameObject _go)
        {
            if (_m_teamInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeCommonRename(7312, true, _m_teamInfo.name, null, GRefdataCoreMgr.instance.npGeneral.mars_explore_team_name_range, null,
                _confirmName =>
                {
                    if (_m_teamInfo == null)
                    {
                        ALLog.Warning("GGUIWndMarsExploreTeamHeroEdit._onClickRename: team info is null.");
                        return;
                    }
                    
                    NPPlayer.instance.marsComp.exploreSubComponent.reqTeamRename(_m_teamInfo.teamId, _confirmName, () =>
                    {
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE);
                    });
                }, null, true));
        }
        private void _onClickConfirm(GameObject _go)
        {
            if (_m_teamInfo == null || _m_subWndHeroSelectGrid == null)
                return;
            
            List<long> heroIdList = new List<long>();
            foreach (HeroInfo heroInfo in _m_subWndHeroSelectGrid.curSelectedHeroList)
                heroIdList.Add(heroInfo.id);
            NPPlayer.instance.marsComp.exploreSubComponent.reqTeamHeroList(_m_teamInfo.teamId, heroIdList, () => _onClickClose(null));
        }
        private void _tryAddEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;

            _m_teamInfo.onNameChg += refreshName;
        }
        private void _tryRemoveEventListeners()
        {
            if (_m_teamInfo == null || !_m_bIsShow)
                return;

            _m_teamInfo.onNameChg -= refreshName;
        }

        private void _onClickHeroInfo(HeroInfo _heroInfo)
        {
            if (_m_subWndHeroSelectGrid != null) 
                _m_subWndHeroSelectGrid.clickItem(_heroInfo);
        }
        private void _onSimulateClickConfirm()
        {
            if (wnd == null) return;
            _onClickConfirm(wnd.btnConfirm);
        }
    }
}
