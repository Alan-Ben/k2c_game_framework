using ALPackage;
using UnityEngine;

namespace GOE
{
    
    public enum EEarningGoalGlobalPageType
    {
        GLOBAL,
        HONOR,
    }
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndEarningGoalGlobal : _ATALBasicUIWnd<GGUIMonoEarningGoalGlobal>
    {
        private static GGUIWndEarningGoalGlobal _g_instance = new GGUIWndEarningGoalGlobal();
    
        public static GGUIWndEarningGoalGlobal instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndEarningGoalGlobal();
                return _g_instance;
            }
        }
        private EEarningGoalGlobalPageType _m_pageType = EEarningGoalGlobalPageType.GLOBAL;
        private NPGGUIWndCommonTab _m_tabGlobal;
        private NPGGUIWndCommonTab _m_tabHonor;
        private GGUIWndEarningGoalGlobalItemGrid _m_globalItemGrid;
        private GGUIWndEarningGoalHonorItemContainer _m_honorItemContainer;
        
        public GGUIWndEarningGoalGlobal() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoEarningGoalGlobal.assetPath; }
        protected override string _monoObjName { get => GGUIMonoEarningGoalGlobal.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_SELF_REACH_CHG, _refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_DRAW_CHG, _refreshHonorPage);
            WinMsg.RegisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_SELF_DRAW_CHG, _refreshGlobalPage);

        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_SELF_REACH_CHG, _refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_HONOR_DRAW_CHG, _refreshHonorPage);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EARNINGS_GOAL_SELF_DRAW_CHG, _refreshGlobalPage);
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            _m_tabGlobal?.discard();
            _m_tabGlobal = null;
        
            _m_tabHonor?.discard();
            _m_tabHonor = null;

            if (wnd != null) ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (null != wnd.tabGlobal)
            {
                _m_tabGlobal = new NPGGUIWndCommonTab(wnd.tabGlobal);
                _m_tabGlobal.clickDelegate += _clickGlobal;
            }
            if (null != wnd.tabHonor)
            {
                _m_tabHonor = new NPGGUIWndCommonTab(wnd.tabHonor);
                _m_tabHonor.clickDelegate += _clickHonor;
            }
            if (null != wnd.globalItemGrid)
            {
                _m_globalItemGrid = new GGUIWndEarningGoalGlobalItemGrid(wnd.globalItemGrid);
            }
            if (null != wnd.honorItemContainer)
            {
                _m_honorItemContainer = new GGUIWndEarningGoalHonorItemContainer(wnd.honorItemContainer);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            switch (_m_pageType)
            {
                case EEarningGoalGlobalPageType.GLOBAL:
                    _m_tabGlobal?.setSelected(true);
                    _m_tabHonor?.setSelected(false);
                    if (_m_globalItemGrid != null)
                    {
                        _m_globalItemGrid.showWnd();
                        _m_globalItemGrid.showItemList(GRefdataCoreMgr.instance.earningGoalRewardRefCore?.refList);
                    }
                    if (_m_honorItemContainer != null)
                    {
                        _m_honorItemContainer.hideWnd();
                    }

                    break;
                case EEarningGoalGlobalPageType.HONOR:
                    _m_tabGlobal?.setSelected(false);
                    _m_tabHonor?.setSelected(true);
                    if (_m_globalItemGrid != null)
                    {
                        _m_globalItemGrid.hideWnd();
                    }
                    if (_m_honorItemContainer != null)
                    {
                        _m_honorItemContainer.showWnd();
                        _m_honorItemContainer.showItemList(GRefdataCoreMgr.instance.earningGoalHonorRewardRefCore?.refList);
                    }
                    break;
            }
        }
        
        private void _clickGlobal(bool obj)
        {
            _m_pageType = EEarningGoalGlobalPageType.GLOBAL;
            _refreshWnd();
        }
        private void _clickHonor(bool obj)
        {
            _m_pageType = EEarningGoalGlobalPageType.HONOR;
            _refreshWnd();
        }

        private void _refreshGlobalPage()
        {
            if (_m_pageType == EEarningGoalGlobalPageType.GLOBAL && _m_globalItemGrid != null)
            {
                _m_globalItemGrid.showWnd();
                _m_globalItemGrid.showItemList(GRefdataCoreMgr.instance.earningGoalRewardRefCore?.refList);
            }
        }
        
        private void _refreshHonorPage()
        {
            if (_m_pageType == EEarningGoalGlobalPageType.HONOR && _m_honorItemContainer != null)
            {
                _m_honorItemContainer.showWnd();
                _m_honorItemContainer.showItemList(GRefdataCoreMgr.instance.earningGoalHonorRewardRefCore?.refList);
            }
        }
        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EARNING_GOAL_GLOBAL_REWARD);
        }
    }
}