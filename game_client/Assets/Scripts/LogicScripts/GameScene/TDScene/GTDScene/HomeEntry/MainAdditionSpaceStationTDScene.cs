using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 空间站场景
    /// </summary>
    public class MainAdditionSpaceStationTDScene : _ABaseHomeEntryAdditionTDScene
    {
        private static MainAdditionSpaceStationTDScene _g_instance;
        [NotNull] public static MainAdditionSpaceStationTDScene instance { get { return _g_instance ??= new MainAdditionSpaceStationTDScene(); } }

        private GTDSpaceStationSceneMono _m_sceneMono;

        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;
        
        private MainAdditionSpaceStationTDScene()
        {
        }

        protected override bool _isNeedCheckScreenClickAni { get { return true; } }
        
        public override bool needDiscardOnSwitch
        {
            get
            {
                return checkNeedReload();
            }
        }
        

        public override void onSwitchHideScene()
        {
            
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                //条件符合是二级卧室场景id
                long sceneId = GRefdataCoreMgr.instance.npGeneral.space_station_entry_scene_id;
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(sceneId);
            }
        }

        public void showGos()
        {
            if (_m_sceneMono != null) 
                ALUGUICommon.setGameObjEnable(_m_sceneMono.showGos, true);
        }

        public void hideGos()
        {
            if (_m_sceneMono != null) 
                ALUGUICommon.setGameObjEnable(_m_sceneMono.showGos, false);
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _onRootGOLoadedEx(GameObject _go)
        {
            if (null == _m_sceneMono && _go != null)
            {
                _m_sceneMono = _go.GetComponent<GTDSpaceStationSceneMono>();
            }
        }

        protected override void _dealShowSceneEx(Action _delegate)
        {
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            if (_m_lastQuitRecorded)
            {
                CameraController.instance.cameraPos = _m_lastQuitCameraPos;
                CameraController.instance.cameraFocusPos = _m_lastQuitCameraFocusPos;
            }

            if (_delegate != null) 
                _delegate();
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
            //尝试获取当前显示的输入位置，要在重置状态前获取，要不数据可能被改动
            _m_lastQuitRecorded = true;
            _m_lastQuitCameraPos = CameraController.instance.cameraPos;
            _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
            
            if (_delegate != null) 
                _delegate();
        }

        protected override void _onSceneInitedEx()
        {
           
        }

        protected override void _onQuitTDSceneEx()
        {
            _m_lastQuitRecorded = false;
        }

        public bool checkNeedReload()
        {
            if (!isInited)
                return false;
            
            if (null == sceneRefObj || null == sceneIndex)
                return false;
            
            //判断是否变动了场景，如变动则需要重新加载
            NPGSceneIndex curIndex = sceneRefObj.td_scene_idx;

            if (sceneIndex == null || curIndex == null)
            {
                _m_lastQuitRecorded = false;
                return true;
            }

            bool result = sceneIndex.mainId != curIndex.mainId || sceneIndex.subId != curIndex.subId;
            if (result)
                _m_lastQuitRecorded = false;
            
            return result;
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
    }
}