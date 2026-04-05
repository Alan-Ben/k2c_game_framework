using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ALPackage
{
    /*****************
     * 下载后管理的资源信息对象
     **/
    public class ALAssetBundleObj
    {
        private ALAssetBundleResourceMgr _m_arParentMgr;

        /** 在父集合中的索引 */
        private string _m_sIdx;
        /** 实际资源下载对象 */
        private AssetBundle _m_abAssetBundle;
        /** 是否www加载的资源 */
        private bool _m_bIsWWWAssetBundle;
        /** 加载中的统计函数 */
        private int _m_iLoadingOpCount;

        /** 已经下载的资源信息对象 */
        private ALAssetBundleLoadedObj _m_loAssetLoadedObj;

        public ALAssetBundleObj(ALAssetBundleResourceMgr _parent, string _idx, AssetBundle _assetBundle, bool _isWWWAssetBundle)
        {
            _m_arParentMgr = _parent;

            //_m_bReleased = false;
            _m_sIdx = _idx;
            _m_abAssetBundle = _assetBundle;
            _m_iLoadingOpCount = 0;
            _m_bIsWWWAssetBundle = _isWWWAssetBundle;

            _m_loAssetLoadedObj = new ALAssetBundleLoadedObj(this);
        }

        public ALAssetBundleResourceMgr parent { get { return _m_arParentMgr; } }
        public string idx { get { return _m_sIdx; } }
        public ALAssetBundleLoadedObj loadedInfoObj { get { return _m_loAssetLoadedObj; } }
        //如为非www资源则直接返回1，表示一直都在加载，不需要释放资源
        public int loadingOpCount { get { return _m_iLoadingOpCount; } }
        public AssetBundle assetBundle { get { return _m_abAssetBundle; } }

        /****************
         * 从本资源对象中获取指定名称的对象
         **/
        public UnityEngine.Object load(string _name)
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset: {_name}, {_m_iLoadingOpCount}");
            }
            return _m_abAssetBundle.LoadAsset(_name);
        }
        public T load<T>(string _name) where T : UnityEngine.Object
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset<T>: {_name}, {_m_iLoadingOpCount}");
            }
            return _m_abAssetBundle.LoadAsset<T>(_name);
        }
        public UnityEngine.Object load(string _name, Type _type)
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset type: {_name}, {_m_iLoadingOpCount}");
            }
            return _m_abAssetBundle.LoadAsset(_name, _type);
        }

        /****************
         * 异步加载函数
         **/
        public void loadAsyn(string _name, Action<UnityEngine.Object> _delegate)
        {
            //增加异步加载统计
            _m_iLoadingOpCount++;

            //调用加载函数
            AssetBundleRequest loadRequest = _m_abAssetBundle.LoadAssetAsync(_name, typeof(UnityEngine.Object));
            if(null == loadRequest)
            {
                _m_iLoadingOpCount--;
                if (null != _delegate)
                    _delegate(null);
                return;
            }
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset async: {_name}, {_m_iLoadingOpCount}");
                loadRequest.completed += _operation =>
                {
                    UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset async done: {_name}, {_m_iLoadingOpCount}");
                };
            }

            //开启异步加载后续处理
            ALAssetBundleAsynMonitor monitor = new ALAssetBundleAsynMonitor(this, loadRequest, _delegate);
            ALCoroutineDealerMgr.instance.addCoroutine(monitor);
        }
        /*******************
         * 加载对应的附加场景
         **/
        public void loadScene(string _sceneName, Action _loadedDelegate)
        {
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Scene: {_sceneName}");
            }
            //同步加载场景
            SceneManager.LoadScene(_sceneName);
            
            //调用回调
            if (null != _loadedDelegate)
                _loadedDelegate();
        }
        /*******************
         * 加载对应的附加场景
         **/
        public void loadAsynScene(string _sceneName, Action<float> _processDelegate, Action _loadedDelegate)
        {
            //增加加载统计对象
            _m_iLoadingOpCount++;
            //加载场景，并开启监控进行处理
            AsyncOperation asynOp = SceneManager.LoadSceneAsync(_sceneName);
            if (null == asynOp)
            {
                //如果加载失败则直接减少加载统计，并调用完成回调
                _m_iLoadingOpCount--;
                if (null != _loadedDelegate)
                    _loadedDelegate();
                return;
            }
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Scene async: {_sceneName}, {_m_iLoadingOpCount}");
                asynOp.completed += _operation =>
                {
                    UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Scene async done: {_sceneName}, {_m_iLoadingOpCount}");
                };
            }

            //开启监控对象
            ALAssetBundleSceneAsynMonoitor monitor = new ALAssetBundleSceneAsynMonoitor(this, asynOp, _processDelegate, _loadedDelegate);
            monitor.startMonitor();
        }

        /***************
         * 增加正在操作的统计操作函数
         **/
        public void addLoadingOpCount()
        {
            _m_iLoadingOpCount++;
        }
        public void onLoadingOpDone()
        {
            //减少统计
            _m_iLoadingOpCount--;
            //如果是非www资源则不需要刷新时间标记，如果异步统计还是大于0则不需要刷新加载标记
            if (_m_iLoadingOpCount > 0)
                return;

            //刷新本对象的处理时间
            ALAssetBundleLoadedMgr.instance.refreshLoadedObj(_m_loAssetLoadedObj);
        }

        /****************
         * 加载所有本资源中的对象
         **/
        public UnityEngine.Object[] loadAllObjs()
        {
            UnityEngine.Object[] objArr = _m_abAssetBundle.LoadAllAssets();

            return objArr;
        }

        /******************
         * 将本资源对象转变为静态对象
         **/
        public void staticAssetBundle()
        {
            _m_iLoadingOpCount++;
        }

        /*********************
         * 主动释放资源的函数
         **/
        public void discard()
        {
            //从父容器注销
            parent.unregAssetBundle(idx);

            if (null != _m_abAssetBundle)
                _m_abAssetBundle.Unload(true);
            _m_abAssetBundle = null;
        }

        /********************
         * 释放本资源对象
         **/
        protected internal void _unload()
        {
            //判断资源是否www加载方式，非www加载方式不进行下面的释放操作
            //经测试，本地文件加载的assetbundle基本不占用内存，不需要反复的创建和释放
            if (!_m_bIsWWWAssetBundle)
                return;

            //从父容器注销
            parent.unregAssetBundle(idx);

            if (null != _m_abAssetBundle)
                _m_abAssetBundle.Unload(false);
            _m_abAssetBundle = null;
        }
    }
}
