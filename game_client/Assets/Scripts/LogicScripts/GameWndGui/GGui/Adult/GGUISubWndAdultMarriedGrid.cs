using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndAdultMarriedGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAdultMarriedGridItem, GGUIMonoAdultMarriedGrid, GGUISubWndAdultMarriedGridItem>
    {
        private List<MarriedInfo> _m_marriedInfoList;
        
        
        public GGUISubWndAdultMarriedGrid(GGUIMonoAdultMarriedGrid _wnd) 
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
        protected override GGUISubWndAdultMarriedGridItem _createItemWnd(GGUIMonoAdultMarriedGridItem _itemMono)
        {
            return new GGUISubWndAdultMarriedGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndAdultMarriedGridItem _itemMono, int _itemIdx)
        {
            if (_m_marriedInfoList == null || _itemMono == null || _itemIdx < 0 || _itemIdx >= _m_marriedInfoList.Count)
                return;
            
            _itemMono.refreshWnd(_m_marriedInfoList[^(_itemIdx + 1)]);
        }

        public void refreshWnd(List<MarriedInfo> _marriedInfoList)
        {
            _m_marriedInfoList = _marriedInfoList;
            setItemCount(_m_marriedInfoList?.Count ?? 0);
        }
    }
}