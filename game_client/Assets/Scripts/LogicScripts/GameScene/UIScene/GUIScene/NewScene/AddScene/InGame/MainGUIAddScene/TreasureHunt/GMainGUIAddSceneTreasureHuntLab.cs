using System;
using ALPackage;

namespace GOE
{
    public class GMainGUIAddSceneTreasureHuntLab : _ANPGMainGUIAddSceneResBar
    {
        private static GMainGUIAddSceneTreasureHuntLab _g_instance;
        public static GMainGUIAddSceneTreasureHuntLab instance { get { return _g_instance ??= new GMainGUIAddSceneTreasureHuntLab(); } }

        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);
            
            GGUIWndTreasureHuntLab.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntLabFollowItemRoot.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitSceneSub()
        {
            GGUIWndTreasureHuntLab.instance.discard();
            GGUIWndTreasureHuntLabFollowItemRoot.instance.discard();
        }
        
        public override void _dealShowScene(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(_delegate);
            
            GGUIWndTreasureHuntLab.instance.showWnd(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntLabFollowItemRoot.instance.showWnd(stepCounter.addDoneStepCount);
            showResBar(GGUIWndTreasureHuntLab.instance.getBarResId(), GGUIWndTreasureHuntLab.instance.getPlayerIconResId(), stepCounter.addDoneStepCount);
        }

        protected override void _dealHideSceneSub(Action _delegate)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);
            stepCounter.regAllDoneDelegate(_delegate);
            
            GGUIWndTreasureHuntLab.instance.hideWnd(stepCounter.addDoneStepCount);
            GGUIWndTreasureHuntLabFollowItemRoot.instance.hideWnd(stepCounter.addDoneStepCount);
            hideResBar();
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 设置显示的实验室
        /// </summary>
        public void setShowLab(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _putInTreasureInfo, long _selectedTreasureId)
        {
            GGUIWndTreasureHuntLab.instance.setData(_labRefObj, _putInTreasureInfo, _selectedTreasureId);
        }
    }
}