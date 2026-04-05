using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;

namespace GOE
{
    /// <summary>
    /// 宴会宾客历史item容器
    /// </summary>
    public class GGUIWndDinnerGuestItemGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoDinnerGuestItem,GGUIMonoDinnerGuestItemGrid,GGUIWndDinnerGuestItem>
    {
        private List<GDinnerGuestInfo> _m_itemDataList;
        
        public GGUIWndDinnerGuestItemGrid(GGUIMonoDinnerGuestItemGrid _wnd) : base(_wnd)
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

        protected override GGUIWndDinnerGuestItem _createItemWnd(GGUIMonoDinnerGuestItem _itemMono)
        {
            GGUIWndDinnerGuestItem itemWnd = new GGUIWndDinnerGuestItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemwnd(GGUIWndDinnerGuestItem _itemMono, int _itemIdx)
        {
            if(_itemIdx <0 || _itemIdx >= _m_itemDataList.Count)
                return;
            
            _itemMono.setInfo(_m_itemDataList[_itemIdx], _itemIdx + 1);
        }
        
        public void showItemList(List<GDinnerGuestInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList = _itemDataList;
            setItemCount(_m_itemDataList.Count);
            ALUGUICommon.setGameObjEnable(wnd.emptyListHide,_m_itemDataList.Count != 0);
            ALUGUICommon.setGameObjEnable(wnd.emptyListShow,_m_itemDataList.Count == 0);
        }
    }
}
