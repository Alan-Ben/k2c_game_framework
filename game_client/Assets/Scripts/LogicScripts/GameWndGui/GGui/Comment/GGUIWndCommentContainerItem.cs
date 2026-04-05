using ALPackage;

namespace GOE
{
    // 关卡气泡item
    public class GGUIWndCommentContainerItem : _ATALBasicUISubWnd<GGUIMonoCommentContainerItem>
    {
        //关卡气泡数据
        private CommentRandomInfo _m_info;
        //图片
        private NPGGuiWndTexture _m_iconWnd;

        public GGUIWndCommentContainerItem(GGUIMonoCommentContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public CommentRandomInfo info { get { return _m_info; } }

        // 初始化
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.iconImg)
                _m_iconWnd = new NPGGuiWndTexture(wnd.iconImg);

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

        protected override void _onDiscard()
        {
            if (null != _m_iconWnd)
                _m_iconWnd.discard();
            _m_iconWnd = null;
        }

        public void setInfo(CommentRandomInfo _info)
        {
            if (null == _info)
                return;

            _m_info = _info;
            _refresh();
        }

        private void _refresh()
        {
            if (null == wnd || null == _m_info)
                return;
            
            if (null != _m_iconWnd)
            {
                _m_iconWnd.setTexture(_m_info.icon);
                _m_iconWnd.showWnd();
            }

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_info.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_info.comment, _m_info.comment_args));
        }
    }
}
