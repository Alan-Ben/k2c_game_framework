using ALPackage;
using NPEnum;
using System;

namespace GOE
{
    /// <summary>
    /// 家人CG分享聊天频道项
    /// </summary>
    public class GGUIWndConsortCGShareChannelItem : _ATALBasicUISubWnd<GGUIMonoConsortCGShareChannelItem>
    {
        //页签
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<ENPChatRoomType> _m_aOnClickItem;

        /// <summary>
        /// 当前频道类型
        /// </summary>
        public ENPChatRoomType channelType { get { return wnd == null ? ENPChatRoomType.NONE : wnd.channelType; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<ENPChatRoomType> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndConsortCGShareChannelItem(GGUIMonoConsortCGShareChannelItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_wTab?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTab?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTab?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wTab?.discard();
            _m_wTab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onClickItem;
            }
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }

        //点击item事件
        private void _onClickItem(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_aOnClickItem?.Invoke(wnd.channelType);
        }
    }
}