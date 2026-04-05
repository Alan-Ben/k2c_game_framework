using System;
using System.Threading;
using System.Collections.Generic;

using System.IO;
using UnityEngine;

/*****************************
 * 游戏中管理远程资源，本地资源以及版本信息等同步的管理对象
 **/
namespace ALPackage
{
    public abstract class _AALResourceCore
    {
        private const int _c_g_iDefaultRetryCount = 3;

        /** 远程（从指定URL下载到PersistenceDataPath）加载资源的加载对象 */
        protected _AALBasicAssetBundleLoadMgr _m_lmRemoteResLoadMgr;

        /** 本地补丁信息的保存处理对象 */
        protected ALLocalPatchInfo _m_piPatchInfo;
        /** 本地（StreamingAsset里）资源在打包目录的资源加载对象 */
        protected _AALBasicAssetBundleLoadMgr _m_lmLocalResLoadMgr;

        /** 本对象初始化的步骤统计对象 */
        protected ALStepCounter _m_scInitStepCounter;
        /** 本对象远程加载处理对象 */
        protected ALHttpDownloadMgr _m_dmHttpDownloadMgr;

        /** 依赖关系是否初始化 */
        private bool _m_bIsDependenceInit;
        /** 依赖关系加载处理 */
        protected ALResABDependenceMgr _m_dmDependenceMgr;
        /** 重试次数 */
        private int _m_iRetryCount = 3;

        //创建单纯从远程下载资源的处理对象
        public _AALResourceCore(string _remoteURL, bool _useCache)
        {
            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(_remoteURL), _useCache);

            _m_piPatchInfo = null;
            _m_lmLocalResLoadMgr = null;

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        public _AALResourceCore(_IALAssetBundleLoadPathProvider _remoteURLProvider, bool _useCache)
        {
            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, _remoteURLProvider, _useCache);

            _m_piPatchInfo = null;
            _m_lmLocalResLoadMgr = null;

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        //创建单纯从远程下载资源的处理对象
        public _AALResourceCore(string _remoteURL, int _maxLoadingCount, int _maxRetryCount, bool _useCache)
        {
            _m_iRetryCount = _maxRetryCount;

            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_maxLoadingCount, _m_iRetryCount, new ALABStringLoadPathProvider(_remoteURL), _useCache);

            _m_piPatchInfo = null;
            _m_lmLocalResLoadMgr = null;

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        public _AALResourceCore(_IALAssetBundleLoadPathProvider _remoteURLProvider, int _maxLoadingCount, int _maxRetryCount, bool _useCache)
        {
            _m_iRetryCount = _maxRetryCount;

            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_maxLoadingCount, _m_iRetryCount, _remoteURLProvider, _useCache);

            _m_piPatchInfo = null;
            _m_lmLocalResLoadMgr = null;

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALResourceCore(string _remoteURL, string _localPatchFolerPath)
        {
            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(_remoteURL), false);

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        public _AALResourceCore(_IALAssetBundleLoadPathProvider _remoteURLProvider, string _localPatchFolerPath)
        {
            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, _remoteURLProvider, false);

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALResourceCore(string _remoteURL, string _localPatchFolerPath, int _maxLoadingCount, int _maxRetryCount)
        {
            _m_iRetryCount = _maxRetryCount;

            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_maxLoadingCount, _maxRetryCount, new ALABStringLoadPathProvider(_remoteURL), false);

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        public _AALResourceCore(_IALAssetBundleLoadPathProvider _remoteURLProvider, string _localPatchFolerPath, int _maxLoadingCount, int _maxRetryCount)
        {
            _m_iRetryCount = _maxRetryCount;

            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_maxLoadingCount, _maxRetryCount, _remoteURLProvider, false);

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        //创建仅有本地资源处理对象的数据处理对象
        public _AALResourceCore(string _localPatchFolerPath)
        {
            _m_lmRemoteResLoadMgr = null;

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }
        public _AALResourceCore(string _localPatchFolerPath, int _maxRetryCount)
        {
            _m_iRetryCount = _maxRetryCount;

            _m_lmRemoteResLoadMgr = null;

            _m_piPatchInfo = new ALLocalPatchInfo(_localPatchFolerPath);
            //创建本地资源包打包资源加载对象
            _m_lmLocalResLoadMgr = new ALLocalAssetBundleLoadMgr(_c_g_iDefaultRetryCount, _m_iRetryCount, new ALABStringLoadPathProvider(Application.streamingAssetsPath + "/" + _localPatchFolerPath), false);

            _m_scInitStepCounter = null;
            _m_dmHttpDownloadMgr = null;

            _m_bIsDependenceInit = false;
            _m_dmDependenceMgr = new ALResABDependenceMgr();
        }

        public virtual bool canLoadLocalRes { get { return true; } }
        public long remoteVersionNum { get { return null == _m_lmRemoteResLoadMgr ? 0 : _m_lmRemoteResLoadMgr.versionNum; } }
        public long localVersionNum { get { return null == _m_lmLocalResLoadMgr ? 0 : _m_lmLocalResLoadMgr.versionNum; } }

        /** 获取本地补丁加载对象 */
        protected internal ALLocalPatchInfo _patchInfo { get { return _m_piPatchInfo; } }

        /// <summary>
        /// 是否有资源能加载，远程，本地都算
        /// </summary>
        public bool hasResources
        {
            get
            {
                bool hasResources = false;
                if(_m_lmRemoteResLoadMgr != null && _m_lmRemoteResLoadMgr.hasResource)
                {
                    hasResources = true;
                }
                if(_m_lmLocalResLoadMgr != null && _m_lmLocalResLoadMgr.hasResource)
                {
                    hasResources = true;
                }
                return hasResources;
            }
        }

        /********************
         * 初始化本资源版本管理对象的相关资源
         **/
        public void init(Action _delegate)
        {
            init(_delegate, 3);
        }
        public void init(Action _delegate, int _downloadMaxDealingCount)
        {
            //创建步骤统计对象
            _m_scInitStepCounter = new ALStepCounter();
            //注册回调
            _m_scInitStepCounter.regAllDoneDelegate(_delegate);
            //增加基础资源加载步骤
            if (null != _m_lmRemoteResLoadMgr)
                _m_scInitStepCounter.chgTotalStepCount(1);
            //设置总步骤数
            if(null != _m_piPatchInfo)
                _m_scInitStepCounter.chgTotalStepCount(1);
            if (null != _m_lmLocalResLoadMgr)
                _m_scInitStepCounter.chgTotalStepCount(1);

            //开启远程资源加载对象
            if (null != _m_lmRemoteResLoadMgr)
                _m_lmRemoteResLoadMgr.initVersionSet(_m_scInitStepCounter.addDoneStepCount);
            //根据本地资源部分是否有效进行处理
            if (null != _m_piPatchInfo)
            {
                //开启线程初始化本地资源，本地资源需要读取本地资源版本存储文件进行处理
                ALThread localPatchInitThread = new ALThread(_initLocalPatchThread);
                localPatchInitThread.startThread();

                //创建加载对象
                //获取当前系统安全的下载过程大小
                long saveLoadingSizeMB = 10;
                if (SystemInfo.systemMemorySize > 720)
                    saveLoadingSizeMB = 30;
                if (SystemInfo.systemMemorySize > 1024)
                    saveLoadingSizeMB = 50;
                if (SystemInfo.systemMemorySize > 1720)
                    saveLoadingSizeMB = 200;

                //创建下载处理对象
                _m_dmHttpDownloadMgr = new ALHttpDownloadMgr(_m_iRetryCount, (saveLoadingSizeMB * 1024 * 1024), _downloadMaxDealingCount);
            }
            //初始化本地打包资源版本信息
            if (null != _m_lmLocalResLoadMgr)
                _m_lmLocalResLoadMgr.initVersionSet(_m_scInitStepCounter.addDoneStepCount);
        }

        /*******************
         * 初始化本对象中的远程加载对象，在平台资源加载的时候有需要使用本操作
         **/
        public void initRemoteResLoadMgr(string _remoteURL, int _maxLoadingCount, int _maxRetryCount, bool _useCache, Action _delegate)
        {
            initRemoteResLoadMgr(new ALABStringLoadPathProvider(_remoteURL), _maxLoadingCount, _maxRetryCount, _useCache, _delegate);
        }
        public void initRemoteResLoadMgr(_IALAssetBundleLoadPathProvider _urlProvider, int _maxLoadingCount, int _maxRetryCount, bool _useCache, Action _delegate)
        {
            if (null != _m_lmRemoteResLoadMgr)
            {
                //报错
                Debug.LogError("try to init remote res load mgr: " + _urlProvider.roolURL + " while the remote res load already init by: " + _m_lmRemoteResLoadMgr.rootURLProvider.roolURL);
                return;
            }

            //创建对象
            _m_lmRemoteResLoadMgr = new ALRemoteAssetBundleLoadMgr(_maxLoadingCount, _maxRetryCount, _urlProvider, false);
            //调用初始化
            _m_lmRemoteResLoadMgr.initVersionSet(_delegate);
        }

        /***********************
         * 初始化相关本地资源信息
         **/
        protected void _initLocalPatchThread()
        {
            try
            {
                //开始初始化版本信息
                _m_piPatchInfo.init();
            }
            catch(Exception)
            {
            }

            //增加下一帧任务进行操作步骤操作
            ALStepCounterAddStepMonoTask.addStep(_m_scInitStepCounter);
        }

        /// <summary>
        /// 初始化依赖信息
        /// </summary>
        /// <param name="_doneDelegate">初始化完成后的处理</param>
        protected void _initDependence(Action _doneDelegate)
        {
            //加载本地依赖信息
            _dealLoadAsset("__dependence.unity3d"
                , (bool _isSuc, ALAssetBundleObj _assetObj) => 
                        {
                            _dependenceAssetDownloadedDelegate(_isSuc, _assetObj);

                            //处理回调
                            if(null != _doneDelegate)
                                _doneDelegate();

                            //释放资源
                            if(null != _assetObj)
                                _assetObj.discard();
                        }
                , null);
        }
        /// <summary>
        /// 依赖关系加载完成的处理
        /// </summary>
        /// <param name="_isSuc"></param>
        /// <param name="_assetObj"></param>
        protected void _dependenceAssetDownloadedDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            if(_m_bIsDependenceInit)
                return;

            //设置初始化完成
            _m_bIsDependenceInit = true;

            if(!_isSuc)
            {
                //这里不成功，比较奇怪，所以加多一点的log来帮助定位
                long patchResourceVersion = _m_piPatchInfo != null ? _m_piPatchInfo.resourceVersion : -1;
                long localVersion = -1;
                if(_m_lmLocalResLoadMgr != null)
                {
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo("__dependence.unity3d");
                    if(localVersionInfo != null)
                    {
                        localVersion = localVersionInfo.versionNum;
                    }
                }    
                long remoteVersion = -1;
                if(_m_lmRemoteResLoadMgr != null)
                {
                    ALAssetBundleVersionInfo remoteVersionInfo = _m_lmRemoteResLoadMgr.getVersionInfo("__dependence.unity3d");
                    if(remoteVersionInfo != null)
                    {
                        remoteVersion = remoteVersionInfo.versionNum;
                    }
                }
                bool isAssetobjNull = _assetObj == null;
                UnityEngine.Debug.LogError(string.Format(
                    "Init AssetBundle Dependence Err! patchResourcesVersion:{0}, remoteVersionNum:{1}, localVersion:{2}, remoteVersion:{3}, isAssetobjNull:{4}",
                    patchResourceVersion, remoteVersionNum, localVersion, remoteVersion, isAssetobjNull));
                return;
            }

            //根据数据进行初始化
            ALSOAssetBundleDependenceSet dependenceSet = _assetObj.assetBundle.LoadAsset<ALSOAssetBundleDependenceSet>("__dependence");
            if(null == dependenceSet)
            {
                UnityEngine.Debug.LogError("Get AssetBundle Dependence Set Err!");
                return;
            }

            //初始化
            _m_dmDependenceMgr.init(dependenceSet);
        }

        /// <summary>
        /// 加载资源的处理函数
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_delegate"></param>
        /// <param name="_lateDelegate"></param>
        //public void loadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        //{
        //    if (!_m_bIsDependenceInit)
        //    {
        //        //进行初始化处理
        //        _initDependence(() => { _realDealLoadAsset(_assetPath, _delegate, _lateDelegate); });
        //    }
        //    else
        //    {
        //        //开始加载处理
        //        _realDealLoadAsset(_assetPath, _delegate, _lateDelegate);
        //    }
        //}
        public void loadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            if (!_m_bIsDependenceInit)
            {
                //进行初始化处理
                _initDependence(() => { _recursionAsset(_assetPath, _delegate, _lateDelegate, new HashSet<string>()); });
            }
            else
            {
                //开始加载处理
                _recursionAsset(_assetPath, _delegate, _lateDelegate, new HashSet<string>());
            }
        }
        public void loadSynAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            if (!_m_bIsDependenceInit)
            {
                //进行初始化处理
                _initDependence(() => { _recursionSynAsset(_assetPath, _delegate, _lateDelegate, new HashSet<string>()); });
            }
            else
            {
                //开始加载处理
                _recursionSynAsset(_assetPath, _delegate, _lateDelegate, new HashSet<string>());
            }
        }

        /// <summary>
        /// 清除所有缓存资源
        /// </summary>
        public void clearPatch()
        {
            if(null == _m_piPatchInfo)
                return;

            _m_piPatchInfo.clearPatch();
        }

        /// <summary>
        /// 递归调用资源解析放入加载队列
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_delegate"></param>
        /// <param name="_lateDelegate"></param>
        /// <param name="_depth">深度，当深度递增到7的时候将直接进行加载处理</param>
        protected void _recursionAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate, HashSet<string> _doingSet)
        {
			//优先判断深度进行处理
			if (_doingSet.Contains(_assetPath))
            {
                //直接开始加载并返回
                _dealLoadAsset(_assetPath, _delegate, _lateDelegate);
                return;
            }

            //获取依赖关系
            ALResABDependenceInfo dependenceInfo = _m_dmDependenceMgr.getABDependence(_assetPath);
            if (null == dependenceInfo || dependenceInfo.dependenceList.Count <= 0 || dependenceInfo.isLoadedDependence)
            {
                //直接开始加载并返回
                _dealLoadAsset(_assetPath, _delegate, _lateDelegate);
                return;
            }

            //开启步骤处理
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(dependenceInfo.dependenceList.Count);
            //注册最后回调
            stepCounter.regAllDoneDelegate(
                () =>
                {
                    //此处只设置依赖已加载，不进行其他的加载处理，加载关系使用递归处理加入队列
                    dependenceInfo.setLoadedDependence();

                    //递归完子节点后才开始具体加载
                    _dealLoadAsset(_assetPath, _delegate, _lateDelegate);
                });

            //添加当前加载数据到集合中
            _doingSet.Add(_assetPath);

			//开始进行依赖加载
			for (int i = 0; i < dependenceInfo.dependenceList.Count; i++)
            {
                _recursionAsset(dependenceInfo.dependenceList[i]
                    , (bool _isSuc, ALAssetBundleObj _assetObj) => { stepCounter.addDoneStepCount(); }
                    , null, _doingSet);
            }
        }

        /// <summary>
        /// 递归调用资源解析放入加载队列
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_delegate"></param>
        /// <param name="_lateDelegate"></param>
        /// <param name="_depth">深度，当深度递增到7的时候将直接进行加载处理</param>
        protected void _recursionSynAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate, HashSet<string> _doingSet)
        {
			//优先判断深度进行处理
			if (_doingSet.Contains(_assetPath))
            {
                //直接开始加载并返回
                _dealSynLoadAsset(_assetPath, _delegate, _lateDelegate);
                return;
            }

            //获取依赖关系
            ALResABDependenceInfo dependenceInfo = _m_dmDependenceMgr.getABDependence(_assetPath);
            if (null == dependenceInfo || dependenceInfo.dependenceList.Count <= 0 || dependenceInfo.isLoadedDependence)
            {
                //直接开始加载并返回
                _dealSynLoadAsset(_assetPath, _delegate, _lateDelegate);
                return;
            }

            //开启步骤处理
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(dependenceInfo.dependenceList.Count);
            //注册最后回调
            stepCounter.regAllDoneDelegate(
                () =>
                {
                    //此处只设置依赖已加载，不进行其他的加载处理，加载关系使用递归处理加入队列
                    dependenceInfo.setLoadedDependence();

                    //递归完子节点后才开始具体加载
                    _dealSynLoadAsset(_assetPath, _delegate, _lateDelegate);
                });

            //添加当前加载数据到集合中
            _doingSet.Add(_assetPath);

            //开始进行依赖加载
            for (int i = 0; i < dependenceInfo.dependenceList.Count; i++)
            {
                _recursionSynAsset(dependenceInfo.dependenceList[i]
                    , (bool _isSuc, ALAssetBundleObj _assetObj) => { stepCounter.addDoneStepCount(); }
                    , null, _doingSet);
            }
        }

        /// <summary>
        /// 实际处理依赖及加载的操作
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_delegate"></param>
        /// <param name="_lateDelegate"></param>
        protected void _realDealLoadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            //获取依赖关系
            ALResABDependenceInfo dependenceInfo = _m_dmDependenceMgr.getABDependence(_assetPath);
            if(null == dependenceInfo || dependenceInfo.dependenceList.Count <= 0 || dependenceInfo.isLoadedDependence)
            {
                //直接开始加载并返回
                _dealLoadAsset(_assetPath, _delegate, _lateDelegate);
                return;
            }

            //开启步骤处理
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(dependenceInfo.dependenceList.Count);
            //注册最后回调
            stepCounter.regAllDoneDelegate(
                () =>
                {
                    //设置依赖已加载
                    dependenceInfo.setLoadedDependence();
                    //开始具体加载
                    _dealLoadAsset(_assetPath, _delegate, _lateDelegate);
                });

            //开始进行依赖加载
            for(int i = 0; i < dependenceInfo.dependenceList.Count; i++)
            {
                _dealLoadAsset(dependenceInfo.dependenceList[i]
                    , (bool _isSuc, ALAssetBundleObj _assetObj) => { stepCounter.addDoneStepCount(); }
                    , null);
            }
        }

        /********************
         * 加载资源的处理函数
         **/
        protected abstract void _dealLoadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate);
        /********************
         * 必须同步加载资源的处理函数
         **/
        protected abstract void _dealSynLoadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate);

        /*********************
         * 更新所有资源，带入过程回调和结果回调，过程回调的3个参数为已下载字节 - 已解压字节数 - 总字节，是否在下载中
         **/
        //public void updateAllRes(Action<long, long, long, bool> _processDelegate, Action _doneDelegate, Action<long, Action> _askDelegate)
        //{
        //    //判断本地处理对象是否为空，是表示不需要更新，直接返回
        //    if (null == _m_piPatchInfo)
        //        return;

        //    if (null == _m_lmRemoteResLoadMgr)
        //    {
        //        Debug.LogError("Can not Update All Res With RemoteResMgr is Null!");
        //        return;
        //    }

        //    //将所有打包资源和补丁资源信息取出与远程资源进行比对后进行后续处理
        //    List<ALAssetBundleVersionInfo> allRemoteAssetInfo = _m_lmRemoteResLoadMgr.getAllVersionInfo();
        //    Dictionary<string, ALAssetBundleVersionInfo> allPatchAssetInfo = _m_piPatchInfo.cloneVersionDic();


        //    //需要从补丁中删除的资源对象
        //    List<ALAssetBundleVersionInfo> removePatchAssetList = new List<ALAssetBundleVersionInfo>();
        //    //需要更新的资源补丁对象
        //    List<ALAssetBundleVersionInfo> updatePatchAssetList = new List<ALAssetBundleVersionInfo>();
        //    long totalUpdateSize = 0;
        //    //遍历远程资源信息，判断哪些需要删除，哪些需要更新
        //    for (int i = 0; i < allRemoteAssetInfo.Count; i++)
        //    {
        //        ALAssetBundleVersionInfo remoteAssetInfo = allRemoteAssetInfo[i];
        //        if (null == remoteAssetInfo)
        //            continue;

        //        //获取补丁的资源版本号是否匹配
        //        ALAssetBundleVersionInfo patchInfo = null;
        //        if(allPatchAssetInfo.ContainsKey(remoteAssetInfo.assetPath))
        //        {
        //            patchInfo = allPatchAssetInfo[remoteAssetInfo.assetPath];
        //            //从补丁克隆信息中删除对应信息
        //            allPatchAssetInfo.Remove(remoteAssetInfo.assetPath);
        //        }

        //        //获取本地打包资源中的版本信息
        //        ALAssetBundleVersionInfo localInfo = _m_lmLocalResLoadMgr.getVersionInfo(remoteAssetInfo.assetPath);

        //        //判断文件是否存在，不存在则需要下载
        //        if (null != patchInfo && !File.Exists(_m_piPatchInfo.assetBundleLoadMgr.getAssetRealURL(patchInfo.assetPath)))
        //        {
        //            //文件不存在则当作数据版本无效
        //            patchInfo.versionNum = 0;
        //        }

        //        //判断资源是否不需要更新，是则进入下一个循环
        //        if (null != patchInfo && patchInfo.versionNum == remoteAssetInfo.versionNum)
        //            continue;

        //        //判断本地资源是否为最新，是则不需要更新
        //        if (null == patchInfo && null != localInfo && localInfo.versionNum == remoteAssetInfo.versionNum)
        //            continue;

        //        //判断是需要更新还是需要删除
        //        if(null != patchInfo)
        //        {
        //            //如果补丁信息存在则判断本地信息是否最新，是则需要删除补丁信息
        //            if (null != localInfo && localInfo.versionNum == remoteAssetInfo.versionNum)
        //            {
        //                removePatchAssetList.Add(remoteAssetInfo);
        //            }
        //            else
        //            {
        //                updatePatchAssetList.Add(remoteAssetInfo);
        //                totalUpdateSize += remoteAssetInfo.fileSize;
        //            }
        //        }
        //        else
        //        {
        //            //无补丁信息则必须更新
        //            updatePatchAssetList.Add(remoteAssetInfo);
        //            totalUpdateSize += remoteAssetInfo.fileSize;
        //        }
        //    }

        //    //判断是否不需要更新
        //    if (totalUpdateSize <= 0)
        //    {
        //        if (null != _doneDelegate)
        //            _doneDelegate();

        //        return;
        //    }

        //    //询问是否继续剩下的操作，是则继续，否则不进行处理
        //    if (null == _askDelegate)
        //    {
        //        _updateFunc(allPatchAssetInfo, removePatchAssetList, updatePatchAssetList, _processDelegate, _doneDelegate);
        //    }
        //    else
        //    {
        //        _askDelegate(totalUpdateSize, () => { _updateFunc(allPatchAssetInfo, removePatchAssetList, updatePatchAssetList, _processDelegate, _doneDelegate); });
        //    }
        //}
        ///*************
        // * 实际处理更新的操作函数
        // **/
        //protected void _updateFunc(
        //    Dictionary<string, ALAssetBundleVersionInfo> _allPatchAssetInfo, List<ALAssetBundleVersionInfo> _removePatchAssetList, List<ALAssetBundleVersionInfo> _updatePatchAssetList
        //    , Action<long, long, long, bool> _processDelegate, Action _doneDelegate)
        //{
        //    //剩余的补丁信息对象都是需要删除的资源
        //    foreach (ALAssetBundleVersionInfo versionInfo in _allPatchAssetInfo.Values)
        //    {
        //        _m_piPatchInfo.removePatchAsset(versionInfo.assetPath);
        //    }

        //    //删除需要删除的信息对象
        //    for (int i = 0; i < _removePatchAssetList.Count; i++)
        //    {
        //        ALAssetBundleVersionInfo removeInfo = _removePatchAssetList[i];
        //        _m_piPatchInfo.removePatchAsset(removeInfo.assetPath);
        //    }

        //    //由于删除了文件，此时强制保存补丁信息
        //    _m_piPatchInfo.savePatchInfo();

        //    //开启剩余下载操作
        //    if (null != _updatePatchAssetList && _updatePatchAssetList.Count > 0)
        //    {
        //        //逐个对象开启加载
        //        for (int i = 0; i < _updatePatchAssetList.Count; i++)
        //        {
        //            ALAssetBundleVersionInfo updateInfo = _updatePatchAssetList[i];
        //            if (null == updateInfo)
        //                continue;

        //            //创建保存处理对象
        //            ALResourcePatchUpdateWWWDelegate delegateObj = new ALResourcePatchUpdateWWWDelegate(this, updateInfo);
        //            string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(updateInfo);
        //            //放入下载处理对象
        //            _m_dmHttpDownloadMgr.download(_m_lmRemoteResLoadMgr.rootURL + "/" + updateInfo.assetPath, updateInfo.fileSize, tempDownloadLocalFile, delegateObj);

        //            //开启解压进度监控
        //            ALResourcePatchUpdateUnzipMonitor unzipMonitor = new ALResourcePatchUpdateUnzipMonitor(_m_dmHttpDownloadMgr, delegateObj);
        //            ALMonoTaskMgr.instance.addMonoTask(unzipMonitor);
        //        }

        //        //开启监控操作，监控下载处理对象是否完成，以及进度更新
        //        ALResourceUpdateMonitor monitor = new ALResourceUpdateMonitor(_m_dmHttpDownloadMgr, _processDelegate, 
        //            () => { _m_piPatchInfo.savePatchInfo(); if (null != _doneDelegate) { _doneDelegate(); } });
        //        //开启任务
        //        ALMonoTaskMgr.instance.addMonoTask(monitor);
        //    }
        //    else
        //    {
        //        //直接调用完成回调
        //        if (null != _doneDelegate)
        //            _doneDelegate();
        //    }
        //}

        ///*****************
        // * 判断是否需要更新资源信息，是则更新资源信息后调用对应的回调函数
        // **/
        //public void loadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        //{
        //    //获取远程资源中的对应资源信息
        //    ALAssetBundleVersionInfo remoteVersionInfo = null;
        //    if (null != _m_lmRemoteResLoadMgr)
        //    {
        //        //当有远程信息的时候获取到的资源如为空，表示资源无效
        //        remoteVersionInfo = _m_lmRemoteResLoadMgr.getVersionInfo(_assetPath);
        //        if (null == remoteVersionInfo)
        //        {
        //            //无版本信息，直接当作失败处理
        //            if (null != _delegate)
        //                _delegate(false, null);
        //            if (null != _lateDelegate)
        //                _lateDelegate(false, null);

        //            //返回不进行后续处理
        //            return;
        //        }
        //    }

        //    //判断是否有补丁信息
        //    if (null != _m_piPatchInfo)
        //    {
        //        //有补丁信息则判断是否需要更新
        //        //获取补丁对象的信息和打包部分的信息
        //        ALAssetBundleVersionInfo patchVersionInfo = _m_piPatchInfo.getAssetPatchVersionInfo(_assetPath);
        //        //判断本地补丁对象是否最新，不是则直接删除旧文件，然后判断是否需要下载
        //        if(null != patchVersionInfo)
        //        {
        //            if (null != remoteVersionInfo && patchVersionInfo.versionNum != remoteVersionInfo.versionNum)
        //            {
        //                //当资源对象有效且版本不匹配时才需要删除补丁对象信息
        //                _m_piPatchInfo.removePatchAsset(_assetPath);
        //                //设置数据更改，保存数据
        //                _m_piPatchInfo._setDataChg();
        //            }
        //            else
        //            {
        //                //无远程数据或版本号一致则直接加载补丁信息
        //                _m_piPatchInfo.loadAsset(_assetPath, _delegate, _lateDelegate);
        //                //直接返回，不进行后续操作
        //                return;
        //            }
        //        }

        //        //判断是否有打包信息，有则判断版本号是否为最新
        //        if (null != _m_lmLocalResLoadMgr)
        //        {
        //            //获取本地打包版本信息
        //            ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
        //            if (null != localVersionInfo && (null == remoteVersionInfo || localVersionInfo.versionNum == remoteVersionInfo.versionNum))
        //            {
        //                //此时版本与本地版本一致直接使用本地加载即可
        //                _m_lmLocalResLoadMgr.addLoadAssetInfo(_assetPath, false, true, _delegate, _lateDelegate);
        //                return;
        //            }
        //        }

        //        //如本地信息也不是最新，此时开启下载并在下载后，通过补丁管理器对象进行加载
        //        if (null != _m_lmRemoteResLoadMgr)
        //        {
        //            //创建保存处理对象
        //            ALResourcePatchLoadWWWDelegate delegateObj = new ALResourcePatchLoadWWWDelegate(this, remoteVersionInfo, _delegate, _lateDelegate);
        //            string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(remoteVersionInfo);
        //            //放入下载处理对象
        //            _m_dmHttpDownloadMgr.download(_m_lmRemoteResLoadMgr.rootURL + "/" + remoteVersionInfo.assetPath, remoteVersionInfo.fileSize, tempDownloadLocalFile, delegateObj);
        //        }
        //        else
        //        {
        //            //直接当作失败处理
        //            if (null != _delegate)
        //                _delegate(false, null);
        //        }
        //    }
        //    else
        //    {
        //        //判断是否有打包信息，有则判断版本号是否为最新
        //        if(null != _m_lmLocalResLoadMgr)
        //        {
        //            //获取本地打包版本信息
        //            ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
        //            if (null == localVersionInfo || (null != remoteVersionInfo && localVersionInfo.versionNum != remoteVersionInfo.versionNum))
        //            {
        //                //本地无信息或不为最新，则直接开启加载
        //                if (null != _m_lmRemoteResLoadMgr)
        //                {
        //                    _m_lmRemoteResLoadMgr.addLoadAssetInfo(_assetPath, false, true, _delegate, _lateDelegate);
        //                }
        //                else
        //                {
        //                    //直接当作失败处理
        //                    if (null != _delegate)
        //                        _delegate(false, null);
        //                }
        //            }
        //            else
        //            {
        //                //此时版本与本地版本一致直接使用本地加载即可
        //                _m_lmLocalResLoadMgr.addLoadAssetInfo(_assetPath, false, true, _delegate, _lateDelegate);
        //            }
        //        }
        //        else
        //        {
        //            //本地对象为空，则直接开启远程加载
        //            if (null != _m_lmRemoteResLoadMgr)
        //            {
        //                _m_lmRemoteResLoadMgr.addLoadAssetInfo(_assetPath, false, true, _delegate, _lateDelegate);
        //            }
        //            else
        //            {
        //                //直接当作失败处理
        //                if (null != _delegate)
        //                    _delegate(false, null);
        //            }
        //        }
        //    }
        //}

        /*****************
         * 根据资源是否有更新资源返回对应的文件路径
         **/
        public string getPatchAssetPath(string _assetPath)
        {
            //判断是否有补丁信息
            if (null != _m_piPatchInfo)
            {
                return _m_piPatchInfo.assetBundleLoadMgr.getAssetRealURL(_assetPath);
            }

            return null;
        }
        public string getLocalAssetPath(string _assetPath)
        {
            //判断是否有补丁信息
            if (null != _m_lmLocalResLoadMgr)
            {
                return _m_lmLocalResLoadMgr.getAssetRealURL(_assetPath);
            }

            return null;
        }
        public string getRealAssetPath(string _assetPath)
        {
            //判断是否有补丁信息
            if (null != _m_piPatchInfo)
            {
                //有补丁信息则判断是否需要更新
                //获取补丁对象的信息和打包部分的信息
                ALAssetBundleVersionInfo patchVersionInfo = _m_piPatchInfo.getAssetPatchVersionInfo(_assetPath);
                //判断本地补丁对象是否最新，不是则直接删除旧文件，然后判断是否需要下载
                if (null != patchVersionInfo)
                {
                    return _m_piPatchInfo.assetBundleLoadMgr.getAssetRealURL(_assetPath);
                }

                //判断是否有打包信息，有则判断版本号是否为最新
                if (null != _m_lmLocalResLoadMgr)
                {
                    return _m_lmLocalResLoadMgr.getAssetRealURL(_assetPath);
                }

                return null;
            }
            else
            {
                //判断是否有打包信息，有则判断版本号是否为最新
                if (null != _m_lmLocalResLoadMgr)
                {
                    //获取本地打包版本信息
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                    if (null == localVersionInfo)
                    {
                        return null;
                    }
                    else
                    {
                        //此时版本与本地版本一致直接使用本地加载即可
                        return _m_lmLocalResLoadMgr.getAssetRealURL(_assetPath);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 查询对应文件的最新版本信息
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public ALAssetBundleVersionInfo getVersionInfo(string _assetPath)
        {
            //判断是否有补丁信息
            if(null != _m_lmRemoteResLoadMgr)
            {
                ALAssetBundleVersionInfo versionInfo = _m_lmRemoteResLoadMgr.getVersionInfo(_assetPath);
                if(null != versionInfo)
                    return versionInfo;
            }

            if(null != _m_piPatchInfo)
            {
                ALAssetBundleVersionInfo versionInfo = _m_piPatchInfo.assetBundleLoadMgr.getVersionInfo(_assetPath);
                if(null != versionInfo)
                    return versionInfo;
            }

            //判断是否有打包信息，有则判断版本号是否为最新
            if(null != _m_lmLocalResLoadMgr)
            {
                return _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
            }

            return null;
        }

        /// <summary>
        /// 判断是否需要更新指定的资源，根据本地补丁数据以及本地读取数据版本号进行判断
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public bool isLocalAssetNewest(string _assetPath)
        {
            //获取远程资源中的对应资源信息
            ALAssetBundleVersionInfo remoteVersionInfo = null;
            if(null != _m_lmRemoteResLoadMgr && _m_lmRemoteResLoadMgr.isEnable)
            {
                //当有远程信息的时候获取到的资源如为空，表示资源无效
                remoteVersionInfo = _m_lmRemoteResLoadMgr.getVersionInfo(_assetPath);
            }

            //无远程版本则一定是最新
            if(null == remoteVersionInfo)
            {
                return true;
            }

            //判断是否有补丁信息
            if(null != _m_piPatchInfo)
            {
                //有补丁信息则判断是否需要更新
                //获取补丁对象的信息和打包部分的信息
                ALAssetBundleVersionInfo patchVersionInfo = _m_piPatchInfo.getAssetPatchVersionInfo(_assetPath);
                //判断本地补丁对象是否最新，不是则直接删除旧文件，然后判断是否需要下载
                if(null != patchVersionInfo)
                {
                    if(patchVersionInfo.versionNum == remoteVersionInfo.versionNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                //判断是否有打包信息，有则判断版本号是否为最新
                if(null != _m_lmLocalResLoadMgr)
                {
                    //获取本地打包版本信息
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                    if(null != localVersionInfo && localVersionInfo.versionNum == remoteVersionInfo.versionNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }
            else
            {
                //此处无Patch对象，表示不需要更新之后缓存，因此直接调用remote进行加载处理
                //判断是否有打包信息，有则判断版本号是否为最新
                if(null != _m_lmLocalResLoadMgr)
                {
                    //获取本地打包版本信息
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                    if(null == localVersionInfo && localVersionInfo.versionNum == remoteVersionInfo.versionNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 检查本地资源，如果本地原生资源是最新的则直接删除补丁数据
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public void checkLocalSrcAsset(string _assetPath)
        {
            //获取远程资源中的对应资源信息
            ALAssetBundleVersionInfo remoteVersionInfo = null;
            if (null != _m_lmRemoteResLoadMgr && _m_lmRemoteResLoadMgr.isEnable)
            {
                //当有远程信息的时候获取到的资源如为空，表示资源无效
                remoteVersionInfo = _m_lmRemoteResLoadMgr.getVersionInfo(_assetPath);
            }

            //无远程版本则一定是最新
            if (null == remoteVersionInfo)
            {
                return ;
            }

            //判断是否有补丁信息
            if (null != _m_piPatchInfo)
            {
                //有补丁信息则判断是否需要更新
                //获取补丁对象的信息和打包部分的信息
                ALAssetBundleVersionInfo patchVersionInfo = _m_piPatchInfo.getAssetPatchVersionInfo(_assetPath);
                //判断本地补丁对象是否最新，不是则直接删除旧文件，然后判断是否需要下载
                if (null != patchVersionInfo)
                {
                    //已经是最新的则不处理
                    if (patchVersionInfo.versionNum == remoteVersionInfo.versionNum)
                    {
                        return;
                    }

                    //判断是否有打包信息，有则判断版本号是否为最新，如果是最新则删除补丁部分的文件数据
                    if (null != _m_lmLocalResLoadMgr)
                    {
                        //获取本地打包版本信息
                        ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                        if (null != localVersionInfo && localVersionInfo.versionNum == remoteVersionInfo.versionNum)
                        {
                            _m_piPatchInfo.removePatchAsset(_assetPath);
                            //设置数据更改，保存数据
                            _m_piPatchInfo._setDataChg();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取远程下载的资源对象信息
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        public _IALLoadingAssetInterface getAssetLoadingInfo(string _assetPath)
        {
            //判断是否最新，是则一律返回完成
            if(isLocalAssetNewest(_assetPath))
            {
                return new ALLocalNewestAssetInterface(_assetPath);
            }

            //获取对应加载对象中的加载信息，判断是否有patch，有则从http获取，否则从remote下载获取
            if(null != _m_piPatchInfo)
            {
                ALHttpDownloadDealer dealer = _m_dmHttpDownloadMgr.getDownloadDealer(_assetPath);
                if(null == dealer)
                    return null;

                return new ALHttpDownloadDealerAssetInterface(dealer);
            }
            else
            {
                if(null == _m_lmRemoteResLoadMgr)
                    return null;

                ALLoadingAssetInfo loadingInfo = _m_lmRemoteResLoadMgr.getLoadingAssetInfo(_assetPath);
                if(null == loadingInfo)
                    return null;

                return new ALLoadingAssetInterface(loadingInfo);
            }
        }

        /// <summary>
        /// 获取资源的路径列表，如果是目录则返回目录下的所有资源
        /// </summary>
        /// <param name="_path">一个路径，可以是目录也可以是具体文件</param>
        /// <returns>
        /// 如果传入的路径是一个目录，则会返回目录下所有资源，如果传入的是具体文件，则会检查文件是否包含在这个 resCore 中，然后决定是否原封不动返回这个具体文件
        /// </returns>
        public List<string> getAssetPaths(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                return new List<string>(0);

            // 以有没有拓展名来判断是否是指具体文件
            bool isFile = ALCommon.hasExtension(_path);
            // 如果是文件则判断是否包含在 resCore 内，是的话就返回
            if (isFile)
            {
                if (getVersionInfo(_path) != null)
                    return new List<string>(1) { _path };
                
                return new List<string>(0);
            }
            
            // 如果不是具体文件，就认为传入的 _m_sPath 是一个目录，那么就返回这个目录下的所有资源

            // 使用 HashSet 来保证结果唯一性 
            HashSet<string> result = new HashSet<string>();
            // 先尝试从远端资源中获取
            if (_m_lmRemoteResLoadMgr != null)
            {
                List<ALAssetBundleVersionInfo> allVersionInfo = _m_lmRemoteResLoadMgr.getAllVersionInfo();
                for (int i = 0; i < allVersionInfo.Count; i++)
                {
                    if (allVersionInfo[i].assetPath.StartsWith(_path))
                    {
                        result.Add(allVersionInfo[i].assetPath);
                    }
                }
            }

            // 再尝试从补丁资源中获取
            if (_m_piPatchInfo != null)
            {
                List<ALAssetBundleVersionInfo> allVersionInfo = _m_piPatchInfo.assetBundleLoadMgr.getAllVersionInfo();
                for (int i = 0; i < allVersionInfo.Count; i++)
                {
                    if (allVersionInfo[i].assetPath.StartsWith(_path))
                    {
                        result.Add(allVersionInfo[i].assetPath);
                    }
                }
            }

            // 最后再尝试从随包资源中获取
            if (_m_lmLocalResLoadMgr != null)
            {
                List<ALAssetBundleVersionInfo> allVersionInfo = _m_lmLocalResLoadMgr.getAllVersionInfo();
                for (int i = 0; i < allVersionInfo.Count; i++)
                {
                    if (allVersionInfo[i].assetPath.StartsWith(_path))
                    {
                        result.Add(allVersionInfo[i].assetPath);
                    }
                }
            }
                
            return new List<string>(result);
        }

        /******************
         * 释放相关资源，特别如停止线程加载
         **/
        public void discard()
        {
            if(null != _m_dmHttpDownloadMgr)
                _m_dmHttpDownloadMgr.discard();
        }

#if AL_SEVEN_ZIP
        /*****************
         * 增加补丁文件的信息
         **/
        protected internal void _addLZMAPatchFile(ALAssetBundleVersionInfo _versionInfo, string _downloadLocalPath, SevenZip.ICodeProgress _progress)
        {
            if (null == _m_piPatchInfo || null == _versionInfo)
                return;

            //增加补丁文件信息
            _m_piPatchInfo.addLZMAPatchFileInfo(_versionInfo, _downloadLocalPath, _progress);
        }
#endif

        protected internal void _addUnCompressPatchFile(ALAssetBundleVersionInfo _versionInfo, string _downloadLocalPath)
        {
            if(null == _m_piPatchInfo || null == _versionInfo)
                return;

            //增加补丁文件信息
            _m_piPatchInfo.addUnCompressPatchFileInfo(_versionInfo, _downloadLocalPath);
        }

#if AL_SEVEN_ZIP
        /// <summary>
        /// 是否远程资源是压缩资源，是则会调用解压处理，不是则直接拷贝到指定路径
        /// </summary>
        public abstract bool isRemoteResCompress { get; }
#endif
    }
}
