using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIAddSceneTravelMain:_ANPBasicAddContainerUIScene
    { 
        private static GGUIAddSceneTravelMain _g_instance = new GGUIAddSceneTravelMain();
        [NotNull] 
        public static GGUIAddSceneTravelMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIAddSceneTravelMain();
                return _g_instance;
            }
        }

        private _ITravelController _m_travelController;
        
        public GGUIAddSceneTravelMain():base(false)
        {
            
        }
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndTravelMain.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTravelFollowRoot.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTravelDealEventBg.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndTravelMain.instance.discard();
            GGUIWndTravelFollowRoot.instance.discard();
            GGUIWndTravelDealEventBg.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            refreshSceneShowWnd(_delegate);
        }

        public override void _dealHideScene(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(_delegate);

            GGUIWndTravelMain.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndTravelFollowRoot.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndTravelDealEventBg.instance.hideWnd(stepCounter.addDoneStepCount);
        }
        
        public void setTravelController(_ITravelController _controller)
        {
            _m_travelController = _controller;
        }

        public void refreshSceneShowWnd(Action _refreshComplete)
        {
            if (!isShow)
            {
                _refreshComplete?.Invoke();
                return;
            }
            
            if (NPPlayer.instance.travelComp.curDealEvent != null && !NPPlayer.instance.travelComp.curDealEvent.eventDealDone)
            {
                GGUIWndTravelMain.instance.hideWnd();
                GGUIWndTravelFollowRoot.instance.hideWnd();
                
                GGUIWndTravelDealEventBg.instance.refreshWnd(NPPlayer.instance.travelComp.curDealEvent.travelPosRefObj);
                GGUIWndTravelDealEventBg.instance.showWnd(_refreshComplete);
            }
            else
            {
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(2);
                stepCounter.regAllDoneDelegate(_refreshComplete);
            
                GGUIWndTravelDealEventBg.instance.hideWnd();
                
                GGUIWndTravelMain.instance.setTravelController(_m_travelController);
                GGUIWndTravelMain.instance.showWnd(stepCounter.addDoneStepCount);
                GGUIWndTravelFollowRoot.instance.showWnd(stepCounter.addDoneStepCount);
            }
        }
    }
}