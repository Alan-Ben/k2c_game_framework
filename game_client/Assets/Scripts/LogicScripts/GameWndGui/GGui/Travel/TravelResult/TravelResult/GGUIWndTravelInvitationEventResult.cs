using ALPackage;

namespace GOE
{
    /// <summary>
    /// 指定邀约事件结果窗口
    /// </summary>
    public class GGUIWndTravelInvitationEventResult : _ATravelSpecificEventResultWnd<GGUIMonoTravelInvitationEventResult, TravelInvitationEventInfo, _ATravelSpecificEventResultInfo<TravelInvitationEventInfo>>
    {
        private GGUIWndConsortIconItem _m_consortIcon;
        
        public GGUIWndTravelInvitationEventResult(_ATravelSpecificEventResultInfo<TravelInvitationEventInfo> _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelInvitationEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoConsortIcon)
                _m_consortIcon = new GGUIWndConsortIconItem(wnd.monoConsortIcon);
        }

        protected override void _onDiscardSub()
        {
            _m_consortIcon?.discard();
            _m_consortIcon = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_consortIcon?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_consortIcon?.resetWnd();
        }

        protected override void _onRefreshWnd()
        {
            if (_m_consortIcon != null && _m_eventResultInfo != null && _m_eventResultInfo.specificEventInfo != null)
            {
                _m_consortIcon.showWnd();
                _m_consortIcon.setInfo(_m_eventResultInfo.specificEventInfo.selectConsortInfo, 0);
            }
        }
    }
}