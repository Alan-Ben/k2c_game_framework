using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口引导手指
    /// </summary>
    public class GGUIWndEntryGuideHand : _ATALGGUIWndCommonFollowItem<GGUIMonoEntryGuideHand>
    {
        private long _m_showSerialize;

        public GGUIWndEntryGuideHand(GGUIMonoEntryGuideHand _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            _m_showSerialize = ALSerializeOpMgr.next();
            long serialize = _m_showSerialize;

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_showSerialize || !isShow)
                    return;

                hideWnd();
            },wnd.delayHideTime);
        }

        protected override void _onHideWnd()
        {
            _m_showSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }
    }
}