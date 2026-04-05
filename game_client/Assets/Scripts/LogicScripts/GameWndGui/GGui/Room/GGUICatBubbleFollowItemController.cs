using ALPackage;

namespace GOE
{
    /// <summary>
    /// 猫咪气泡跟随入口item控制器
    /// </summary>
    public class GGUICatBubbleFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoCatBubbleFollowItem, GGUIWndCatBubbleFollowItem>
    {
        private readonly GResPathIndex _m_index;

        public GGUICatBubbleFollowItemController():base()
        {            
            _m_index = new GResPathIndex(1513);
        }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndCatBubbleFollowItem _createItemWnd(GGUIMonoCatBubbleFollowItem _wndMono)
        {
            return new GGUIWndCatBubbleFollowItem(_wndMono);
        }

        /// <summary>
        /// 展示入口
        /// </summary>
        public void setShowEntrance()
        {
            regItemWndLoadDoneDelegate(() =>
            {
                wnd?.showWnd();
            });
        }
    }
}