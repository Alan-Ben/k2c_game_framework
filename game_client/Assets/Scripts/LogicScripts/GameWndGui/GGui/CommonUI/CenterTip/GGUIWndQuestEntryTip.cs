using ALPackage;

namespace GOE
{
    /// <summary>
    /// 主线任务入口提示
    /// </summary>
    public class GGUIWndQuestEntryTip : _ANPGGUIBasicWnd<GGUIMonoQuestEntryTip>
    {
        private static GGUIWndQuestEntryTip _g_instance = new GGUIWndQuestEntryTip();
        public static GGUIWndQuestEntryTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndQuestEntryTip();
                return _g_instance;
            }
        }

        //操作序列号
        private long _m_lOpSerialize;

        protected GGUIWndQuestEntryTip() : base(EALUIWndLayer.NOTICE)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoQuestEntryTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoQuestEntryTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GET_QUEST_REWARD, _onGetQuestReward);
            WinMsg.RegisterMsgAct(WinMsgType.ON_QUEST_ENTRY_SHOW, _onQuestEntryShow);
            WinMsg.RegisterMsgAct(WinMsgType.ON_QUEST_CLICK_GOTO, _onClickGoTo);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _delayHideTip();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GET_QUEST_REWARD, _onGetQuestReward);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_QUEST_ENTRY_SHOW, _onQuestEntryShow);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_QUEST_CLICK_GOTO, _onClickGoTo);
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //设置CustomMono是使用在tip中
            if (wnd.questCustomMono != null)
                wnd.questCustomMono.isUseInTipPrefab = true;
        }

        //延时隐藏tip
        private void _delayHideTip()
        {
            if (wnd == null)
                return;

            long opSerialize = _m_lOpSerialize;
            //定时隐藏
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (opSerialize != _m_lOpSerialize)
                    return;

                if(wnd == null || wnd.noOpHideAni == null)
                    hideWnd();
                else
                    wnd.noOpHideAni.forcePlay(hideWnd);
            }, wnd.delayHideTimeSec);
        }

        //播放显示动画
        private void _playShowAni()
        {
            if (wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(wnd.showAniName))
                return;

            wnd.wndAnimation.ForcePlay(wnd.showAniName);
        }

        //任务领取完奖励
        private void _onGetQuestReward()
        {
            //检查是否需要展示下一个任务tip，不需要就直接隐藏tip
            if (NPPlayer.instance.questComp.checkCanShowNextEntryTip())
            {
                _m_lOpSerialize = ALSerializeOpMgr.next();
                _playShowAni();
                _delayHideTip();
            }
            else
                hideWnd();
        }

        //主线任务入口显示
        private void _onQuestEntryShow()
        {
            hideWnd();
        }

        //点击前往按钮
        private void _onClickGoTo()
        {
            hideWnd();
        }
    }
}
