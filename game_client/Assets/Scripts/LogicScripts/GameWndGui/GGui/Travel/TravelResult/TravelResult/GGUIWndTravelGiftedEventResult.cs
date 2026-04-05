using ALPackage;

namespace GOE
{
    /// <summary>
    /// 卷王事件结果窗口
    /// </summary>
    public class GGUIWndTravelGiftedEventResult : _ATravelSpecificEventResultWnd<GGUIMonoTravelGiftedEventResult, TravelGiftdeEventInfo, _ATravelSpecificEventResultInfo<TravelGiftdeEventInfo>>
    {
        public GGUIWndTravelGiftedEventResult(_ATravelSpecificEventResultInfo<TravelGiftdeEventInfo> _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelGiftedEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onWndInitDoneSub()
        {
        }

        protected override void _onDiscardSub()
        {
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
        }

        protected override void _onResetSub()
        {
        }

        protected override void _onRefreshWnd()
        {
        }
    }
}