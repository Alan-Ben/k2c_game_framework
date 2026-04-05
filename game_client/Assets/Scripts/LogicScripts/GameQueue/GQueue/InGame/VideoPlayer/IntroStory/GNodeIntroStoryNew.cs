using System;

namespace GOE
{
    /// <summary>
    /// 开篇剧情Node
    /// </summary>
    public class GNodeIntroStoryNew : BaseQueueNode
    {
        private GGUIWndIntroStoryNew _m_wndIntroStory;

        private Action _m_aOnWndWarmUpDone;//窗口预热完成回调
        private Action _m_aOnClose;//关闭回调
        
        public GNodeIntroStoryNew(Action _onWndWarmUpDone, Action _onClose) : base(EUIQueueStageType.MAIN)
        {
            GCommon.sendStepReport(TraceConst.ADD_INTRO_STORY_NODE);//发送埋点-添加开篇剧情节点

            _m_aOnWndWarmUpDone = _onWndWarmUpDone;
            _m_aOnClose = _onClose;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        public override bool isOnlyUINode { get { return true; } }
        
        public override void onEnterQueue()
        {
            _m_wndIntroStory?.discard();
            _m_wndIntroStory = null;
        }

        public override void onClose()
        {
            _m_wndIntroStory?.discard();
            _m_wndIntroStory = null;

            _m_aOnWndWarmUpDone = null;

            if (_m_aOnClose != null) 
                _m_aOnClose();
            _m_aOnClose = null;
        }

        public override void doEnterNode(Action _triggerEnterDone)
        {
            if (_m_wndIntroStory == null)
            {
                _m_wndIntroStory = new GGUIWndIntroStoryNew(null);
                _m_wndIntroStory.load();
            }
            
            _m_wndIntroStory.regLoadDoneDelegate(() =>
            {
                _m_wndIntroStory.warmUpWnd(() =>
                {
                    _m_aOnWndWarmUpDone?.Invoke();
                    _triggerEnterDone?.Invoke();
                    
                    _m_wndIntroStory.showWnd();
                    _m_wndIntroStory.play();
                });   
            });
            
            base.doEnterNode(null);
        }
        
        public override void doQuitNode(Action _dealOnQuitDone)
        {
            _m_wndIntroStory?.discard();
            _m_wndIntroStory = null;
            
            _dealOnQuitDone?.Invoke();
            
            base.doQuitNode(null);
        }

        public override void EnterNode()
        {
        }

        public override void QuitNode()
        {
        }
    }
}