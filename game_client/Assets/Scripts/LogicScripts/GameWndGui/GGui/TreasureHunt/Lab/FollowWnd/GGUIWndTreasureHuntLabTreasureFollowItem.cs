using System;
using ALPackage;

namespace GOE
{
    public class GGUIWndTreasureHuntLabTreasureFollowItem : _ATALGGUIWndCommonFollowItem<GGUIMonoTreasureHuntLabTreasureFollowItem>
    {
        private _ITreasureHuntTreasureInfo _m_iTreasureInfo;
        
        public GGUIWndTreasureHuntLabTreasureFollowItem(GGUIMonoTreasureHuntLabTreasureFollowItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
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

        public void setData(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_iTreasureInfo = _treasureInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iTreasureInfo == null)
                return;
            
            if(wnd.stateShow != null)
                wnd.stateShow.setShowData(_m_iTreasureInfo.treasureState);
        }
    }

    public class GGUITreasureHuntLabTreasureFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoTreasureHuntLabTreasureFollowItem, GGUIWndTreasureHuntLabTreasureFollowItem>
    {
        private readonly GResPathIndex _m_index;

        public GGUITreasureHuntLabTreasureFollowItemController(int _uiPathId) : base()
        {            
            _m_index = new GResPathIndex(_uiPathId);
        }
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_index; } }
        protected override GGUIWndTreasureHuntLabTreasureFollowItem _createItemWnd(GGUIMonoTreasureHuntLabTreasureFollowItem _wndMono)
        {
            GGUIWndTreasureHuntLabTreasureFollowItem itemWnd = new GGUIWndTreasureHuntLabTreasureFollowItem(_wndMono);
            return itemWnd;
        }

        public void dealItemWnd(Action<GGUIWndTreasureHuntLabTreasureFollowItem> _action)
        {
            if(_action == null)
                return;
            
            regItemWndLoadDoneDelegate(() =>
            {
                _action(wnd);
            });
        }
    }
}