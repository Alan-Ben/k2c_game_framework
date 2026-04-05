using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionBuildingTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionBuildingTDScene instance { get { return _g_instance ??= new MainAdditionBuildingTDScene(); } }
        private static MainAdditionBuildingTDScene _g_instance;

        protected EntryGuideHandPointView _m_guideHand;
        private GTDMonoBuildingScene _m_sceneMono;
        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;
        
        
        public override bool needDiscardOnSwitch { get { return false; } }
        public override SceneInfoRefObj sceneRefObj { get { return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.building_scene_id); } }
        
        public BuildingSceneConfig sceneConfig { get { if (_m_sceneMono == null) return new BuildingSceneConfig(); return _m_sceneMono.config; } }
        

        protected override void _onSceneInited()
        {
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onQuitTDScene()
        {
            _m_guideHand?.discard();
            _m_guideHand = null;
            
            _m_sceneMono = null;
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoBuildingScene>();
        }
        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }
        protected override void _dealShowSceneNP(Action _delegate)
        {
            WinMsg.RegisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导
            WinMsg.RegisterMsg(WinMsgType.SHOW_GUIDE_HAND, _showGuideHand);
            WinMsg.RegisterMsgAct(WinMsgType.ON_START_TUTORIAL, _onStartTutorial);//开始引导消息
            
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            if (_m_lastQuitRecorded)
            {
                CameraController.instance.cameraPos = _m_lastQuitCameraPos;
                CameraController.instance.cameraFocusPos = _m_lastQuitCameraFocusPos;
            }
            _delegate?.Invoke();
        }
        protected override void _dealHideSceneNP(Action _delegate)
        {
            WinMsg.UnregisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导
            WinMsg.UnregisterMsg(WinMsgType.SHOW_GUIDE_HAND, _showGuideHand);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_START_TUTORIAL, _onStartTutorial);//开始引导消息
            
            _m_guideHand?.discard();
            _m_guideHand = null;
    
            _m_lastQuitRecorded = true;
            _m_lastQuitCameraPos = CameraController.instance.cameraPos;
            _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
            _delegate?.Invoke();
        }
        
        
        public Transform getBuildingPosTransform(long _buildingId)
        {
            if (_m_sceneMono == null)
                return null;

            foreach (GTDMonoBuildingPos posMono in _m_sceneMono.buildingPosList)
            {
                if (posMono.buildingId == _buildingId)
                    return posMono.transform;
            }

            return null;
        }
        public Vector3 getBuildingFocusPos(long _buildingId)
        {
            if (_m_sceneMono == null)
                return Vector3.zero;
            
            foreach (GTDMonoBuildingPos posMono in _m_sceneMono.specialBuildingFocusPosList)
            {
                if (posMono.buildingId == _buildingId)
                    return posMono.transform.position;
            }
            foreach (GTDMonoBuildingPos posMono in _m_sceneMono.buildingPosList)
            {
                if (posMono.buildingId == _buildingId)
                    return posMono.transform.position;
            }

            return Vector3.zero;
        }
        public Vector3 getBuildingPos(long _buildingId)
        {
            Transform posTransform = getBuildingPosTransform(_buildingId);
            if (posTransform != null)
                return posTransform.position;
            
            return Vector3.zero;
        }
        public bool tryGetBuildingPos(long _buildingId, out Vector3 _pos)
        {
            _pos = Vector3.zero;
            Transform posTransform = getBuildingPosTransform(_buildingId);
            if (posTransform != null)
            {
                _pos = posTransform.position;
                return true;
            }
            
            return false;
        }
        public Transform getAnecdoteTransform(long _posId)
        {
            if (_m_sceneMono == null)
                return null;

            foreach (GTDMonoAnecdotePos posMono in _m_sceneMono.anecdotePosList)
            {
                if (posMono.posId == _posId)
                    return posMono.transform;
            }

            return null;
        }
        
        public Transform getRankGiftPackPos()
        {
            if (_m_sceneMono == null)
                return null;

            return _m_sceneMono.rankGiftPackPos;
        }

        public GTDMonoRushExchange getRushExchangeMono()
        {
            if (_m_sceneMono == null)
                return null;
            return _m_sceneMono.rushExchangeMono;
        }
        
        public Vector3 getAnecdotePos(long _posId)
        {
            Transform posTransform = getAnecdoteTransform(_posId);
            if (posTransform != null)
                return posTransform.position;
            
            return Vector3.zero;
        }
        public bool tryGetAnecdotePos(long _posId, out Vector3 _pos)
        {
            _pos = Vector3.zero;
            Transform posTransform = getAnecdoteTransform(_posId);
            if (posTransform != null)
            {
                _pos = posTransform.position;
                return true;
            }
            
            return false;
        }
        public void createBuilding<T>(NPGGoIndex _resIndex, Vector3 _position, Action<T> _complete)
            where T : MonoBehaviour
        {
            if (_m_sceneMono == null)
            {
                _complete?.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(_resIndex, _go =>
            {
                if (_go == null)
                {
                    _complete?.Invoke(null);
                    return;
                }

                T mono = _go.GetComponent<T>();
                if (mono == null || _m_sceneMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _go);
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.buildingParent);
                monoTrans.position = _position;
                _complete?.Invoke(mono);
            });
        }
        public void discardBuilding<T>(NPGGoIndex _resIndex, T _building)
            where T : MonoBehaviour
        {
            if (_building == null)
                return;
            
            GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _building.gameObject);
        }
        public void createAnecdoteEvent<T>(NPGGoIndex _resIndex, long _posId, Action<T> _complete)
            where T : MonoBehaviour
        {
            if (_m_sceneMono == null)
            {
                _complete?.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(_resIndex, _go =>
            {
                if (_go == null)
                {
                    _complete?.Invoke(null);
                    return;
                }

                T mono = _go.GetComponent<T>();
                if (mono == null || _m_sceneMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _go);
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.anecdoteParent);
                monoTrans.position = getAnecdotePos(_posId);
                _complete?.Invoke(mono);
            });
        }
        public void discardAnecdoteEvent<T>(NPGGoIndex _resIndex, T _obj)
            where T : MonoBehaviour
        {
            if (_obj == null)
                return;
            
            GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _obj.gameObject);
        }
        public void setCameraSize(float _size, float _duration)
        {
            if (!isEntered || sceneRefObj == null)
                return;
            
            if (sceneRefObj.camera_setting.isOrthographic)
                CameraController.instance.setCameraOrthographicSizeController(new CameraOrthographicSizeEaseController(_size, _duration));
            else
                CameraController.instance.setCameraFieldOfViewController(new CameraFieldOfViewEaseController(_size, _duration));
        }
        public void resetCameraSize(float _duration)
        {
            if (!isEntered || sceneRefObj == null)
                return;
            
            if (sceneRefObj.camera_setting.isOrthographic)
                CameraController.instance.setCameraOrthographicSizeController(new CameraOrthographicSizeEaseController(sceneRefObj.camera_setting.orthographicSize, _duration));
            else
                CameraController.instance.setCameraFieldOfViewController(new CameraFieldOfViewEaseController(sceneRefObj.camera_setting.cameraFieldOfView, _duration));
        }

        #region 相机相关

        /// <summary>
        /// 变化到指定Size
        /// </summary>
        public void SmoothChgOrthographicSize(float _targetValue, float _duration, float _accTime)
        {
            if (!isShow)
                return;

            CameraController.instance.setCameraOrthographicSizeController(new CameraOrthographicSizeALFadeController(_targetValue, _duration, _accTime));
        }

        /// <summary>
        /// 变化到指定fov
        /// </summary>
        public void SmoothChgFieldOfView(float _targetValue, float _duration, float _accTime, Action _onComplete = null)
        {
            if (!isShow)
                return;

            CameraController.instance.setCameraFieldOfViewController(new CameraFieldOfViewALFadeController(_targetValue, _duration, _accTime, _onComplete));
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        public void SmoothMoveFocusTo(Vector3 _targetPos, float _duration, float _accTime, Action _onComplete = null)
        {
            if (!isShow || sceneRefObj == null)
                return;

            CameraController.instance.setCameraMoveController(new CameraMoveToFocusPointALFadeController(_targetPos, sceneRefObj.move_type, _duration, _accTime,
                () =>
                {
                    //发送完成move_focus的消息
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CITY_MOVE_FOCUS_DONE);
                    });
                    _onComplete?.Invoke();
                }));
        }

        #endregion
        
        protected void showGuideHand(Transform _transform, long _resId)
        {
            if (_transform == null || _resId <= 0)
                return;

            //只展示一个，先清空之前的
            if (_m_guideHand != null)
                _m_guideHand.discard();
            else
                _m_guideHand = new EntryGuideHandPointView();

            _m_guideHand.init(_transform, _resId);
            _m_guideHand.refreshShow();
        }
        /// <summary>
        /// 展示入口手指引导
        /// </summary>
        /// <param name="_objects"></param>
        private void _setShowEntryGuideHand(object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;

            long id = (long) _objects[0];

            long guideHandResId = 0;
            Transform guideTransform = null;
            BuildingRefObj buildingRef = GRefdataCoreMgr.instance.buildingRefCore.getRef(id);
            if (buildingRef == null)
            {
                AnecdotePosRefObj anecdotePosRef = GRefdataCoreMgr.instance.anecdotePosRefCore.getRef(id);
                if (anecdotePosRef != null)
                {
                    guideHandResId = anecdotePosRef.guide_hand_ui_res_id;
                    guideTransform = getAnecdoteTransform(id);
                }
            }
            else
            {
                guideHandResId = buildingRef.guide_hand_ui_res_id;
                guideTransform = getBuildingPosTransform(id);
            }
            
            if (guideTransform == null || guideHandResId == 0)
                return;

            showGuideHand(guideTransform, guideHandResId);
        }
        /// <summary>
        /// 开始引导消息
        /// </summary>
        private void _onStartTutorial()
        {
            //开始引导需要隐藏引导的手
            if (_m_guideHand != null)
                _m_guideHand.discard();
        }
        private void _showGuideHand(object[] _params)
        {
            if (_params is not { Length: > 1 } || _params[0] is not Transform transform || _params[1] is not long resId)
                return;
            
            showGuideHand(transform, resId);
        }
    }
}