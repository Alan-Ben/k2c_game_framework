using ALPackage;

namespace GOE
{
    /// <summary>
    /// 猫咪气泡的ui跟随窗口信息
    /// </summary>
    public class GGUIWndCatBubbleFollowItem : _ATALGGUIWndCommonFollowItem<GGUIMonoCatBubbleFollowItem>
    {
        //气泡是否被动画控制正在展示
        private bool _m_isShowBubble;

        public GGUIWndCatBubbleFollowItem(GGUIMonoCatBubbleFollowItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_CAT_BUBBLE_SHOW_ANI, _playBubbleShowAni);//触发猫咪气泡显示动画
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_CAT_BUBBLE_HIDE_ANI, _playBubbleHideAni);//触发猫咪气泡隐藏动画
            if (wnd != null && wnd.switchAni != null)
                wnd.switchAni.resetAni(ECatBubbleFollowItemAniType.SHOW);
            _m_isShowBubble = false;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_CAT_BUBBLE_SHOW_ANI, _playBubbleShowAni);//触发猫咪气泡显示动画
            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_CAT_BUBBLE_HIDE_ANI, _playBubbleHideAni);//触发猫咪气泡隐藏动画
            _m_isShowBubble = false;
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

        //播放显示动画
        private void _playBubbleShowAni()
        {
            if (wnd == null)
                return;

            //设置新气泡内容并播放动画
            string bubbleString = GRefdataCoreMgr.instance.getCatBubble();
            if (string.IsNullOrEmpty(bubbleString))
                return;

            ALUGUICommon.setLabelTxt(wnd.txtBubble, bubbleString);
            wnd.switchAni?.forcePlay(ECatBubbleFollowItemAniType.SHOW);
            _m_isShowBubble = true;
        }

        //播放隐藏动画
        private void _playBubbleHideAni()
        {
            if (wnd == null || !_m_isShowBubble)
                return;

            wnd.switchAni?.forcePlay(ECatBubbleFollowItemAniType.HIDE);
            _m_isShowBubble = false;
        }
    }
}