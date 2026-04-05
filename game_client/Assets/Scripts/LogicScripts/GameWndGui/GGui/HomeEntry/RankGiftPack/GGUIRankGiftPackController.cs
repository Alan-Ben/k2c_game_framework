using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口引导手指item控制器
    /// </summary>
    public class GGUIRankGiftPackController : _ATALGGUICommonFollowItemController<GGUIMonoRankGiftPackPointItem, GGUIWndRankGiftPackPointItem>
    {
        private readonly GResPathIndex _m_index;

        public GGUIRankGiftPackController():base()
        {
            _m_index = new GResPathIndex(8901);
        }

        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndRankGiftPackPointItem _createItemWnd(GGUIMonoRankGiftPackPointItem _wndMono)
        {
            return new GGUIWndRankGiftPackPointItem(_wndMono);
        }

        /// <summary>
        /// 展示入口
        /// </summary>
        public void setShowEntrance()
        {
            regItemWndLoadDoneDelegate(() =>
            {
                wnd?.showWnd();
                wnd?.forceRefresh();
            });
        }
    }
}