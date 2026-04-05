using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndAdultEngageSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAdultEngageSelectGridItem, GGUIMonoAdultEngageSelectGrid, GGUISubWndAdultEngageSelectGridItem>
    {
        private AdultInfo _m_targetAdultInfo;
        private List<AdultInfo> _m_myAdultList;
        
        
        public GGUISubWndAdultEngageSelectGrid(GGUIMonoAdultEngageSelectGrid _wnd) 
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
        protected override GGUISubWndAdultEngageSelectGridItem _createItemWnd(GGUIMonoAdultEngageSelectGridItem _itemMono)
        {
            return new GGUISubWndAdultEngageSelectGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndAdultEngageSelectGridItem _itemMono, int _itemIdx)
        {
            if (_m_myAdultList == null || _itemMono == null || _itemIdx < 0 || _itemIdx >= _m_myAdultList.Count)
                return;
            
            _itemMono.refreshWnd(_m_targetAdultInfo, _m_myAdultList[_itemIdx]);
        }
        

        public void refreshWnd(AdultInfo _targetAdultInfo, List<AdultInfo> _myAdultList)
        {
            _m_targetAdultInfo = _targetAdultInfo;
            _m_myAdultList = _myAdultList;
            setItemCount(_m_myAdultList?.Count ?? 0);
        }
    }
}