using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    //宴会3d主场景
    public class MainAddtionDinnerMainTDScene : _ABasicAdditionMainTDScene
    {
        
        // private static MainAddtionDinnerMainTDScene _g_instance;
        // [NotNull] public static MainAddtionDinnerMainTDScene instance { get { return _g_instance ??= new MainAddtionDinnerMainTDScene(); } }
        
        private GTDDinnerMainSceneMono _m_sceneMono;
        private GDinnerInfo _m_dinnerInfo;

        public MainAddtionDinnerMainTDScene()
        {
        }

        public override bool needDiscardOnSwitch { get => true; }
        
        protected override void _onSceneInited()
        {
            if (null != _m_sceneMono)
            {
                GTDDinnerMainSceneMgr.instance.init(_m_sceneMono, _m_dinnerInfo);
            }
        }

        protected override void _onQuitTDScene()
        {
            GTDDinnerMainSceneMgr.instance.discard();
        }
        
        public override void onSwitchHideScene()
        {
        }

        protected override void _onRootGOLoaded(GameObject _g0)
        {
            if (null == _m_sceneMono)
            {
                _m_sceneMono = _g0.GetComponent<GTDDinnerMainSceneMono>();
            }
        }

        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(_m_dinnerInfo.dinnerTypeRef.scene_id);
                return sceneInfoRefObj;
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }

        protected override void _dealShowSceneNP(Action _delegate)
        {
            GTDDinnerMainSceneMgr.instance.show();
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            
            _delegate?.Invoke();
        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            GTDDinnerMainSceneMgr.instance.hideAll();
            
            _delegate?.Invoke();
        }

        public void setInfo(GDinnerInfo _dinnerInfo)
        {
            _m_dinnerInfo = _dinnerInfo;
        }
    }
}