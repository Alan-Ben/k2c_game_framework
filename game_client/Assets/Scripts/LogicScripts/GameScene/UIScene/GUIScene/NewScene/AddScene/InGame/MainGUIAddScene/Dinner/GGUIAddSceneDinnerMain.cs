using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIAddSceneDinnerMain:_ANPBasicAddContainerUIScene
    { 
        private static GGUIAddSceneDinnerMain _g_instance = new GGUIAddSceneDinnerMain();
        [NotNull] 
        public static GGUIAddSceneDinnerMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIAddSceneDinnerMain();
                return _g_instance;
            }
        }
        private GDinnerInfo _m_dinnerInfo;

        public GGUIAddSceneDinnerMain():base(false)
        {
            
        }
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndDinnerMain.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndDinnerMain.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndDinnerMain.instance.setInfo(_m_dinnerInfo, ()=>
            {
                GGUIWndDinnerMain.instance.showWnd(_delegate);
            });
            
        }

        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndDinnerMain.instance.hideWnd(_delegate);
        }

        public void setInfo(GDinnerInfo _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
        }
    }
}