using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMuseumItemListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMuseumItemListGridItem, GGUIMonoMuseumItemListGrid, GGUISubWndMuseumItemListGridItem>
    {
        [ItemNotNull, NotNull]private readonly List<MuseumItemInfo> _m_itemList;


        public GGUISubWndMuseumItemListGrid(GGUIMonoMuseumItemListGrid _wnd) 
            : base(_wnd)
        {
            _m_itemList = new List<MuseumItemInfo>();
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
        protected override GGUISubWndMuseumItemListGridItem _createItemWnd(GGUIMonoMuseumItemListGridItem _itemMono)
        {
            return new GGUISubWndMuseumItemListGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndMuseumItemListGridItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null)
                return;
            
            MuseumItemInfo itemInfo = _m_itemList.SafeGet(_itemIdx);            
            _itemWnd.refreshWnd(itemInfo, _m_itemList);
        }
        

        public void refreshWnd() 
        {
            NPPlayer.instance.museumComp.getItemListNonAlloc(_m_itemList);
            _m_itemList.Sort((_a, _b) =>
            {
                if (_a.isObtain && !_b.isObtain)
                    return -1;
                if (!_a.isObtain && _b.isObtain)
                    return 1;
                if (_a.isActive && !_b.isActive)
                    return 1;
                if (!_a.isActive && _b.isActive)
                    return -1;

                return _a.itemId.CompareTo(_b.itemId);
            });
            setItemCount(_m_itemList.Count);
        }
    }
}