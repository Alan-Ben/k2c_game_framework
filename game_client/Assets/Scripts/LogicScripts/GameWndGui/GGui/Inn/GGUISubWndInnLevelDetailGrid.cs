using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndInnLevelDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoInnLevelDetailGridItem, GGUIMonoInnLevelDetailGrid, GGUISubWndInnLevelDetailGridItem>
    {
        private List<InnLevelRefObj> _m_itemList;


        public GGUISubWndInnLevelDetailGrid(GGUIMonoInnLevelDetailGrid _wnd)
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
        protected override GGUISubWndInnLevelDetailGridItem _createItemWnd(GGUIMonoInnLevelDetailGridItem _itemMono)
        {
            return new GGUISubWndInnLevelDetailGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndInnLevelDetailGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;

            InnLevelRefObj itemInfo = _m_itemList.SafeGet(_itemIdx);
            _itemMono.refreshWnd(itemInfo);
        }


        public void refreshWnd()
        {
            _m_itemList = GRefdataCoreMgr.instance.innLevelRefCore.refList;
            setItemCount(_m_itemList?.Count ?? 0);
        }
    }
}