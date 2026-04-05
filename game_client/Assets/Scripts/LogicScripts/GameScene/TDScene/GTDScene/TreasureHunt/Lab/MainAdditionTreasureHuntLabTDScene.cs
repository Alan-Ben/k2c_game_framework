using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionTreasureHuntLabTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] private TreasureHuntLabRefObj _m_rLabRefObj;//实验室配表数据

        private GTDMonoTreasureHuntLab _m_sceneMono;
        private GTDTreasureHuntLabSceneMgr _m_sceneMgr;
     
        //上次退出的相机位置
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;
        
        public MainAdditionTreasureHuntLabTDScene([NotNull] TreasureHuntLabRefObj _labRefObj)
        {
            _m_rLabRefObj = _labRefObj;
        }

        public TreasureHuntLabRefObj labRefObj { get { return _m_rLabRefObj; } }
        public override bool needDiscardOnSwitch { get { return false; } }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(_m_rLabRefObj?.scene_id ?? 0);
                return sceneInfoRefObj;
            }
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if (null == _m_sceneMono)
            {
                _m_sceneMono = _g0.GetComponent<GTDMonoTreasureHuntLab>();
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _enterSceneAdditionLoad(Action _complete)
        {
            if (_m_sceneMono != null)
            {
                _m_sceneMgr = new GTDTreasureHuntLabSceneMgr(_m_sceneMono);
                _m_sceneMgr.init(_complete);
            }
            else
            {
                _complete.Invoke();
            }
        }

        protected override void _onSceneInited()
        {
        }

        protected override void _onQuitTDScene()
        {
            _m_lastQuitRecorded = false;
            
            _m_sceneMgr?.discard();
            _m_sceneMgr = null;
            
            _m_sceneMono = null;
        }
        
        protected override void _dealShowSceneNP(Action _delegate)
        {
            if(sceneRefObj != null)
                CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            if (_m_lastQuitRecorded)
            {
                CameraController.instance.cameraPos = _m_lastQuitCameraPos;
                CameraController.instance.cameraFocusPos = _m_lastQuitCameraFocusPos;
            }
            
            if (_m_sceneMono != null)
            {
                bool labUnlock = _m_rLabRefObj.isUnlock();
                ALUGUICommon.setGameObjEnable(_m_sceneMono.labUnlockShow, labUnlock);
                ALUGUICommon.setGameObjEnable(_m_sceneMono.labLockShow, !labUnlock);
            }
            
            _m_sceneMgr?.show();
            
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            //尝试获取当前显示的输入位置，要在重置状态前获取，要不数据可能被改动
            _m_lastQuitRecorded = true;
            _m_lastQuitCameraPos = CameraController.instance.cameraPos;
            _m_lastQuitCameraFocusPos = CameraController.instance.cameraFocusPos;
            
            _m_sceneMgr?.hide();
            
            _delegate?.Invoke();
        }
        
        public override void onSwitchHideScene()
        {
        }
    }
}