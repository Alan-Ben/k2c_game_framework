using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星场景
    /// </summary>
    public class MainAdditionMarsTDScene : _ABaseHomeEntryAdditionTDScene
    {
        private static MainAdditionMarsTDScene _g_instance;
        [NotNull] public static MainAdditionMarsTDScene instance { get { return _g_instance ??= new MainAdditionMarsTDScene(); } }
        
        private GTDMonoMarsScene _m_sceneMono;
        
        private MarsResidentReplenishView _m_residentReplenishView;//居民移民视图
        private MarsPopularWillCenterView _m_popularWillCenterView;//民意中心视图
        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;
        
        private MainAdditionMarsTDScene()
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
                long sceneId = GRefdataCoreMgr.instance.npGeneral.mars_entry_scene_id;
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(sceneId);
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new MarsInputDealer(_sceneRefObj);
        }

        protected override void _onRootGOLoadedEx(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoMarsScene>();
        }

        protected override void _dealShowSceneEx(Action _delegate)
        {
            WinMsg.RegisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导

            
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
            WinMsg.UnregisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导
            
            _m_lastQuitRecorded = true;
            _m_lastQuitCameraPos = CameraController.instance.cameraPos;
            _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
            
            if (_delegate != null) 
                _delegate();
        }

        protected override void _onSceneInitedEx()
        {
            _m_residentReplenishView = new MarsResidentReplenishView();
            _m_residentReplenishView.init(_m_sceneMono?.monoMarsResidentReplenish);

            _m_popularWillCenterView = new MarsPopularWillCenterView();
            _m_popularWillCenterView.init(_m_sceneMono?.monoMarsPopularWillCenter);
        }

        protected override void _onQuitTDSceneEx()
        {
            _m_residentReplenishView?.discard();
            _m_residentReplenishView = null;
            
            _m_popularWillCenterView?.discard();
            _m_popularWillCenterView = null;
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
                return true;
            }

            bool result = sceneIndex.mainId != curIndex.mainId || sceneIndex.subId != curIndex.subId;
            return result;
        }

        #region 相机相关

        public void focusToPos(Vector3 _position, Vector2 _focusViewportPos, float _focusScale, float _moveTime)
        {
            CameraController.instance.pausePosLimiter();
            setCameraSize(_focusScale, _moveTime);
            
            float currentScale = sceneRefObj.camera_setting.isOrthographic ? 
                CameraController.instance.controlCamera.orthographicSize : 
                CameraController.instance.controlCamera.fieldOfView;
            
            float scaleRatio = _focusScale / currentScale;
            Vector3 target = CameraController.instance.controlCamera.ViewportToWorldPoint(_focusViewportPos);
            Vector3 center = CameraController.instance.controlCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            Vector3 offset = (center - target) * scaleRatio;
            target = _position + offset;
            
            focusToTarget(target, _moveTime);
        }
        public void cancelThePosFocus(float _moveTime)
        {
            if (!isShow)
                return;

            float size;
            SceneInfoRefObj sceneRef = sceneRefObj;
            if (sceneRef.camera_setting.isOrthographic)
                size = sceneRef.camera_setting.orthographicSize;
            else
                size = sceneRef.camera_setting.cameraFieldOfView;
            
            setCameraSize(size, _moveTime);
            CameraController.instance.resumePosLimiter();
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

        #endregion
        
        
        #region Building Management
        
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
                monoTrans.SetParent(_m_sceneMono.unitParent);
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
        
        public Vector3 getBuildingPos(long _buildingId)
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getBuildingPos failed, scene mono is null.");
                return Vector3.zero;
            }
            
            if (_m_sceneMono.buildingPosList == null)
            {
                ALLog.Error("getBuildingPos failed, buildingPosList is null.");
                return Vector3.zero;
            }
            
            foreach (GTDMonoBuildingPos buildingPos in _m_sceneMono.buildingPosList)
            {
                if (buildingPos != null && buildingPos.buildingId == _buildingId)
                {
                    return buildingPos.transform.position;
                }
            }
            
            ALLog.Warning($"getBuildingPos failed, building position not found for id: {_buildingId}");
            return Vector3.zero;
        }
        
        #endregion

        /// <summary>
        /// 聚焦建筑并展示手指引导
        /// </summary>
        public void focusAndShowGuideHand(long _buildingId, float _duration)
        {
            tryGetBuildingPos(_buildingId, out Vector3 _targetPos);
            focusToTarget(_targetPos, _duration);
            // 若在引导中则不展示手指引导
            if (!Game.instance.isInTutorial)
            {
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    //如果不在引导并且条件通过，展示入口手指引导
                    if (!Game.instance.isInTutorial && GRefdataCoreMgr.instance.npGeneral.entrance_show_hand_guide_cond.IsEnable(null))
                        WinMsg.SendMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _buildingId);
                }, _duration);
            }
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
            MarsAllBuildingRefObj buildingRef = GRefdataCoreMgr.instance.marsAllBuildingRefCore.getRef(id);
            if (buildingRef == null)
                return;
            
            guideHandResId = buildingRef.guide_hand_ui_res_id;
            guideTransform = getBuildingPosTransform(id);
            
            if (guideTransform == null || guideHandResId == 0)
                return;

            showGuideHand(guideTransform, guideHandResId);
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
        
        public void playBuildingUpgradeEffect(Vector3 _position, string _sideTipText, MarsBuildingRefObj _buildingRef)
        {
            if (_m_sceneMono == null || null == _buildingRef)
                return;

            PlaySfxMgr.instance.playSfxByPos(_buildingRef.upgrade_sfx_id, _position);
            TipQueueMgr.instance.pauseShowType(ETipQueueType.MARS_POWER);
            ALCommonActionMonoTask.addMonoTask(() => TipQueueMgr.instance.resumeShowType(ETipQueueType.MARS_POWER), _m_sceneMono.marsPowerDelay);
            ALCommonActionMonoTask.addMonoTask(() => NPGUIAddSceneCenterTip.instance.showIconTextTip(_buildingRef.building_icon, _sideTipText, _m_sceneMono.sideTipId), _m_sceneMono.sideTipDelay);
        }
    }
}