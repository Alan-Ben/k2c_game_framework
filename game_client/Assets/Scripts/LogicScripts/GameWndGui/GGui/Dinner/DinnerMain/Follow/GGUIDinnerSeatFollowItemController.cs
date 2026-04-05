using ALPackage;

namespace GOE
{
    public class GGUIDinnerSeatFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoDinnerSeatFollowItem, GGUIWndDinnerSeatFollowItem>
    {
        private readonly GResPathIndex _m_index;

        public GGUIDinnerSeatFollowItemController():base()
        {            
            _m_index = new GResPathIndex(2913);
        }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndDinnerSeatFollowItem _createItemWnd(GGUIMonoDinnerSeatFollowItem _wndMono)
        {
            return new GGUIWndDinnerSeatFollowItem(_wndMono);
        }

        public void setShowData(GDinnerJoinerInfo _joinerInfo)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                wnd?.showWnd();
                if (_joinerInfo != null)
                    _joinerInfo.regDetailInfo((_info) => { wnd?.setInfo(_info); });
            });
        }
    }
}