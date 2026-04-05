using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    // 杰出者大厅主界面3d主场景
    public class MainAdditionGraveMainTDScene : _ABasicAdditionMainTDScene
    {
        private static MainAdditionGraveMainTDScene _g_instance;
        [NotNull] public static MainAdditionGraveMainTDScene instance { get { return _g_instance ??= new MainAdditionGraveMainTDScene(); } }
        
        private GTDGraveMainSceneMono _m_sceneMono;
        [NotNull] private GTDGraveMainMgr _m_sceneMgr;

        private long _m_graveMainId = 1;
        private List<Common.GraveObj.GraveObj_NewInfo> _m_newInfoList;
        public MainAdditionGraveMainTDScene()
        {
            _m_sceneMgr = new GTDGraveMainMgr();
        }

        public override bool needDiscardOnSwitch { get => true; }
        
        protected override void _onSceneInited()
        {
        }

        protected override void _onQuitTDScene()
        {
        }
        
        public override void onSwitchHideScene()
        {
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if (null == _m_sceneMono)
            {
                _m_sceneMono = _g0.GetComponent<GTDGraveMainSceneMono>();
            }
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(2040);
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
            
            if (_m_sceneMono != null)
            {
                if (_m_sceneMono.canvas != null)
                    _m_sceneMono.canvas.worldCamera = CameraController.instance.controlCamera;
                _m_sceneMgr.init(_m_sceneMono, _m_graveMainId, _m_newInfoList);
            }
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            _m_sceneMgr.discard();
            _delegate?.Invoke();
        }

        public void initData(Action _onInitDone)
        {
            NPPlayer.instance.graveComp.reqGraveNewInfoList(_list =>
            {
                long mainId = 0;
                // 默认进入有数据的最高场景
                foreach (GraveMainRefObj graveMainRef in GRefdataCoreMgr.instance.graveMainRefCore.refList)
                {
                    if (graveMainRef == null) continue;
                    bool canShow = graveMainRef.show_condition == null || graveMainRef.show_condition.isEmpty ||
                                   graveMainRef.show_condition.IsEnable(null);

                    if (canShow)
                    {
                        bool hasPlayer = false;
                        foreach (long type_id in graveMainRef.type_id_list)
                        {
                            var typeRefObj = GRefdataCoreMgr.instance.graveTypeRefCore.getRef(type_id);
                            foreach (long titleId in typeRefObj.player_title_id_list)
                            {
                                foreach (var newInfo in _list)
                                {
                                    if (newInfo == null) continue;
                                    if (titleId == newInfo.getTitleId())
                                    {
                                        hasPlayer = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (mainId == 0 || hasPlayer)
                            mainId = graveMainRef.id;
                    }
                }
                
                _m_graveMainId = mainId;
                _m_newInfoList = _list;
                _onInitDone?.Invoke();
            });
        }

        public bool hasNextGraveMain()
        {
            for (var i = 0; i < GRefdataCoreMgr.instance.graveMainRefCore.refList.Count; i++)
            {
                var graveMainRef = GRefdataCoreMgr.instance.graveMainRefCore.refList[i];
                if (graveMainRef == null) continue;
                bool canShow = graveMainRef.show_condition == null || graveMainRef.show_condition.isEmpty ||
                               graveMainRef.show_condition.IsEnable(null);
                if (canShow)
                {
                    if (_m_graveMainId < graveMainRef.id)
                        return true;
                }
            }

            return false;
        }

        public bool hasPreGraveMain()
        {
            for (var i = GRefdataCoreMgr.instance.graveMainRefCore.refList.Count - 1; i >= 0; i--)
            {
                var graveMainRef = GRefdataCoreMgr.instance.graveMainRefCore.refList[i];
                if (graveMainRef == null) continue;
                bool canShow = graveMainRef.show_condition == null || graveMainRef.show_condition.isEmpty ||
                               graveMainRef.show_condition.IsEnable(null);
                if (canShow)
                    if (_m_graveMainId > graveMainRef.id )
                        return true;
            }
            return false;
        }
        
        public void changePreGraveMain()
        {
            for (var i = GRefdataCoreMgr.instance.graveMainRefCore.refList.Count - 1; i >= 0; i--)
            {
                var graveMainRef = GRefdataCoreMgr.instance.graveMainRefCore.refList[i];
                if (graveMainRef == null) continue;
                bool canShow = graveMainRef.show_condition == null || graveMainRef.show_condition.isEmpty ||
                               graveMainRef.show_condition.IsEnable(null);
                if (canShow)
                {
                    if (_m_graveMainId  > graveMainRef.id)
                    {
                        _m_graveMainId = graveMainRef.id;
                        _m_sceneMgr.changeGraveMain(_m_graveMainId);
                        return;
                    }
                }
            }
        }

        public void changeNextGraveMain()
        {
            for (var i = 0; i < GRefdataCoreMgr.instance.graveMainRefCore.refList.Count; i++)
            {
                var graveMainRef = GRefdataCoreMgr.instance.graveMainRefCore.refList[i];
                if (graveMainRef == null) continue;
                bool canShow = graveMainRef.show_condition == null || graveMainRef.show_condition.isEmpty ||
                               graveMainRef.show_condition.IsEnable(null);
                if (canShow)
                {
                    if (_m_graveMainId < graveMainRef.id)
                    {
                        _m_graveMainId = graveMainRef.id;
                        _m_sceneMgr.changeGraveMain(_m_graveMainId);
                        return;
                    }
                }
            }
        }

        public void setInfo()
        {
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
                    // //发送完成move_focus的消息
                    // ALCommonActionMonoTask.addMonoTask(() =>
                    // {
                    //     WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CITY_MOVE_FOCUS_DONE);
                    // });
                    _onComplete?.Invoke();
                }));
        }

        #endregion
    }
}