using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 测试场景类
    /// </summary>
    public class GTDHotfixScene_Demo : _AGTDHotfixAdditionMainScene<GTDHotfixScene_DemoMono>
    {
        private static GTDHotfixScene_Demo _g_instance;
        public static GTDHotfixScene_Demo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GTDHotfixScene_Demo();

                return _g_instance;
            }
        }
        
        //是否在切换的时候会被释放
        public override bool needDiscardOnSwitch { get { return true; } }
        
        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(10201);
            }
        }

        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }
        
        protected override void _onSceneInited()
        {
            Debug.LogError($"=====NPGTDHotfixScene_Demo===_onSceneInited");
            
            if(null == mono)
                return;
            
            Debug.LogError(mono.floatTest);
        }

        public override void onSwitchHideScene()
        {
            Debug.LogError($"=====NPGTDHotfixScene_Demo===onSwitchHideScene");
        }

        protected override void _dealShowSceneNP(Action _delegate)
        {
            Debug.LogError($"=====NPGTDHotfixScene_Demo===_dealShowSceneNP");

            //设置摄像头信息
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            
            _delegate?.Invoke();

        }

        protected override void _dealHideSceneNP(Action _delegate)
        {
            Debug.LogError($"=====NPGTDHotfixScene_Demo===_dealHideSceneNP");
            
            _delegate?.Invoke();
        }

        protected override void _onQuitTDSceneHotfix()
        {
            Debug.LogError($"=====NPGTDHotfixScene_Demo===_onQuitTDSceneHotfix");
        }
    }
}