namespace GOE
{
    /// <summary>
    /// 通用带两种状态的按钮
    /// </summary>
    public class NPGGUIWndCommonStateBtn : _ATNPGGUIWndStateBtn<ENPCommonBtnState, NPGGUIMonoCommonStateBtn>
    {
        public NPGGUIWndCommonStateBtn(NPGGUIMonoCommonStateBtn _mono) : base(_mono)
        {
            initWnd();
        }

        private NPGGUIWndCommonItem _m_itemWnd;

        protected override void _onDiscard()
        {
            base._onDiscard();
            if (null != _m_itemWnd)
                _m_itemWnd.discard();
            _m_itemWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (null != wnd.itemMono)
                _m_itemWnd = new NPGGUIWndCommonItem(wnd.itemMono);
        }

        public void setItem(NPCommonCostItem _item)
        {
            if (null == _item)
                return;

            if (null != _m_itemWnd)
                _m_itemWnd.setItem(_item);

        }
    }
}