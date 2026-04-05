using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子相关入口场景节点
    /// </summary>
    public class MainAdditionHeroBattleEntryTDScene : _ABaseHomeEntryAdditionTDScene
    {
        private static MainAdditionHeroBattleEntryTDScene _g_instance;
        [NotNull] public static MainAdditionHeroBattleEntryTDScene instance { get { return _g_instance ??= new MainAdditionHeroBattleEntryTDScene(); } }
        
        private bool _m_lastQuitRecorded;
        private Vector3 _m_lastQuitCameraPos;
        private Vector3 _m_lastQuitCameraFocusPos;

        public MainAdditionHeroBattleEntryTDScene()
        {
            
        }
        
        public override bool needDiscardOnSwitch { get { return false; } }
        public override void onSwitchHideScene()
        {
            
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.hero_battle_entry_scene_id);
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _onRootGOLoadedEx(GameObject _go)
        {
            
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

    }
}