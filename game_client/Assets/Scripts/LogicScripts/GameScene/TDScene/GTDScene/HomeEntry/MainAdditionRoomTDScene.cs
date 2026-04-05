using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 卧室场景
    /// </summary>
    public class MainAdditionRoomTDScene : _ABaseHomeEntryAdditionTDScene
    {
        private static MainAdditionRoomTDScene _g_instance;
        [NotNull] public static MainAdditionRoomTDScene instance { get { return _g_instance ??= new MainAdditionRoomTDScene(); } }

        //场景mono
        private GTDMainRoomMono _m_roomMono;
        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;
        //显示序列
        private long _m_showSerialize;

        private MainAdditionRoomTDScene()
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
                PlayerRoomSkinRefObj currentRoomSkin = NPPlayer.instance.roomSkinComp.currentRoomSkinRefObj;
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(currentRoomSkin?.room_scene_id ?? 0);
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _onRootGOLoadedEx(GameObject _go)
        {
            if (null == _m_roomMono && null != _go)
            {
                _m_roomMono = _go.GetComponent<GTDMainRoomMono>();
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
            
            if (_m_roomMono == null)
            {
                //发送卧室场景进入完成的引导步骤
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ROOM_SCENE_ENTER_DONE);
                _delegate?.Invoke();
            }
            else
            {
                //发送卧室场景进入完成的引导步骤
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ROOM_SCENE_ENTER_DONE);
                _delegate?.Invoke();
            }
        }

        protected override void _dealHideSceneEx(Action _delegate)
        {
           //尝试获取当前显示的输入位置，要在重置状态前获取，要不数据可能被改动
           _m_lastQuitRecorded = true;
           _m_lastQuitCameraPos = CameraController.instance.cameraPos;
           _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
           _m_showSerialize = ALSerializeOpMgr.next();

           _delegate?.Invoke();
        }

        protected override void _onSceneInitedEx()
        {
            if (_m_roomMono == null)
                return;

        }

        protected override void _onQuitTDSceneEx()
        {
            _m_lastQuitRecorded = false;
            _m_roomMono = null;

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
            if (result)
                _m_lastQuitRecorded = false;
            
            return result;
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

        #region 隐藏UI处理

        /// <summary>
        /// 处理效果隐藏UI
        /// </summary>
        /// <param name="_durationSec"></param>
        public void dealEffectHideUI(float _durationSec)
        {
            regShowDone(() =>
            {
                //隐藏UI
                foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
                {
                    if (null == pointView)
                        continue;
                    pointView.hide();
                }
                foreach (KeyValuePair<long, EntryUnlockDescPointView> entryUnlockDescPointView in _m_entryUnlockDescPointViewDic)
                {
                    entryUnlockDescPointView.Value?.hide();
                }

                _m_guideHand?.discard();
                _m_guideHand = null;

                //如果小于0一直隐藏
                if (_durationSec < 0)
                    return;

                //延时显示UI
                _m_showSerialize = ALSerializeOpMgr.next();
                long serialize = _m_showSerialize;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (_m_showSerialize != serialize)
                        return;

                    foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
                    {
                        if (null == pointView)
                            continue;
                        pointView.refreshShow();
                    }

                }, _durationSec);
            });
        }

        #endregion
    }
}