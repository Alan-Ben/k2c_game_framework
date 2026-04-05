using ALPackage;

namespace GOE
{
    /// <summary>
    /// 前往火星留言附加窗口
    /// </summary>
    public class GGUIWndMarsGoToSubMsg : _ATALBasicUISubWnd<GGUIMonoMarsGoToSubMsg>
    {
        //留言容器
        private GGUIWndMarsGoToSubMsgContainer _m_wMsgContainer;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndMarsGoToSubMsg(GGUIMonoMarsGoToSubMsg _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wMsgContainer?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wMsgContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wMsgContainer?.discard();
            _m_wMsgContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (_m_wMsgContainer == null)
                _m_wMsgContainer = new GGUIWndMarsGoToSubMsgContainer(wnd.monoMsgContainer);
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_stageId"></param>
        public void setInfo(long _stageId)
        {
            if (wnd == null)
                return;

            //先隐藏留言容器，等数据回来后再显示，避免展示过时数据
            _m_wMsgContainer?.hideWnd();

            //请求数据，展示留言列表
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long curShowSerialize = _m_lShowSerialize;
            NPPlayer.instance.marsComp.goToSubComponent.reqGetStageMsgList((int)_stageId, _msg =>
            {
                if (_msg == null || !isShow || curShowSerialize != _m_lShowSerialize)
                    return;

                _m_wMsgContainer?.showWnd();
                _m_wMsgContainer?.showItemList(_msg.getMsgList(), _msg.getStage());
            });
        }
    }
}
