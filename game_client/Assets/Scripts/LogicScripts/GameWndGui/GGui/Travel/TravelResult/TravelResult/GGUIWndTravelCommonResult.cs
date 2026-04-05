using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelCommonResult : _ATravelResultWnd<GGUIMonoTravelCommonResult, CommonTravelEventResultInfo>
    {
        public GGUIWndTravelCommonResult(CommonTravelEventResultInfo _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelCommonResult.uiResPathId, EALUIWndLayer.ADDITION)
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