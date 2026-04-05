using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.EventSystems;

namespace GOE
{
    /***********************
     * 平台资源的基础3D视图对象
     **/
    public abstract class _ABasicTDScene : _AALBasicTopContainerScene
    {
        private BasicResIndexInfo _m_siSceneIndex;
        /** 过程函数 */
        private Action<float> _m_dProcessAction;
        /** 对应的场景对象 */
        private ALSceneInfo _m_siSceneInfo;

        protected _ABasicTDScene()
            : base((int)ENPSceneType.TD_SCENE)
        {
            _m_siSceneIndex = null;
            _m_dProcessAction = null;
            _m_siSceneInfo = null;
        }

        public BasicResIndexInfo sceneIndex { get { return _m_siSceneIndex; } }

        protected override void _onEnterScene()
        {
            //判断场景数据是否有效
            if(null == _m_siSceneIndex || (0 == _m_siSceneIndex.mainId && 0 == _m_siSceneIndex.subId))
            {
                //场景数据无效则当做不需要加载，设置加载完毕
                setSceneInited();
                return;
            }

            //开始进行资源加载
#if UNITY_EDITOR
            ALLocalResLoaderMgr.instance.loadSceneAsset(_resourceCore, _m_siSceneIndex.assetPath, _assetDownloadedDelegate);
#else
        _resourceCore.loadAsset(_m_siSceneIndex.assetPath, _assetDownloadedDelegate, null);
#endif
        }

        protected override void _dealQuitScene()
        {
            //此时需要重置场景索引
            _m_siSceneIndex = null;
            //释放场景对象
            if(null != _m_siSceneInfo)
                _m_siSceneInfo.discard();
            _m_siSceneInfo = null;

            //退出处理
            _onQuitTDScene();
        }

        /***************
         * 进入对应场景的处理函数
         **/
        public void enterScene(BasicResIndexInfo _sceneIndex, Action _initedDelegate)
        {
            enterScene(_sceneIndex, null, _initedDelegate);
        }
        public void enterScene(BasicResIndexInfo _sceneIndex, Action<float> _processAction, Action _initedDelegate)
        {
            if(null == _sceneIndex)
            {
                UnityEngine.Debug.LogError("load scene null!");
                if(null != _initedDelegate)
                    _initedDelegate();

                return;
            }

            if(isEntered)
            {
                if(_m_siSceneIndex != null && _m_siSceneIndex.mainId == _sceneIndex.mainId && _m_siSceneIndex.subId == _sceneIndex.subId)
                {
                    if(null != _initedDelegate)
                        _initedDelegate();

                    return;
                }

                //先退出场景
                _quitScene();
            }

            //设置新的场景
            _m_siSceneIndex = (BasicResIndexInfo)_sceneIndex.Clone();
            _m_siSceneIndex.mainId = _sceneIndex.mainId;
            _m_siSceneIndex.subId = _sceneIndex.subId;

            //设置过程回调
            _m_dProcessAction = _processAction;

            //注册完成回调
            regInitDelegate(_initedDelegate);

            //调用进入函数
            enterScene();
        }


        /*********************
         * 在资源加载完后调用的函数
         **/
        protected void _assetDownloadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            //开启场景切换
            ALSceneMgr.instance.loadSingleScene(_m_siSceneIndex.objName, _m_dProcessAction, _onSceneLoaded);
        }

        /*********************
         * 在场景加载完成后调用的函数
         **/
        protected void _onSceneLoaded(ALSceneInfo _sceneInfo)
        {
            _m_siSceneInfo = _sceneInfo;

            //关闭camera
            if(null != _m_siSceneInfo)
            {
                //将根节点摄像头关闭
                GameObject[] rootGos = _m_siSceneInfo.sceneObj.GetRootGameObjects();
                if(null != rootGos)
                {
                    for(int i = 0; i < rootGos.Length; i++)
                    {
                        GameObject go = rootGos[i];
                        if(null == go)
                            continue;

                        Camera camera = go.GetComponent<Camera>();
                        if(null != camera)
                        {
                            //设置本对象无效
                            go.SetActive(false);
                        }

                        EventSystem eventSys = go.GetComponent<EventSystem>();
                        if(null != eventSys)
                        {
                            //设置本对象无效
                            go.SetActive(false);
                        }
                    }
                }
            }

            //重置回调
            _m_dProcessAction = null;

            //设置加载完毕
            setSceneInited();
        }

        /** 加载使用的资源管理对象 */
        protected abstract _AALResourceCore _resourceCore { get; }

        /** 推出场景的函数 */
        protected abstract void _onQuitTDScene();
    }
}
