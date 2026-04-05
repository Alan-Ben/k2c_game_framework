using System;
using ALPackage;
using Common.TravelEnum;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    //游历3d主场景
    public class MainAddtionTravelMainTDScene : _ABasicAdditionMainTDScene
    {
        
        private static MainAddtionTravelMainTDScene _g_instance;
        [NotNull] public static MainAddtionTravelMainTDScene instance { get { return _g_instance ??= new MainAddtionTravelMainTDScene(); } }
        
        private GTDTravelMainSceneMono _m_travelMainSceneMono;

        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;

        private int _m_inputMaskSerialize = -1;
        
        public override bool needDiscardOnSwitch { get => checkNeedReload(); }
        
        protected override void _onSceneInited()
        {
        }

        protected override void _onQuitTDScene()
        {
            _m_travelMainSceneMono = null;
            _m_lastQuitRecorded = false;
        }
        
        public override void onSwitchHideScene()
        {
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if (null == _m_travelMainSceneMono && _g0 != null)
            {
                _m_travelMainSceneMono = _g0.GetComponent<GTDTravelMainSceneMono>();
            }
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.travel_scene_id);
                return sceneInfoRefObj;
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _dealShowSceneNP(Action _delegate)
        {
            if (sceneRefObj == null)
            {
                _delegate?.Invoke();
                return;
            }
            
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            
            //如果有上次退出的相机位置记录，恢复到上次的位置
            if (_m_lastQuitRecorded)
            {
                CameraController.instance.cameraPos = _m_lastQuitCameraPos;
                CameraController.instance.cameraFocusPos = _m_lastQuitCameraFocusPos;
            }
            
            WinMsg.RegisterMsg(WinMsgType.TRAVEL_FOCUS_TO_POS, _msgFocusToPos);
            
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            WinMsg.UnregisterMsg(WinMsgType.TRAVEL_FOCUS_TO_POS, _msgFocusToPos);
            
            //记录当前相机位置，下次显示时恢复
            _m_lastQuitRecorded = true;
            _m_lastQuitCameraPos = CameraController.instance.cameraPos;
            _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
            
            _delegate?.Invoke();
        }

        public bool checkNeedReload()
        {
            if (!isInited)
                return false;
            
            SceneInfoRefObj needShowSceneRefObj = sceneRefObj;
            if (null == needShowSceneRefObj || null == sceneIndex)
                return false;
            
            //判断是否变动了场景，如变动则需要重新加载
            NPGSceneIndex curIndex = needShowSceneRefObj.td_scene_idx;

            if (sceneIndex == null || curIndex == null)
            {
                _m_lastQuitRecorded = false;
                return true;
            }

            bool result = sceneIndex.mainId != curIndex.mainId || sceneIndex.subId != curIndex.subId;
            //场景变动时清除相机位置记录
            if (result)
                _m_lastQuitRecorded = false;
            
            return result;
        }
        
        public GTDTravelMainPosItemInfo getTravelPosInfo(long _posId)
        {
            if(_m_travelMainSceneMono == null || _m_travelMainSceneMono.travelPosList == null)
                return null;

            foreach (var posItemInfo in _m_travelMainSceneMono.travelPosList)
            {
                if(posItemInfo != null && posItemInfo.posId == _posId)
                    return posItemInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取卧室位置的Transform
        /// </summary>
        /// <returns></returns>
        public Transform getRoomPosTrans()
        {
            if (_m_travelMainSceneMono == null)
                return null;

            return _m_travelMainSceneMono.roomTrans;
        }
        
        public GTravelAircraftView createTravelAircraftView()
        {
            if (_m_travelMainSceneMono == null)
                return null;

            GTravelAircraftView aircraftView = new GTravelAircraftView(_m_travelMainSceneMono.aircraftGoIndex, _m_travelMainSceneMono.aircraftParent);
            return aircraftView;
        }

        public void focusToPos(long _posId, Action _onFocusDone = null)
        {
            if (!isEntered)
            {
                _onFocusDone?.Invoke();
                return;
            }
            
            GTDTravelMainPosItemInfo posInfo = getTravelPosInfo(_posId);
            if (posInfo != null && posInfo.loadParent != null)
            {
                _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                focusToTarget(posInfo.loadParent.position, _m_travelMainSceneMono?.focusToPosDuration ?? 0.5f, ()=>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                    _onFocusDone?.Invoke();
                });
            }
            else
            {
                _onFocusDone?.Invoke();   
            }
        }
        
        /// <summary>
        /// 聚焦到某个地点位置
        /// </summary>
        public void focusToPos(long _posId, float _duration, Action _onFocusDone = null)
        {
            if (!isEntered)
            {
                _onFocusDone?.Invoke();
                return;
            }
            
            GTDTravelMainPosItemInfo posInfo = getTravelPosInfo(_posId);
            if (posInfo != null && posInfo.loadParent != null)
            {
                _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                focusToTarget(posInfo.loadParent.position, _duration, ()=>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                    _onFocusDone?.Invoke();
                });
            }
            else
            {
                _onFocusDone?.Invoke();   
            }
        }

        public override void focusToTarget(Vector3 _targetPos, float _duration, Action _complete = null)
        {
            if (sceneRefObj == null)
            {
                _complete?.Invoke();
                return;
            }

            if (!isEntered)
            {
                _complete?.Invoke();
                return;
            }
            
            CameraController.instance.setCameraMoveController(new CameraMoveToFocusPointEaseControllerEx(_targetPos, sceneRefObj.move_type, _duration, _complete));
            CameraController.instance.frameCheck();
        }

        /// <summary>
        /// 聚焦到主城位置
        /// </summary>
        public void focusToMainCity(float _duration, Action _onFocusDone = null)
        {
            if (!isEntered)
            {
                _onFocusDone?.Invoke();
                return;
            }
            
            Transform mainCityTrans = getRoomPosTrans();
            if (mainCityTrans != null)
            {
                _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                focusToTarget(mainCityTrans.position, _duration, ()=>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                    _onFocusDone?.Invoke();
                });
            }
            else
            {
                _onFocusDone?.Invoke();   
            }
        }
        
        /// <summary>
        /// 聚焦到主城位置
        /// </summary>
        public void focusToMainCity(Action _onFocusDone = null)
        {
            if (!isEntered)
            {
                _onFocusDone?.Invoke();
                return;
            }
            
            Transform mainCityTrans = getRoomPosTrans();
            if (mainCityTrans != null)
            {
                _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                focusToTarget(mainCityTrans.position, _m_travelMainSceneMono?.focusToPosDuration ?? 0.5f, ()=>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
                    _onFocusDone?.Invoke();
                });
            }
            else
            {
                _onFocusDone?.Invoke();   
            }
        }

        #region 消息监听

        private void _msgFocusToPos(params object[] _params)
        {
            if(_params == null || _params.Length < 1 || !(_params[0] is long _posId))
                return;

            if (_params.Length > 1)
            {
                if(_params[1] is float durationFloat)
                    focusToPos(_posId, durationFloat);
                else if(_params[1] is int durationInt)
                    focusToPos(_posId, durationInt);
                else if (_params[1] is double durationDouble)
                    focusToPos(_posId, (float)durationDouble);
                else if(_params[1] is long durationLong)
                    focusToPos(_posId, durationLong);
                else
                    focusToPos(_posId);
            }
            else
                focusToPos(_posId);
        }

        #endregion
    }
}