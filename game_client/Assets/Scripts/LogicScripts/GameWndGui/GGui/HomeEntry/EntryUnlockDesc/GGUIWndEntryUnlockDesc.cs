using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口解锁描述跟随窗口信息
    /// </summary>
    public class GGUIWndEntryUnlockDesc : _ATALGGUIWndCommonFollowItem<GGUIMonoEntryUnlockDesc>
    {
        private long _m_showSerialize;

        public GGUIWndEntryUnlockDesc(GGUIMonoEntryUnlockDesc _wnd) : base(_wnd)
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

        public void setInfo(EntryPointRefObj _refObj)
        {
            if (wnd == null || _refObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_refObj.entry_lock_desc, _refObj.entry_lock_desc_args));
        }
    }
}