using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    //3d主场景
    public class MainAdditionTowerBattleTDScene : _ABasicAdditionMainTDScene
    {
        private static MainAdditionTowerBattleTDScene _g_instance;
        [NotNull] public static MainAdditionTowerBattleTDScene instance { get { return _g_instance ??= new MainAdditionTowerBattleTDScene(); } }
        
        private TowerChallengeResult _m_battleResult;
        private GTDTowerBattleSceneMono _m_sceneMono;

        public MainAdditionTowerBattleTDScene()
        {
        }

        public override bool needDiscardOnSwitch { get => true; }
        
        protected override void _onSceneInited()
        {
            GTowerBattleSceneMgr.instance.initSceneMono(_m_sceneMono);
        }

        protected override void _onQuitTDScene()
        {
            GTowerBattleSceneMgr.instance.discard();
        }
        
        public override void onSwitchHideScene()
        {
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if (null == _m_sceneMono)
            {
                _m_sceneMono = _g0.GetComponent<GTDTowerBattleSceneMono>();
            }
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(2021);
                return sceneInfoRefObj;
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _dealShowSceneNP(Action _delegate)
        {
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            _delegate?.Invoke();
        }


        public void setInfo(TowerChallengeResult _result, Action _onComplete = null)
        {
            if (_result == null)
            {
                _onComplete?.Invoke();
                return;
            }
            _m_battleResult = _result;
            // 初始化舞台、战斗信息
            GTowerBattleSceneMgr.instance.initStage(_m_battleResult, _onComplete);
        }
    }
}