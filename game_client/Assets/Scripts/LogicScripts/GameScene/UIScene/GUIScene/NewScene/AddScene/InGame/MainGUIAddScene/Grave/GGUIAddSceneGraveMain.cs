using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIAddSceneGraveMain : _ANPBasicAddContainerUIScene
    { 
        private static GGUIAddSceneGraveMain _g_instance = new GGUIAddSceneGraveMain();
        [NotNull] 
        public static GGUIAddSceneGraveMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIAddSceneGraveMain();
                return _g_instance;
            }
        }

        public GGUIAddSceneGraveMain():base(false)
        {
            
        }
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndGraveMain.instance.load(stepCounter.addDoneStepCount);
            GGUIWndGraveMainFollowRoot.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndGraveMain.instance.discard();
            GGUIWndGraveMainFollowRoot.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndGraveMain.instance.showWnd();
            GGUIWndGraveMainFollowRoot.instance.showWnd();

            if (null != _delegate)
                _delegate();
        }

        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndGraveMain.instance.hideWnd();
            GGUIWndGraveMainFollowRoot.instance.hideWnd();

            if (null != _delegate)
                _delegate();
        }
    }
}