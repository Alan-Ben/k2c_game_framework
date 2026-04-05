using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndMarsPosItemSelectContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsPosItemSelectContainerItem, GGUIMonoMarsPosItemSelectContainer, GGUISubWndMarsPosItemSelectContainerItem>
    {
        private List<_IMarsExplorePosItem> _m_itemList;


        public GGUISubWndMarsPosItemSelectContainer(GGUIMonoMarsPosItemSelectContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUISubWndMarsPosItemSelectContainerItem _createItemWnd(GGUIMonoMarsPosItemSelectContainerItem _itemMono)
        {
            return new GGUISubWndMarsPosItemSelectContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsPosItemSelectContainerItem _itemWnd, int _index)
        {
            _IMarsExplorePosItem posItem = _m_itemList.SafeGet(_index);
            _itemWnd.refreshWnd(posItem);
        }


        public void refreshWnd(List<_IMarsExplorePosItem> _list)
        {
            _m_itemList = _list;
            refreshWnd(_m_itemList?.Count ?? 0);
        }
    }
}
