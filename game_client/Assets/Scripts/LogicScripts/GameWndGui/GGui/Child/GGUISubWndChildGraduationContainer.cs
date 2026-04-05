using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndChildGraduationContainer : _AGGUISubWndCommonContainer<GGUIMonoChildGraduationContainerItem, GGUIMonoChildGraduationContainer, GGUISubWndChildGraduationContainerItem>
    {
        private List<_IChildInfo> _m_childList;
        private List<_IItem> _m_presentList;
        
        
        public GGUISubWndChildGraduationContainer(GGUIMonoChildGraduationContainer _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override GGUISubWndChildGraduationContainerItem _createItemWnd(GGUIMonoChildGraduationContainerItem _itemMono)
        {
            return new GGUISubWndChildGraduationContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndChildGraduationContainerItem _itemWnd, int _index)
        {
            if (_m_childList == null)
                return;

            _itemWnd.refreshWnd(_m_childList.SafeGet(_index), _m_presentList.SafeGet(_index));
        }


        public void refreshWnd(List<_IChildInfo> _childList, List<_IItem> _presentList)
        {
            _m_childList = _childList;
            _m_presentList = _presentList;
            refreshWnd(_m_childList?.Count ?? 0);
        }


        protected override void _onRefreshWnd()
        {
            base._onRefreshWnd();
            wnd.scrollRect.verticalNormalizedPosition = 1;
        }
    }
}