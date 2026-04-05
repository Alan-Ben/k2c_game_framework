using UnityEngine;

namespace GOE
{
    public class GGUIWndUnLockConsortDetailInteractionStoryPage : _AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd<GGUIMonoUnlockConsortDetailInteractionStoryPage>
    {
        private GGUIWndConsortStoryGrid _m_wndConsortStoryGrid;
        
        public GGUIWndUnLockConsortDetailInteractionStoryPage(_IGGUIWndUnlockConsortDetailInteractionPageParam _pageParam, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_pageParam, _commonAssetPathInfo, _parent)
        {
        }

        public override EUnlockConsortDetailWndInteractionPageTabType tabPageType
        {
            get
            {
                return EUnlockConsortDetailWndInteractionPageTabType.STORY;
            }
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.storyGrid != null)
                _m_wndConsortStoryGrid = new GGUIWndConsortStoryGrid(wnd.storyGrid);
        }

        protected override void _onDiscardSub()
        {
            if(_m_wndConsortStoryGrid != null)
                _m_wndConsortStoryGrid.discard();
            _m_wndConsortStoryGrid = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wndConsortStoryGrid?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wndConsortStoryGrid?.resetWnd();
        }
        
        protected override void _setDataSub()
        {
            
        }
        
        protected override void _refreshWndSub()
        {
            if (_m_wndConsortStoryGrid != null && _m_iConsortInfo != null && _m_iConsortInfo.consortRefObj != null)
            {
                _m_wndConsortStoryGrid.showWnd();
                _m_wndConsortStoryGrid.setData(_m_iConsortInfo.consortRefObj.getNeedShowConsortStoryRefObjDic(), _m_iConsortInfo, null);
            }
        }
    }
}