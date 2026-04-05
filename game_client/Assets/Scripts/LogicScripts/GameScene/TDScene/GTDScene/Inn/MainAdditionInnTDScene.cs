using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionInnTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionInnTDScene instance { get { return _g_instance ??= new MainAdditionInnTDScene(); } }
        private static MainAdditionInnTDScene _g_instance;
        
        
        private GTDMonoInnScene _m_sceneMono;
        
        
        public override bool needDiscardOnSwitch { get { return true; } }
        public override SceneInfoRefObj sceneRefObj { get { return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.inn_scene_id); } }
        

        protected override void _onSceneInited()
        {
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg += _tryCheckSpecialGuestShow;
            _tryCheckSpecialGuestShow();
        }
        protected override void _onQuitTDScene()
        {
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg -= _tryCheckSpecialGuestShow;
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoInnScene>();
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
                monoTrans.position = _position;
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

        public Vector3 getCashRegisterPosition()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getCashRegisterPosition failed, scene mono is null.");
                return Vector3.zero;
            }

            if (_m_sceneMono.cashRegisterPosition == null)
            {
                ALLog.Error("getCashRegisterPosition failed, cashRegisterPosition is null.");
                return Vector3.zero;
            }

            return _m_sceneMono.cashRegisterPosition.position;
        }
        public GTDMonoInnServeCounter getServeCounter()
        { 
            if (_m_sceneMono == null)
            {
                ALLog.Error("getServeCounter failed, scene mono is null.");
                return null;
            }

            if (_m_sceneMono.monoServeCounter == null)
            {
                ALLog.Error("getServeCounter failed, monoServeCounter is null.");
                return null;
            }

            ALUGUICommon.setGameObjEnable(_m_sceneMono.monoServeCounter, true);
            return _m_sceneMono.monoServeCounter;
        }
        public Vector3 getQueueOffset()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getQueueOffset failed, scene mono is null.");
                return Vector3.zero;
            }

            if (_m_sceneMono.monoServeCounter == null)
            {
                ALLog.Error("getQueueOffset failed, monoServeCounter is null.");
                return Vector3.zero;
            }

            return _m_sceneMono.monoServeCounter.getQueueOffset();
        }
        public Vector3 getQueuePosition(int _queueIndex)
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getQueuePosition failed, scene mono is null.");
                return Vector3.zero;
            }
            
            if (_queueIndex < 0)
            {
                if (_m_sceneMono.guestLeavePos == null)
                {
                    ALLog.Error("getQueuePosition failed, guestLeavePos is null.");
                    return Vector3.zero;
                }

                return _m_sceneMono.guestLeavePos.position;
            }

            if (_m_sceneMono.monoServeCounter == null)
            {
                ALLog.Error("getQueuePosition failed, monoServeCounter is null.");
                return Vector3.zero;
            }

            return _m_sceneMono.monoServeCounter.getQueuePosition(_queueIndex);
        }
        public Vector3 calculateSpawnPosition(int _totalGuestsInQueue)
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("calculateSpawnPosition failed, scene mono is null.");
                return Vector3.zero;
            }

            if (_m_sceneMono.monoServeCounter == null)
            {
                ALLog.Error("calculateSpawnPosition failed, monoServeCounter is null.");
                return Vector3.zero;
            }

            return _m_sceneMono.monoServeCounter.calculateSpawnPosition(_totalGuestsInQueue);
        }
        public float getGuestMoveSpeed()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getGuestMoveSpeed failed, scene mono is null.");
                return 0f;
            }

            if (_m_sceneMono.guestMoveSpeed <= 0f)
            {
                ALLog.Error("getGuestMoveSpeed failed, guestMoveSpeed is not set.");
                return 0f;
            }

            return _m_sceneMono.guestMoveSpeed;
        }
        public int getMaxGuestCount()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getMaxGuestCount failed, scene mono is null.");
                return 0;
            }

            if (_m_sceneMono.maxGuestCount <= 0)
            {
                ALLog.Error("getMaxGuestCount failed, maxGuestCount is not set.");
                return 0;
            }

            return _m_sceneMono.maxGuestCount;
        }


        private void _tryCheckSpecialGuestShow()
        {
            if (_m_sceneMono == null)
                return;

            InnSpecialGuestInfo specialGuestInfo = NPPlayer.instance.innComp.tryGetFirstSpecialGuest();
            bool hasSpecialGuest = specialGuestInfo != null;
            _m_sceneMono.setHasSpecialGuest(hasSpecialGuest);
        }
    }
}