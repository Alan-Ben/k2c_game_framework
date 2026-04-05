using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace GOE
{
    /***********************
     * 平台资源的基础3D视图对象
     **/
    public abstract class _ABasicAdditionTDScene : _AALBasicSubContainerScene_NoChild
    {
        private BasicResIndexInfo _m_siSceneIndex;
        /** 过程函数 */
        private Action<float> _m_dProcessAction;
        /** 对应的场景对象 */
        private ALSceneInfo _m_siSceneInfo;
        
        private long _m_lSerialize;

        public _ABasicAdditionTDScene()
            : base((int)ENPSceneType.TD_SCENE)
        {
            _m_siSceneIndex = null;
            _m_dProcessAction = null;
            _m_siSceneInfo = null;
            
            _m_lSerialize = ALSerializeOpMgr.next();
        }

        public BasicResIndexInfo sceneIndex { get { return _m_siSceneIndex; } }
        /// <summary>
        /// 附加场景加载的方式（不断新建/不再重复）
        /// </summary>
        protected ALSceneMgr.EALSceneAddLoadType _addTDSceneLoadType { get { return ALSceneMgr.EALSceneAddLoadType.KEEP_SINGLE; } }

        protected override void _onEnterScene()
        {
            // 如果没有指定场景index，尝试加入默认场景
            if(null == _m_siSceneIndex)
                _m_siSceneIndex = _getDefaultSceneIndex();
            
            //判断场景数据是否有效
            if(null == _m_siSceneIndex || (0 == _m_siSceneIndex.mainId && 0 == _m_siSceneIndex.subId))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("load scene 0-0!");
#endif
                //直接设置完成
                _onSceneLoaded(null);

                return;
            }
            
            _m_lSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lSerialize;

            //开始进行资源加载
#if UNITY_EDITOR
            ALLocalResLoaderMgr.instance.loadSceneAsset(_resourceCore, _m_siSceneIndex.assetPath, _assetDownloadedDelegate);
#else
            _resourceCore.loadAsset(_m_siSceneIndex.assetPath, _assetDownloadedDelegate, null);
#endif
            //在资源加载完后调用的函数
            void _assetDownloadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
            {
                //防止同时enter然后quit，回调延迟执行，实际上已经quit了，那场景应该销毁，先用isEntered判断，还有问题可以加序列号判断处理
                if(serialize != _m_lSerialize)
                    return;
                
                if(null == _m_siSceneIndex)
                    return;
                
                //开启场景切换
                ALSceneMgr.instance.loadAddScene(_m_siSceneIndex.objName, _m_dProcessAction, (_sceneInfo) =>
                {
                    //防止同时enter然后quit，回调延迟执行，实际上已经quit了，那场景应该销毁，先用isEntered判断，还有问题可以加序列号判断处理
                    if(serialize != _m_lSerialize)
                    {
                        if (null != _sceneInfo)
                            _sceneInfo.discard();
                        return;
                    }
                    
                    _onSceneLoaded(_sceneInfo);
                }, _addTDSceneLoadType);
            }
            
        }

        protected sealed override void _dealQuitScene()
        {
            //序列号增加
            _m_lSerialize = ALSerializeOpMgr.next();

            //此时需要重置场景索引
            _m_siSceneIndex = null;
            //释放场景对象
            if(null != _m_siSceneInfo)
                _m_siSceneInfo.discard();
            _m_siSceneInfo = null;

            //调用退出函数
            _onQuitTDScene();
        }

        /***************
         * 进入对应场景的处理函数
         **/
        public void enterScene(BasicResIndexInfo _sceneIndex)
        {
            enterScene(_sceneIndex, null, null);
        }
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
                        _rootGOLoaded(go);
                    }
                }
            }

            //重置回调
            _m_dProcessAction = null;

            //设置加载完毕
            _enterSceneAdditionLoad(setSceneInited);
        }
        /// <summary>
        /// 对加载的场景每一个rootGo进行处理
        /// </summary>
        private void _rootGOLoaded([NotNull] GameObject _go)
        {
            Camera camera = _go.GetComponent<Camera>();
            if (null != camera)
            {
                //设置本对象无效
                ALUGUICommon.setGameObjDisable(_go);
            }

            EventSystem eventSys = _go.GetComponent<EventSystem>();
            if (null != eventSys)
            {
                //设置本对象无效
                ALUGUICommon.setGameObjDisable(_go);
            }
            
            Light light = _go.GetComponent<Light>();
            if (null != light)
            {
                //设置本对象无效
                ALUGUICommon.setGameObjDisable(_go);
            }
            _onRootGOLoaded(_go);
        }

        /** 加载使用的资源管理对象 */
        protected abstract _AALResourceCore _resourceCore { get; }
        /** 推出场景的函数 */
        protected abstract void _onQuitTDScene();
        /** 对加载的场景每一个rootGo进行处理 */
        protected abstract void _onRootGOLoaded(GameObject _g0);
        /** 默认进入的场景 */
        protected virtual BasicResIndexInfo _getDefaultSceneIndex() { return null; }
        /** 加载场景时额外的加载内容 */
        protected virtual void _enterSceneAdditionLoad([NotNull] Action _complete) { _complete.Invoke(); }
    }
}
