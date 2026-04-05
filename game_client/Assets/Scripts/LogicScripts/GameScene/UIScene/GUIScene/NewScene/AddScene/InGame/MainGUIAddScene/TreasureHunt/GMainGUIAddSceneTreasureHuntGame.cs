using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GMainGUIAddSceneTreasureHuntGame : _ANPGMainGUIAddSceneResBar
    {
        private static GMainGUIAddSceneTreasureHuntGame _g_instance;
        [NotNull] public static GMainGUIAddSceneTreasureHuntGame instance { get { return _g_instance ??= new GMainGUIAddSceneTreasureHuntGame(); } }

        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(4);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            GGUIWndTreasureHuntGameMain.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntGamePlay.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntGamePlayGamingTip.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntGamePlayRunUpTip.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitSceneSub()
        {
            GGUIWndTreasureHuntGameMain.instance.discard();
            GGUIWndTreasureHuntGamePlay.instance.discard();
            GGUIWndTreasureHuntGamePlayGamingTip.instance.discard();
            GGUIWndTreasureHuntGamePlayRunUpTip.instance.discard();
        }
        
        public override void _dealShowScene(Action _delegate)
        {
            showMainWndWithResBar(GGUIWndTreasureHuntGameMain.instance, _delegate);
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            _hideAll(_delegate);
        }


        public void switchToGamePlay()
        {
            showMainWnd(GGUIWndTreasureHuntGamePlay.instance);
        }
        public void switchToGameMain()
        {
            showMainWndWithResBar(GGUIWndTreasureHuntGameMain.instance, null);
        }
        public void addGamingTip()
        {
            showAddWnd(GGUIWndTreasureHuntGamePlayGamingTip.instance, null);
        }
        public void removeGamingTip()
        {
            GGUIWndTreasureHuntGamePlayGamingTip.instance.hideWnd();
        }
        public void addRunUpTip()
        {
            showAddWnd(GGUIWndTreasureHuntGamePlayRunUpTip.instance, null);
        }
        public void removeRunUpTip()
        {
            GGUIWndTreasureHuntGamePlayRunUpTip.instance.hideWnd();
        }
    }
}