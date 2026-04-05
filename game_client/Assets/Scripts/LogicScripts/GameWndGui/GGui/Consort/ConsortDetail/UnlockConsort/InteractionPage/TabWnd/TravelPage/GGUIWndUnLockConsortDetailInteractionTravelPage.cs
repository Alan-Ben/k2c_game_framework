using UnityEngine;

namespace GOE
{
    public class GGUIWndUnLockConsortDetailInteractionTravelPage : _AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd<GGUIMonoUnlockConsortDetailInteractionTravelPage>
    {
        private GGUIWndConsortTravelContainer _m_wndTravelContainer;
        public GGUIWndUnLockConsortDetailInteractionTravelPage(_IGGUIWndUnlockConsortDetailInteractionPageParam _pageParam, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_pageParam, _commonAssetPathInfo, _parent)
        {
        }

        public override EUnlockConsortDetailWndInteractionPageTabType tabPageType
        {
            get { return EUnlockConsortDetailWndInteractionPageTabType.TRAVEL; }
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoTravelContainer != null)
                _m_wndTravelContainer = new GGUIWndConsortTravelContainer(wnd.monoTravelContainer);
        }

        protected override void _onDiscardSub()
        {
            _m_wndTravelContainer?.discard();
            _m_wndTravelContainer = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wndTravelContainer?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wndTravelContainer?.resetWnd();
        }
        
        protected override void _setDataSub()
        {
            
        }
        
        protected override void _refreshWndSub()
        {
            if (_m_wndTravelContainer != null)
            {
                _m_wndTravelContainer.showWnd();
                _m_wndTravelContainer.setData(_m_iConsortInfo);
            }
        }
    }
}