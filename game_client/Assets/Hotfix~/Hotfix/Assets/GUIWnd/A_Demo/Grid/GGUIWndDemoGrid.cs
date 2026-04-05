using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 范例grid窗口
    /// </summary>
    public class GGUIWndDemoGrid : _AHotfixBaseShowAnimGridWnd<GGUIMonoDemoGrid, GGUIWndDemoGridItem>
    {
        private List<int> _m_lNumList;

        public GGUIWndDemoGrid(GGUIHotfixGridMono _wnd) : base(_wnd)
        {
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

        protected override GGUIWndDemoGridItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndDemoGridItem(_itemMono);
        }

        protected override void _onRefreshItemWnd(GGUIWndDemoGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_lNumList == null || _itemIdx < 0 || _itemIdx >= _m_lNumList.Count)
                return;

            _itemMono.setInfo(_m_lNumList[_itemIdx]);
        }

        protected override void _onWndInitDoneHotfix()
        {
        }

        public void showItemList(List<int> _list)
        {
            if (_list == null)
                return;

            _m_lNumList = _list;
            setItemCount(_m_lNumList.Count);
            forceRefreshAllItem();
        }
    }
}