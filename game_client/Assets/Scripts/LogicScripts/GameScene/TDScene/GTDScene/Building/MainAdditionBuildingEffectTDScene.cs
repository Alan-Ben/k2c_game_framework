using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionBuildingEffectTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionBuildingEffectTDScene instance { get { return _g_instance ??= new MainAdditionBuildingEffectTDScene(); } }
        private static MainAdditionBuildingEffectTDScene _g_instance;

        private GTDMonoBuildingEffectScene _m_sceneMono;


        public override bool needDiscardOnSwitch { get { return true; } }
        public override SceneInfoRefObj sceneRefObj { get { return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.building_effect_scene_id); } }

        public Vector3 cameraEndPos { get { return _m_sceneMono != null && _m_sceneMono.cameraEnd != null ? _m_sceneMono.cameraEnd.position : Vector3.zero; } }
        public float cameraDuration { get { return _m_sceneMono != null ? _m_sceneMono.cameraDuration : 0; } }


        public void playAnimation(Action _complete)
        {
            if (_m_sceneMono != null && _m_sceneMono.anim != null)
                _m_sceneMono.anim.Play(_m_sceneMono.animName, _complete);
            else
                _complete?.Invoke();
        }


        protected override void _onSceneInited()
        {
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onQuitTDScene()
        {
            _m_sceneMono = null;
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoBuildingEffectScene>();
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
    }
}
