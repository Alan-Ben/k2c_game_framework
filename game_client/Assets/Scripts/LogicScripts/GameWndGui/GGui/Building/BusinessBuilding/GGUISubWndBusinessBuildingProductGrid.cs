using ALPackage;

namespace GOE
{
    public class GGUISubWndBusinessBuildingProductGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoBusinessBuildingProductGridItem, GGUIMonoBusinessBuildingProductGrid, GGUISubWndBusinessBuildingProductGridItem>
    {
        private BusinessBuildingInfo _m_buildingInfo;
        private bool _m_needUpdate = false;
        
        
        public GGUISubWndBusinessBuildingProductGrid(GGUIMonoBusinessBuildingProductGrid _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_needUpdate = QueueMgr.instance._lastNode.nodeTag == UINodeTagConst.C_BUILDING_MAIN;
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }
        protected override GGUISubWndBusinessBuildingProductGridItem _createItemWnd(GGUIMonoBusinessBuildingProductGridItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingProductGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndBusinessBuildingProductGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;

            BusinessBuildingProductRefObj productRef = _m_buildingInfo?.productList.SafeGet(_itemIdx);
            _itemMono.refreshWnd(productRef);
        }
        

        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            setItemCount(_m_buildingInfo?.productList.Count ?? 0);
            refreshAllItem((_itemMono, _index) =>
            {
                BusinessBuildingProductRefObj productRef = _m_buildingInfo?.productList.SafeGet(_index);
                _itemMono?.refreshWnd(productRef);
            });
        }


        protected override void _onFrameRefresh()
        {
            base._onFrameRefresh();

            if (!_m_needUpdate)
                return;
            
            refreshAllItem((_itemMono, _) =>
            {
                _itemMono?.update();
            });
        }
        
        
        private void _onNodeChg()
        {
            _m_needUpdate = QueueMgr.instance._lastNode.nodeTag ==UINodeTagConst.C_BUILDING_MAIN;  
        }
    }
}