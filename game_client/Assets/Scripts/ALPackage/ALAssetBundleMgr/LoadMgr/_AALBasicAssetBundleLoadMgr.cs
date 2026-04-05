using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /********************
     * AssetBundle资源加载管理对象
     **/
    public abstract class _AALBasicAssetBundleLoadMgr : _IALAssetBundleVersionInitFunc
    {
        /** 最大同时下载数量 */
        protected int _m_iMaxLoadingCount;
        /** 最多重试次数 */
        private int _m_iMaxRetryCount;
        /** 下载根URL */
        private _IALAssetBundleLoadPathProvider _m_ppPathProvider;
        /** 是否使用Cache */
        private bool _m_bUseCache;

        /** 加载的版本资源版本号 */
        private long _m_lVersionNum; 

        /** 非必要资源的等待下载的队列 */
        private LinkedList<ALLoadingAssetInfo> _m_lWaitingUnnecResList;
        /** 必要资源的等待下载的队列 */
        private LinkedList<ALLoadingAssetInfo> _m_lWaitingNecResList;

        /** 正在下载的队列 */
        private List<ALLoadingAssetInfo> _m_lLoadingList;
        /** 已经处理了加载的帧序列编号 */
        private int _m_iDealedPopFrameCount;

        /** 所有等待或正在处理的路径集合 */
        private Dictionary<string, ALLoadingAssetInfo> _m_hsDealingPathDic;

        /** 下载资源管理对象 */
        private ALAssetBundleResourceMgr _m_rmResourceMgr;
        /** 资源版本管理对象 */
        private ALAssetBundleVersionInfoMgr _m_vimVersionMgr;

        /// <summary>
        /// 在已经过了每帧加载之后，如还有出现需要加载的必要资源，则调用本任务进行处理
        /// </summary>
        private ALAssetBundleSynLoadAdditionTask _m_atLoadNecABAdditionTask;

        /** 是否已经开启了加载监控 */
        private bool _m_bIsMonitorStart;
        /** 版本数据是否有效 */
        private bool _m_bIsVersionEnable;

        public _AALBasicAssetBundleLoadMgr(int _maxLoadingCount, int _maxRetryCount, _IALAssetBundleLoadPathProvider _urlProvider, bool _useCache)
        {
            _m_iMaxLoadingCount = _maxLoadingCount;
            _m_iMaxRetryCount = _maxRetryCount;
            _m_ppPathProvider = _urlProvider;

            _m_bUseCache = _useCache;

            //根据系统内存判断最大加载数量
            int sysMemSize = SystemInfo.systemMemorySize;
#if UNITY_ANDROID
            if (sysMemSize <= 256)
            {
                if (_m_iMaxLoadingCount > 1)
                    _m_iMaxLoadingCount = 1;
            }
            else if (sysMemSize <= 512)
            {
                if (_m_iMaxLoadingCount > 1)
                    _m_iMaxLoadingCount = 1;
            }
            else if (sysMemSize <= 1024)
            {
                if (_m_iMaxLoadingCount > 1)
                    _m_iMaxLoadingCount = 1;
            }
            else if (sysMemSize <= 2048)
            {
                if (_m_iMaxLoadingCount > 2)
                    _m_iMaxLoadingCount = 2;
            }
            else
            {
                if (_m_iMaxLoadingCount > 4)
                    _m_iMaxLoadingCount = 4;
            }
#else
            if (sysMemSize <= 1024)
            {
                if (_m_iMaxLoadingCount > 3)
                    _m_iMaxLoadingCount = 3;
            }
            else if (sysMemSize <= 2048)
            {
                if (_m_iMaxLoadingCount > 5)
                    _m_iMaxLoadingCount = 5;
            }
            else
            {
                if (_m_iMaxLoadingCount > 10)
                    _m_iMaxLoadingCount = 10;
            }
#endif

            _m_lWaitingUnnecResList = new LinkedList<ALLoadingAssetInfo>();
            _m_lWaitingNecResList = new LinkedList<ALLoadingAssetInfo>();

            _m_lLoadingList = new List<ALLoadingAssetInfo>();
            _m_iDealedPopFrameCount = 0;

            _m_hsDealingPathDic = new Dictionary<string, ALLoadingAssetInfo>();

            _m_rmResourceMgr = new ALAssetBundleResourceMgr();

            _m_vimVersionMgr = new ALAssetBundleVersionInfoMgr();

            _m_atLoadNecABAdditionTask = new ALAssetBundleSynLoadAdditionTask(this);

            _m_bIsMonitorStart = false;
            _m_bIsVersionEnable = true;
        }

        public long versionNum { get { return _m_lVersionNum; } }

        public string rootURL
        {
            get
            {
                return _m_ppPathProvider.roolURL;
            }
        }

        //获取路径提供数据对象
        public _IALAssetBundleLoadPathProvider rootURLProvider
        {
            get
            {
                return _m_ppPathProvider;
            }
        }

        public bool useCache
        {
            get { return _m_bUseCache; }
        }

        public ALAssetBundleResourceMgr resourceMgr
        {
            get { return _m_rmResourceMgr; }
        }

        /** 获取版本是否已经初始化完成 */
        public bool isVersionInited { get { return _m_vimVersionMgr.isInit; } }
        /** 版本信息是否有效 */
        public bool isEnable { get { return _m_bIsVersionEnable; } }

        /// <summary>
        /// 是否有资源
        /// </summary>
        public bool hasResource { get { return _m_vimVersionMgr.hasResource; } }

        /*******************
         * 开始本资源管理对象的版本资源加载及初始化操作
         **/
        public void initVersionSet(Action _initedDelegate)
        {
            //注册回调
            _m_vimVersionMgr.regInitedDelegate(_initedDelegate);

            //当还未初始化完成版本信息时，开启资源版本加载。
            //此处即使重复开启也无问题，因此不对是否开始加载进行判断
            if (!_m_vimVersionMgr.isInit)
                _initVersion();
        }

        /********************
         * 根据外部带入的版本信息队列直接进行版本信息的初始化操作
         **/
        public void initVersionSet(ALSOAssetBundleVersionSet _versionSet)
        {
            if (null == _versionSet)
                return;
            
            initVersionSetList(_versionSet.versionNum, _versionSet.versionInfoList);
        }


        public void initVersionFail()
        {
            //设置版本信息无效
            _m_bIsVersionEnable = false;
            initVersionSetList(0, null);
        }
        public void initVersionSetList(long _versionNum, List<ALAssetBundleVersionInfo> _versionInfoList)
        {
            //设置版本号
            _m_lVersionNum = _versionNum;

            //设置版本信息队列
            _m_vimVersionMgr.initVersionDic(_versionInfoList);

            //开启加载调度处理对象
            startMonitor();
        }

        /****************
         * 开启加载监控操作
         **/
        public void startMonitor()
        {
            if (_m_bIsMonitorStart)
                return;

            //设置监控开启
            _m_bIsMonitorStart = true;
            //开启加载调度处理对象
            ALCoroutineDealerMgr.instance.addCoroutine(new ALAssetBundleLoadingObjMgrDealer(this));
        }

        /****************
         * 获取对应路径的加载文件信息对象
         **/
        public ALAssetBundleVersionInfo getVersionInfo(string _assetPath)
        {
            if (!isVersionInited)
                return null;

            return _m_vimVersionMgr.getVersionInfo(_assetPath);
        }

        /****************
         * 获取所有资源信息的队列
         **/
        public List<ALAssetBundleVersionInfo> getAllVersionInfo()
        {
            return _m_vimVersionMgr.getAllVersionInfo();
        }

        /****************
         * 添加需要下载的资源信息
         **/
        public void addLoadAssetInfo(string _resPath, bool _useCache, bool _isNecessary)
        {
            if (_m_hsDealingPathDic.ContainsKey(_resPath))
                return;

            ALLoadingAssetInfo info = new ALLoadingAssetInfo(_resPath, _useCache, _isNecessary);

            _addLoadAssetInfo(info);
        }

        public void addLoadAssetInfo(string _resPath, bool _useCache, bool _isNecessary, List<_assetDownloadedDelegate> _delegateList, List<_assetDownloadedDelegate> _lateDelegateList)
        {
            ALLoadingAssetInfo info = new ALLoadingAssetInfo(_resPath, _useCache, _isNecessary);

            //将回调添加到加载对象中
            if (null != _delegateList)
            {
                for (int i = 0; i < _delegateList.Count; i++)
                {
                    info.addDelegate(_delegateList[i]);
                }
            }
            if (null != _lateDelegateList)
            {
                for (int i = 0; i < _lateDelegateList.Count; i++)
                {
                    info.addLateDelegate(_lateDelegateList[i]);
                }
            }

            _addLoadAssetInfo(info);
        }
        public void addLoadAssetInfo(string _resPath, bool _useCache, bool _isNecessary, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            ALLoadingAssetInfo info = new ALLoadingAssetInfo(_resPath, _useCache, _isNecessary);

            //将回调添加到加载对象中
            if (null != _delegate)
            {
                info.addDelegate(_delegate);
            }
            if (null != _lateDelegate)
            {
                info.addLateDelegate(_lateDelegate);
            }

            _addLoadAssetInfo(info);
        }

        /************
         * 根据Asset路径获取实际的加载路径
         **/
        public string getAssetRealURL(string _assetPath)
        {
            string lowerAssetPath = "";
            if (!string.IsNullOrEmpty(_assetPath))
            {
                lowerAssetPath = _assetPath.ToLowerInvariant();
            }
            //使用实际路径拼凑下载具体路径
#if UNITY_ANDROID
            return _m_ppPathProvider.getAndroidAssetPath(lowerAssetPath);
            //rootURL.Replace("jar:file://", "").Replace("!/assets", "!assets") + "/" + _assetPath.ToLowerInvariant();
#else
            return _m_ppPathProvider.getAssetPath(lowerAssetPath);
            //return rootURL + "/" + _assetPath.ToLowerInvariant();
#endif
        }

        /**********************
         * 获取正在加载的资源信息
         **/
        public ALLoadingAssetInfo getLoadingAssetInfo(string _assetPath)
        {
            string path = _assetPath.ToLowerInvariant();

            if (!_m_hsDealingPathDic.ContainsKey(path))
                return null;

            return _m_hsDealingPathDic[path];
        }

        /******************
         * 尝试取出一个下载对象进行加载操作
         **/
        public void tryPopLoadingObj()
        {
            //当正在加载的对象数量未超过上限，则开始尝试取出需要加载的对象信息，必要队列必须全部加载
            while (_m_lLoadingList.Count < _m_iMaxLoadingCount || _m_lWaitingNecResList.Count > 0)
            {
                ALLoadingAssetInfo loadInfo = null;

                if (_m_lWaitingNecResList.Count > 0)
                {
                    //获取第一个需要加载的对象
                    loadInfo = _m_lWaitingNecResList.First.Value;
                    //删除队列第一个对象
                    _m_lWaitingNecResList.RemoveFirst();
                }
                else if (_m_lWaitingUnnecResList.Count > 0)
                {
                    //获取第一个需要加载的对象
                    loadInfo = _m_lWaitingUnnecResList.First.Value;
                    //删除队列第一个对象
                    _m_lWaitingUnnecResList.RemoveFirst();
                }

                if (null == loadInfo)
                {
                    //无需要加载对象则跳出循环
                    break;
                }

                //将对象添加到正在加载队列中
                _m_lLoadingList.Add(loadInfo);

                //开启任务进行加载
                _AALBaseAssetBundleDealer dealer = _createAssetBundleDealer(loadInfo);
                ALCommonActionMonoTask.addLaterMonoTask(dealer.startLoad);
            }

            //刷新帧编号
            _m_iDealedPopFrameCount = Time.frameCount;
        }

        /// <summary>
        /// 额外进行的同步加载资源的加载处理
        /// </summary>
        public void tryPopLoadingNecObj()
        {
            //当正在加载的对象数量未超过上限，则开始尝试取出需要加载的对象信息，必要队列必须全部加载
            while(_m_lWaitingNecResList.Count > 0)
            {
                ALLoadingAssetInfo loadInfo = null;

                if(_m_lWaitingNecResList.Count > 0)
                {
                    //获取第一个需要加载的对象
                    loadInfo = _m_lWaitingNecResList.First.Value;
                    //删除队列第一个对象
                    _m_lWaitingNecResList.RemoveFirst();
                }

                if(null == loadInfo)
                {
                    //无需要加载对象则跳出循环
                    break;
                }

                //将对象添加到正在加载队列中
                _m_lLoadingList.Add(loadInfo);

                //开启任务进行加载
                _AALBaseAssetBundleDealer dealer = _createAssetBundleDealer(loadInfo);
                ALCommonActionMonoTask.addLaterMonoTask(dealer.startLoad);
            }

            //刷新帧编号
            _m_iDealedPopFrameCount = Time.frameCount;
        }

        /*****************
         * 将资源加载信息重新放入队列中，等待下次调用
         **/
        public void retryInfo(ALLoadingAssetInfo _info)
        {
            if (null == _info)
                return;

            //当对象在正在加载队列中时将对象从正在加载队列删除
            _m_lLoadingList.Remove(_info);

            //判断最大重试次数是否超出
            if (_info.retryCount >= _m_iMaxRetryCount)
            {
                Debug.LogError("AssetBundle Download Error: " + _info.path);

                //从加载信息集合中删除
                string idxPath = _info.path.ToLowerInvariant();
                if (_m_hsDealingPathDic.ContainsKey(idxPath))
                    _m_hsDealingPathDic.Remove(idxPath);

                //调用失败回调
                _info.callFailDelegate();
                _info.scheduleLateFail();

                return;
            }

            //增加重试次数
            _info.addRetryCount();

            //重新添加回队列
            if (_info.isNecessary)
            {
                _m_lWaitingNecResList.AddLast(_info);
            }
            else
            {
                _m_lWaitingUnnecResList.AddLast(_info);
            }
        }

        /*****************
         * 资源加载完成调用的设置函数
         **/
        public void comfirmAsset(ALLoadingAssetInfo _info, AssetBundle _assetBundle, bool _isWWWAssetBundle)
        {
            if (null == _info)
                return;

            //资源无效则重试
            if (null == _assetBundle)
            {
                retryInfo(_info);
                return;
            }

            //从正在加载对象中删除对象
            _m_lLoadingList.Remove(_info);

            //从加载信息集合中删除
            string idxPath = _info.path.ToLowerInvariant();
            if (_m_hsDealingPathDic.ContainsKey(idxPath))
                _m_hsDealingPathDic.Remove(idxPath);

            //注册结果到资源管理对象中
            ALAssetBundleObj assetObj = _m_rmResourceMgr.addLoadedAssetBundle(_info.path, _assetBundle, _isWWWAssetBundle);

            //调用回调
            if (null != assetObj)
            {
                _info.callSucDelegate(assetObj);
                //开启下一帧操作任务进行回调处理
                _info.scheduleLateSuc(assetObj);
            }
            else
            {
                _info.callFailDelegate();
                _info.scheduleLateFail();
            }
        }

        /********************
         * 内部统一调用的添加加载对象到队列中
         **/
        protected void _addLoadAssetInfo(ALLoadingAssetInfo _info)
        {
            //将回调添加到加载对象中
            if (null == _info)
                return;

            //判断是否已经加载完成
            ALAssetBundleObj loadedAssetObj = _m_rmResourceMgr.getLoadedAssetBundle(_info.path);
            if (null != loadedAssetObj)
            {
                //直接调用成功回调，并返回
                _info.callSucDelegate(loadedAssetObj);
                //开启延迟任务进行下一帧的处理操作
                _info.scheduleLateSuc(loadedAssetObj);

                return;
            }

            string idxPath = _info.path.ToLowerInvariant();

            //判断是否在下载集合中
            if (_m_hsDealingPathDic.ContainsKey(idxPath))
            {
                //将回调添加到正在加载的对象中
                ALLoadingAssetInfo loadingInfo = _m_hsDealingPathDic[idxPath];

                if (null == loadingInfo)
                    return;

                //添加回调
                loadingInfo.addDelegate(_info.downloadDelegate);
                loadingInfo.addLateDelegate(_info.downloadLateDelegate);

                //判断是否提升了优先级，首先加入的是必须加载的，而原先不是必须加载的
                //同时原先对象还没开始加载，此时需要将原数据从队列取出放入必须加载队列
                if (!loadingInfo.isNecessary && _info.isNecessary
                    && !_m_lLoadingList.Contains(loadingInfo))
                {
                    //修改必须信息
                    loadingInfo.setNec();
                    //从非必要队列取出
                    if (_m_lWaitingUnnecResList.Remove(loadingInfo))
                    {
                        //放入必须队列
                        _m_lWaitingNecResList.AddLast(loadingInfo);

                        //检测是否需要增加额外加载任务
                        _checkSynAdditionTask();
                    }
                }
            }
            else
            {
                //根据是否必要下载对象放入不同队列
                if (_info.isNecessary)
                {
                    _m_lWaitingNecResList.AddLast(_info);

                    //检测是否需要增加额外加载任务
                    _checkSynAdditionTask();
                }
                else
                {
                    _m_lWaitingUnnecResList.AddLast(_info);
                }

                //添加到下载对象集合中
                _m_hsDealingPathDic.Add(idxPath, _info);
            }
        }

        /// <summary>
        /// 检查是否需要开启同步加载的额外检查任务
        /// </summary>
        protected void _checkSynAdditionTask()
        {
            //判断是否有数据需要处理
            if(_m_lWaitingNecResList.Count <= 0)
                return;

            //判断是否已经进行过pop处理，帧序号不同代表没处理
            if(Time.frameCount != _m_iDealedPopFrameCount)
                return;

            //此时开任务处理，并设置帧序列为0
            ALMonoTaskMgr.instance.addLaterMonoTask(_m_atLoadNecABAdditionTask);
            _m_iDealedPopFrameCount = 0;
        }

        /****************
         * 初始化资源版本的操作
         **/
        protected abstract void _initVersion();
        /******************
         * 创建资源加载对象
         **/
        protected abstract _AALBaseAssetBundleDealer _createAssetBundleDealer(ALLoadingAssetInfo _loadingInfo);
    }
}
