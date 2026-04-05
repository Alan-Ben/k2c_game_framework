using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class MainAdditionDragBoxGameTDScene : _AMainAdditionMiniGameTDScene
    {
        private static MainAdditionDragBoxGameTDScene _g_instance;
        [NotNull] public static MainAdditionDragBoxGameTDScene instance { get { return _g_instance ??= new MainAdditionDragBoxGameTDScene(); } }
        
        private long _m_lTdSceneSerialId;
        private GTDMonoDragBoxGame _m_gameMono;
        
        private DragBoxGameLogic _m_gameLogic;
        
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
                _m_gameMono = _g0.GetComponent<GTDMonoDragBoxGame>();
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonCanDragGoInputDefaultDealer(_sceneRefObj);
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

        public void setGameLogic(DragBoxGameLogic _gameLogic)
        {
            _m_gameLogic = _gameLogic;
        }
        
        public void popGamePrefab(NPCommonAssetPathInfo _assetPathInfo, Action<GTDDragBoxGamePrefab> _onLoadDone)
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
                    Debug.LogError($"【MainAdditionDragBoxGameTDScene.popGamePrefab Error】加载的预制体:{_assetPathInfo}为空");
                    _onLoadDone.Invoke(null);
                    return;
                }
               
                GTDMonoDragBoxGamePrefab monoGamePrefab = _go.GetComponent<GTDMonoDragBoxGamePrefab>();
                if (monoGamePrefab == null)
                {
                    Debug.LogError($"【MainAdditionDragBoxGameTDScene.popGamePrefab Error】加载的预制体:{_assetPathInfo} 不存在MainAdditionDragBoxGameTDScene 组件");
                    _onLoadDone.Invoke(null);
                    return;
                }
                
                monoGamePrefab.transform.SetParent(_m_gameMono == null ? null : _m_gameMono.gamePrefabParent, false);
                GTDDragBoxGamePrefab gamePrefab = new GTDDragBoxGamePrefab(monoGamePrefab);
                gamePrefab.onInit(_onGameSuccess);
                
                _onLoadDone.Invoke(gamePrefab);
            });
        }

        public void pushBackGamePrefab(NPCommonAssetPathInfo _assetPathInfo, GTDDragBoxGamePrefab _gamePrefab)
        {
            if(_gamePrefab == null)
                return;
            
            _gamePrefab.onDiscard();
            GAssetPathCacheMgr.instance.pushbackItem(_assetPathInfo, _gamePrefab.go);
        }

        private void _onGameSuccess()
        {
            _m_gameLogic?.setGameSuccess(true);
        }
    }
}