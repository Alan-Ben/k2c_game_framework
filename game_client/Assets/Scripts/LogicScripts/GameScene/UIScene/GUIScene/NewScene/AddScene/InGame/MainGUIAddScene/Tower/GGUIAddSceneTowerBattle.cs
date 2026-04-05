using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIAddSceneTowerBattle : _ANPBasicAddContainerUIScene
    { 
        private static GGUIAddSceneTowerBattle _g_instance = new GGUIAddSceneTowerBattle();
        [NotNull] 
        public static GGUIAddSceneTowerBattle instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIAddSceneTowerBattle();
                return _g_instance;
            }
        }

        public GGUIAddSceneTowerBattle():base(false)
        {
            
        }
        
        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndTowerBattle.instance.load(stepCounter.addDoneStepCount);
            GGUIWndTowerBattleFollowShow.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _dealQuitScene()
        {
            GGUIWndTowerBattle.instance.discard();
            GGUIWndTowerBattleFollowShow.instance.discard();
        }

        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndTowerBattle.instance.showWnd();
            GGUIWndTowerBattleFollowShow.instance.showWnd();

            if (null != _delegate)
                _delegate();
        }

        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndTowerBattle.instance.hideWnd();
            GGUIWndTowerBattleFollowShow.instance.hideWnd();

            if (null != _delegate)
                _delegate();
        }

        public void setInfo(TowerChallengeResult _result)
        {
            GGUIWndTowerBattle.instance.setInfo(_result);
        }
    }
}