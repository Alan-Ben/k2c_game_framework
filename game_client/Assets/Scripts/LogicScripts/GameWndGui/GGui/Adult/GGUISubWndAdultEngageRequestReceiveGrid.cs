using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestReceiveGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAdultEngageRequestReceiveGridItem, GGUIMonoAdultEngageRequestReceiveGrid, GGUISubWndAdultEngageRequestReceiveGridItem>
    {
        private List<AdultEngageRequestInfo> _m_requestList;
        
        
        public GGUISubWndAdultEngageRequestReceiveGrid(GGUIMonoAdultEngageRequestReceiveGrid _wnd) 
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
        protected override GGUISubWndAdultEngageRequestReceiveGridItem _createItemWnd(GGUIMonoAdultEngageRequestReceiveGridItem _itemMono)
        {
            return new GGUISubWndAdultEngageRequestReceiveGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndAdultEngageRequestReceiveGridItem _itemMono, int _itemIdx)
        {
            if (_m_requestList == null || _itemMono == null || _itemIdx < 0 || _itemIdx >= _m_requestList.Count)
                return;

            _itemMono.refreshWnd(_m_requestList[_itemIdx]);
        }


        public void refreshWnd(List<AdultEngageRequestInfo> _requestList)
        {
            _m_requestList = _requestList;
            setItemCount(_m_requestList?.Count ?? 0);
            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _requestList == null || _requestList.Count <= 0);
        }
    }
}