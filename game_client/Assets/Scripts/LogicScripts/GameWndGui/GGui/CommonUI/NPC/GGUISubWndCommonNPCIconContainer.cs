using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndCommonNPCIconContainer : _AGGUISubWndCommonContainer<GGUIMonoCommonNPCIconContainerItem, GGUIMonoCommonNPCIconContainer, GGUISubWndCommonNPCIconContainerItem>
    {
        private List<NPNPCRefObj> _m_npcList;
        
        public GGUISubWndCommonNPCIconContainer(GGUIMonoCommonNPCIconContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            refreshWnd();
        }

        protected override GGUISubWndCommonNPCIconContainerItem _createItemWnd(GGUIMonoCommonNPCIconContainerItem _itemMono)
        {
            return new GGUISubWndCommonNPCIconContainerItem(_itemMono);
        }

        protected override void _refreshItemWnd(GGUISubWndCommonNPCIconContainerItem _itemWnd, int _index)
        {
            _itemWnd.refreshWnd(_m_npcList?.SafeGet(_index));
        }
        
        public void refreshWnd(List<NPNPCRefObj> _npcList)
        {
            _m_npcList = _npcList;
            refreshWnd();
        }
        public new void refreshWnd()
        {
            if (_m_npcList == null)
                return;
            
            base.refreshWnd(_m_npcList.Count);
        }
    }
}