using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 日常窗口
    /// </summary>
    public class GMainGUIAddSceneDailyMainInfo : _ANPBasicAddContainerUIScene
    {
        private static GMainGUIAddSceneDailyMainInfo _g_instance = new GMainGUIAddSceneDailyMainInfo();
        [NotNull] 
        public static GMainGUIAddSceneDailyMainInfo instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GMainGUIAddSceneDailyMainInfo();
                return _g_instance;
            }
        }

        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndDailyMain.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndDailyMain.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndDailyMain.instance.showWnd();

            _delegate?.Invoke();
        }

        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndDailyMain.instance.hideWnd();

            _delegate?.Invoke();
        }
    }
}
