using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟主场景
    /// </summary>
    public class MainAdditionGuildTDScene : _ABaseHomeEntryAdditionTDScene
    {
        private static MainAdditionGuildTDScene _g_instance;
        [NotNull] public static MainAdditionGuildTDScene instance { get { return _g_instance ??= new MainAdditionGuildTDScene(); } }

        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;

        private MainAdditionGuildTDScene()
        {
        }

        protected override bool _isNeedCheckScreenClickAni { get { return true; } }
        
        public override bool needDiscardOnSwitch { get { return false; } }


        public override void onSwitchHideScene()
        {
            
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                long sceneId = GRefdataCoreMgr.instance.npGeneral.guild_main_scene_id;
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(sceneId);
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _onRootGOLoadedEx(GameObject _go)
        {
        }

        protected override void _enterSceneAdditionLoad(Action _complete)
        {
            _complete?.Invoke();
        }

        protected override void _dealShowSceneEx(Action _delegate)
        {
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            if (_m_lastQuitRecorded)
            {
                CameraController.instance.cameraPos = _m_lastQuitCameraPos;
                CameraController.instance.cameraFocusPos = _m_lastQuitCameraFocusPos;
            }
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
           //尝试获取当前显示的输入位置，要在重置状态前获取，要不数据可能被改动
           _m_lastQuitRecorded = true;
           _m_lastQuitCameraPos = CameraController.instance.cameraPos;
           _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;

           _delegate?.Invoke();
        }

        protected override void _onSceneInitedEx()
        {
        }

        protected override void _onQuitTDSceneEx()
        {
            _m_lastQuitRecorded = false;
        }

        #region 相机相关

        //返回单位的视野角度
        public Vector3 getActorViewForward()
        {
            return -CameraController.instance.controlCamera.transform.forward;
        }


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