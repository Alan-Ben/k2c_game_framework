using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame.TakeThingsSequentiallyGame
{
    public class MainAdditionTakeThingsSequentiallyGameTDScene : _AMainAdditionMiniGameTDScene
    {
        private static MainAdditionTakeThingsSequentiallyGameTDScene _g_instance;
        [NotNull] public static MainAdditionTakeThingsSequentiallyGameTDScene instance { get { return _g_instance ??= new MainAdditionTakeThingsSequentiallyGameTDScene(); } }
        
        private long _m_lTdSceneSerialId;

        private GTDTakeThingsSequentiallyGame _m_gameMono;
        
        protected override void _onSceneInited()
        {
        }

        public override void onSwitchHideScene()
        {
        }

        protected override void _onQuitTDScene()
        {
            _m_gameMono = null;
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if(_m_gameMono == null && _g0 != null)
                _m_gameMono = _g0.GetComponent<GTDTakeThingsSequentiallyGame>();
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _dealShowSceneNP(Action _delegate)
        {
            _m_lTdSceneSerialId = ALSerializeOpMgr.next();
            
            if(sceneRefObj != null)
                CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            _m_lTdSceneSerialId = ALSerializeOpMgr.next();

            _delegate?.Invoke();
        }

        public void popGamePrefab(NPCommonAssetPathInfo _assetPathInfo, Action<GTDTakeThingsSequentiallyGamePrefab> _onLoadDone)
        {
            if (_onLoadDone == null)
                return;

            if (_assetPathInfo == null || !_assetPathInfo.enable)
            {
                _onLoadDone?.Invoke(null);
                return;
            }
            
            long serialId = _m_lTdSceneSerialId;
            GAssetPathCacheMgr.instance.popItem(_assetPathInfo, (_go) =>
            {
                if (serialId != _m_lTdSceneSerialId)
                {
                    GAssetPathCacheMgr.instance.pushbackItem(_assetPathInfo, _go);
                    return;
                }
                
                if (_go == null)
                {
                    Debug.LogError($"【MainAdditionTakeThingsSequentiallyGameTDScene.popGamePrefab Error】加载的预制体:{_assetPathInfo}为空");
                    _onLoadDone.Invoke(null);
                    return;
                }
               
                GTDMonoTakeThingsSequentiallyGamePrefab monoGamePrefab = _go.GetComponent<GTDMonoTakeThingsSequentiallyGamePrefab>();
                if (monoGamePrefab == null)
                {
                    Debug.LogError($"【MainAdditionTakeThingsSequentiallyGameTDScene.popGamePrefab Error】加载的预制体:{_assetPathInfo}没有GTDMonoTakeThingsSequentiallyGamePrefab组件");
                    _onLoadDone.Invoke(null);
                    return;
                }
                
                monoGamePrefab.transform.SetParent(_m_gameMono == null ? null : _m_gameMono.gamePrefabParent, false);
                GTDTakeThingsSequentiallyGamePrefab gamePrefab = new GTDTakeThingsSequentiallyGamePrefab(monoGamePrefab);
                gamePrefab.onInit();
                
                _onLoadDone.Invoke(gamePrefab);
            });
        }

        public void pushBackGamePrefab(NPCommonAssetPathInfo _assetPathInfo, GTDTakeThingsSequentiallyGamePrefab _gamePrefab)
        {
            if(_gamePrefab == null)
                return;
            
            _gamePrefab.onDiscard();
            GAssetPathCacheMgr.instance.pushbackItem(_assetPathInfo, _gamePrefab.go);
        }
    }
}