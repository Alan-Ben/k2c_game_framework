using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndBusinessBuildingRnDPageProductGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoBusinessBuildingRnDPageProductGridItem, GGUIMonoBusinessBuildingRnDPageProductGrid, GGUISubWndBusinessBuildingRnDPageProductGridItem>
    {
        private List<BusinessBuildingProductRefObj> _m_productList;
        private BusinessBuildingInfo _m_buildingInfo;
        
        
        public GGUISubWndBusinessBuildingRnDPageProductGrid(GGUIMonoBusinessBuildingRnDPageProductGrid _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
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
        protected override GGUISubWndBusinessBuildingRnDPageProductGridItem _createItemWnd(GGUIMonoBusinessBuildingRnDPageProductGridItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingRnDPageProductGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndBusinessBuildingRnDPageProductGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;
            
            BusinessBuildingProductRefObj productRef = _m_productList.SafeGet(_itemIdx);
            if (productRef == null)
                return;
            
            _itemMono.refreshWnd(productRef, _m_buildingInfo);
        }

        public void refreshWnd(List<BusinessBuildingProductRefObj> _productList, BusinessBuildingInfo _buildingInfo)
        {
            _m_productList = _productList;
            _m_buildingInfo = _buildingInfo;
            setItemCount(_m_productList?.Count ?? 0);
        }
    }
}