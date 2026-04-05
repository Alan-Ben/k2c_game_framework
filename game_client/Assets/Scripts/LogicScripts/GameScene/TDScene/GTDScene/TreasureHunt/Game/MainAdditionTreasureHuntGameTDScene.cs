using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionTreasureHuntGameTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionTreasureHuntGameTDScene instance { get { return _g_instance ??= new MainAdditionTreasureHuntGameTDScene(); } }
        private static MainAdditionTreasureHuntGameTDScene _g_instance;
        
        
        private GTDMonoTreasureHuntGame _m_sceneMono;
        private float _m_defaultPlayerPositionX = 0f;
        
        
        public override bool needDiscardOnSwitch { get { return true; } }
        public override SceneInfoRefObj sceneRefObj { get { return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_game_scene_id); } }


        protected override void _onSceneInited()
        {
        }
        protected override void _onQuitTDScene()
        {
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoTreasureHuntGame>();
            if (_m_sceneMono != null && _m_sceneMono.monoPlayer != null)
                _m_defaultPlayerPositionX = _m_sceneMono.monoPlayer.transform.localPosition.x;
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


        public void createUnit<T>(NPGGoIndex _resIndex, Vector3 _position, Action<T> _complete) 
            where T : MonoBehaviour
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error($"{_resIndex} load failed, scene mono is null.");
                _complete?.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(_resIndex, _go =>
            {
                if (_go == null)
                {
                    ALLog.Error($"{_resIndex} load failed, cannot find the resource.");
                    _complete?.Invoke(null);
                    return;
                }

                T mono = _go.GetComponent<T>();
                if (mono == null || _m_sceneMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _go);
                    ALLog.Error($"{_resIndex} load failed, the resource doesn't have the component {typeof(T).Name}.");
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.unitRoot);
                monoTrans.localPosition = _position;
                _complete?.Invoke(mono);
            });
        }
        public void discardUnit<T>(NPGGoIndex _resIndex, T _mono)
            where T : MonoBehaviour
        {
            if (_mono == null)
                return;
            
            GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _mono.gameObject);
        }
        public GTDMonoTreasureHuntPlayer getPlayerMono()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getPlayerMono failed, scene mono is null.");
                return null;
            }

            if (_m_sceneMono.monoPlayer == null)
            {
                ALLog.Error("getPlayerMono failed, monoPlayer is null.");
                return null;
            }

            return _m_sceneMono.monoPlayer;
        }
        public Transform getUnitRoot()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getUnitRoot failed, scene mono is null.");
                return null;
            }

            if (_m_sceneMono.unitRoot == null)
            {
                ALLog.Error("getUnitRoot failed, unitRoot is null.");
                return null;
            }

            return _m_sceneMono.unitRoot;
        }
        public GTDMonoTreasureHuntGame getSceneMono()
        {
            return _m_sceneMono;
        }
        public float getDefaultPlayerPositionX()
        {
            return _m_defaultPlayerPositionX;
        }
        public float getWorldScale()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getWorldScale failed, scene mono is null.");
                return 1f;
            }

            return _m_sceneMono.worldScale;
        }
        public float getEnemySpawnTimeInRunUpState()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getEnemySpawnTimeInRunUpState failed, scene mono is null.");
                return 3f; // 默认值
            }

            return _m_sceneMono.enemySpawnTimeInRunUpState;
        }
        public NPGGoIndex getRewardDistanceLineResIndex()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getRewardDistanceLineResIndex failed, scene mono is null.");
                return null;
            }

            return _m_sceneMono.rewardDistanceLineResIndex;
        }
        public void doEndGameDelay(bool _isWin, Action _complete)
        {
            if (_complete == null)
                return;
            
            if (_m_sceneMono == null)
            {
                ALLog.Error("doEndGameDelay failed, scene mono is null.");
                _complete.Invoke();
                return;
            }
            
            ALCommonTaskController.CommonActionAddMonoTask(_complete, _isWin ? _m_sceneMono.gameWinDelay : _m_sceneMono.gameLoseDelay);
        }
        public Transform getGroundPos()
        {
            if (_m_sceneMono == null)
                return null;
            
            return _m_sceneMono.transGroundPos;
        }
    }
}