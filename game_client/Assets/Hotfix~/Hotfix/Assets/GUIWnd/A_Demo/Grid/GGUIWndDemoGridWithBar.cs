using System.Collections.Generic;
using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 范例grid窗口
    /// </summary>
    public class GGUIWndDemoGridWithBar : _AHotfixBaseShowAnimGridWnd<GGUIMonoDemoGrid, GGUIWndDemoGridItem>
    {
        private List<int> _m_lNumList;

        private GGUIWndDemoGridBarController _m_BarController1;
        private GGUIWndDemoGridBarController _m_BarController2;

        public GGUIWndDemoGridWithBar(GGUIHotfixGridMono _wnd) : base(_wnd)
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
            if (null != _m_BarController1)
            {
                removeBar(_m_BarController1);
                _m_BarController1.discard();
                _m_BarController1 = null;
            }

            if (null != _m_BarController2)
            {
                removeBar(_m_BarController2);
                _m_BarController2.discard();
                _m_BarController2 = null;
            }

            if (_m_lNumList != null)
                _m_lNumList.Clear();
            _m_lNumList = null;
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
            _addBar();
            forceRefreshAllItem();
        }


        private void _addBar()
        {
            if (wnd == null)
                return;

            if (_m_BarController1 == null)
            {
                _m_BarController1 = new GGUIWndDemoGridBarController(wnd.gridAreaUIObj);
                addBar(_m_BarController1);
            }
            _m_BarController1.setInsertIndex(1);
            _m_BarController1.setInfo("test bar");
            _m_BarController1.show();

            if (_m_BarController2 == null)
            {
                _m_BarController2 = new GGUIWndDemoGridBarController(wnd.gridAreaUIObj);
                addBar(_m_BarController2);
            }
            _m_BarController2.setInsertIndex(3);
            _m_BarController2.setInfo("test bar");
            _m_BarController2.show();
        }
    }
}