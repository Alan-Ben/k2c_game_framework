using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口引导手指item控制器
    /// </summary>
    public class GGUIEntryGuideHandController : _ATALGGUICommonFollowItemController<GGUIMonoEntryGuideHand, GGUIWndEntryGuideHand>
    {
        private int _m_iUIResId;
        private readonly GResPathIndex _m_index;

        public GGUIEntryGuideHandController(int _uiResId):base()
        {
            _m_iUIResId = _uiResId;
            _m_index = new GResPathIndex(_uiResId);
        }

        public int uiResId { get { return _m_iUIResId; } }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndEntryGuideHand _createItemWnd(GGUIMonoEntryGuideHand _wndMono)
        {
            return new GGUIWndEntryGuideHand(_wndMono);
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