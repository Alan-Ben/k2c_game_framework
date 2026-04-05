using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星探险UiScene
    /// </summary>
    public class GMainGUIAddSceneMarsExplore : _ANPBasicAddContainerUIScene
    { 
        [NotNull] public static GMainGUIAddSceneMarsExplore instance { get { return _g_instance ??= new GMainGUIAddSceneMarsExplore(); } }
        private static GMainGUIAddSceneMarsExplore _g_instance = new GMainGUIAddSceneMarsExplore();
        

        public GMainGUIAddSceneMarsExplore()
            : base(false)
        {
        }
        
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndMarsExplore.instance.load(stepCounter.addDoneStepCount);
            GGUIWndMarsExploreHUDRoot.instance.load(stepCounter.addDoneStepCount);
        }
        protected override void _onSceneInited()
        {
        }
        protected override void _dealQuitScene()
        {
            GGUIWndMarsExplore.instance.discard();
            GGUIWndMarsExploreHUDRoot.instance.discard();
        }
        

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndMarsExplore.instance.showWnd();
            GGUIWndMarsExploreHUDRoot.instance.showWnd();

            _delegate?.Invoke();
        }
        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndMarsExplore.instance.hideWnd();
            GGUIWndMarsExploreHUDRoot.instance.hideWnd();

            _delegate?.Invoke();
        }
    }
}