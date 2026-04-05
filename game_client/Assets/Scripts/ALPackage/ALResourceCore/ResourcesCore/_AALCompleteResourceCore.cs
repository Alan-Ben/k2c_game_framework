using System;
using System.Threading;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

/*****************************
 * 完整更新的资源加载
 **/
namespace ALPackage
{
    public abstract class _AALCompleteResourceCore : _AALResourceCore
    {
        //创建单纯从远程下载资源的处理对象
        public _AALCompleteResourceCore(string _remoteURL, bool _useCache)
            : base(_remoteURL, _useCache)
        {
        }
        //创建单纯从远程下载资源的处理对象
        public _AALCompleteResourceCore(string _remoteURL, int _maxLoadingCount, int _maxRetryCount, bool _useCache)
            : base(_remoteURL, _maxLoadingCount, _maxRetryCount, _useCache)
        {
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALCompleteResourceCore(string _remoteURL, string _localPatchFolerPath)
            : base(_remoteURL, _localPatchFolerPath)
        {
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALCompleteResourceCore(string _remoteURL, string _localPatchFolerPath, int _maxLoadingCount, int _maxRetryCount)
            : base(_remoteURL, _localPatchFolerPath, _maxLoadingCount, _maxRetryCount)
        {
        }
        //创建仅有本地资源处理对象的数据处理对象
        public _AALCompleteResourceCore(string _localPatchFolerPath)
            : base(_localPatchFolerPath)
        {
        }
        //创建仅有本地资源处理对象的数据处理对象
        public _AALCompleteResourceCore(string _localPatchFolerPath, int _maxRetryCount)
            : base(_localPatchFolerPath, _maxRetryCount)
        {
        }

        /*********************
         * 更新所有资源，带入过程回调和结果回调，过程回调的3个参数为已下载字节 - 已解压字节数 - 总字节，是否在下载中
         **/
        public void updateAllRes(Action<long, long, long, bool> _processDelegate, Action<int> _doneDelegate, Action<long, Action> _askDelegate)
        {
            updateAllRes(_processDelegate, null, _doneDelegate, _askDelegate);
        }
        public void updateAllRes(Action<long, long, long, bool> _processDelegate, Func<string, bool> _judgeNeedUpdate, Action<int> _doneDelegate, Action<long, Action> _askDelegate)
        {
            //判断本地处理对象是否为空，是表示不需要更新，直接返回
            if (null == _m_piPatchInfo)
                return;

            if (null == _m_lmRemoteResLoadMgr)
            {
                Debug.LogError("Can not Update All Res With RemoteResMgr is Null!");
                return;
            }

            //将所有打包资源和补丁资源信息取出与远程资源进行比对后进行后续处理
            List<ALAssetBundleVersionInfo> allRemoteAssetInfo = _m_lmRemoteResLoadMgr.getAllVersionInfo();
            Dictionary<string, ALAssetBundleVersionInfo> allPatchAssetInfo = _m_piPatchInfo.cloneVersionDic();


            //需要从补丁中删除的资源对象
            List<ALAssetBundleVersionInfo> removePatchAssetList = new List<ALAssetBundleVersionInfo>();
            //需要更新的资源补丁对象
            List<ALAssetBundleVersionInfo> updatePatchAssetList = new List<ALAssetBundleVersionInfo>();
            long totalUpdateSize = 0;
            //遍历远程资源信息，判断哪些需要删除，哪些需要更新
            for (int i = 0; i < allRemoteAssetInfo.Count; i++)
            {
                ALAssetBundleVersionInfo remoteAssetInfo = allRemoteAssetInfo[i];
                if (null == remoteAssetInfo)
                    continue;

                //获取补丁的资源版本号是否匹配
                ALAssetBundleVersionInfo patchInfo = null;
                if(allPatchAssetInfo.ContainsKey(remoteAssetInfo.assetPath))
                {
                    patchInfo = allPatchAssetInfo[remoteAssetInfo.assetPath];
                    //从补丁克隆信息中删除对应信息
                    allPatchAssetInfo.Remove(remoteAssetInfo.assetPath);
                }

                //获取本地打包资源中的版本信息
                ALAssetBundleVersionInfo localInfo = _m_lmLocalResLoadMgr.getVersionInfo(remoteAssetInfo.assetPath);

                //判断文件是否存在，不存在则需要下载
                if (null != patchInfo && !File.Exists(_m_piPatchInfo.assetBundleLoadMgr.getAssetRealURL(patchInfo.assetPath)))
                {
                    //文件不存在则当作数据版本无效
                    patchInfo.versionNum = 0;
                }

                //判断资源是否不需要更新，是则进入下一个循环
                if (null != patchInfo && patchInfo.versionNum == remoteAssetInfo.versionNum)
                    continue;

                //判断本地资源是否为最新，是则不需要更新
                if (null == patchInfo && null != localInfo && localInfo.versionNum == remoteAssetInfo.versionNum)
                    continue;

                //判断是需要更新还是需要删除
                if(null != patchInfo)
                {
                    //如果补丁信息存在则判断本地信息是否最新，是则需要删除补丁信息
                    if (null != localInfo && localInfo.versionNum == remoteAssetInfo.versionNum)
                    {
                        removePatchAssetList.Add(remoteAssetInfo);
                    }
                    else
                    {
                        //判断是否需要更新，如不需要更新则不加入队列
                        if(null == _judgeNeedUpdate || _judgeNeedUpdate(remoteAssetInfo.assetPath))
                        {
                            updatePatchAssetList.Add(remoteAssetInfo);
                            totalUpdateSize += remoteAssetInfo.fileSize;
                        }
                    }
                }
                else
                {
                    //判断是否需要更新，如不需要更新则不加入队列
                    if(null == _judgeNeedUpdate || _judgeNeedUpdate(remoteAssetInfo.assetPath))
                    {
                        //无补丁信息则必须更新
                        updatePatchAssetList.Add(remoteAssetInfo);
                        totalUpdateSize += remoteAssetInfo.fileSize;
                    }
                }
            }

            //判断是否不需要更新
            if (totalUpdateSize <= 0)
            {
                //判断是否有文件需要移除，如果需要则需要在这里移除，避免可能存在文件不被移除
                if (null != removePatchAssetList && removePatchAssetList.Count > 0)
                {
                    //删除需要删除的信息对象
                    for (int i = 0; i < removePatchAssetList.Count; i++)
                    {
                        ALAssetBundleVersionInfo removeInfo = removePatchAssetList[i];
                        _m_piPatchInfo.removePatchAsset(removeInfo.assetPath);
                    }
                    
                    //由于删除了文件，此时强制保存补丁信息
                    _m_piPatchInfo.savePatchInfo();
                }

                if (null != _doneDelegate)
                    _doneDelegate(0);

                return;
            }

            //询问是否继续剩下的操作，是则继续，否则不进行处理
            if (null == _askDelegate)
            {
                _updateFunc(allPatchAssetInfo, removePatchAssetList, updatePatchAssetList, _processDelegate, _doneDelegate);
            }
            else
            {
                _askDelegate(totalUpdateSize, () => { _updateFunc(allPatchAssetInfo, removePatchAssetList, updatePatchAssetList, _processDelegate, _doneDelegate); });
            }
        }
        /*************
         * 实际处理更新的操作函数
         **/
        protected void _updateFunc(
            Dictionary<string, ALAssetBundleVersionInfo> _allPatchAssetInfo, List<ALAssetBundleVersionInfo> _removePatchAssetList, List<ALAssetBundleVersionInfo> _updatePatchAssetList
            , Action<long, long, long, bool> _processDelegate, Action<int> _doneDelegate)
        {
            //剩余的补丁信息对象都是需要删除的资源
            foreach (ALAssetBundleVersionInfo versionInfo in _allPatchAssetInfo.Values)
            {
                _m_piPatchInfo.removePatchAsset(versionInfo.assetPath);
            }

            //删除需要删除的信息对象
            for (int i = 0; i < _removePatchAssetList.Count; i++)
            {
                ALAssetBundleVersionInfo removeInfo = _removePatchAssetList[i];
                _m_piPatchInfo.removePatchAsset(removeInfo.assetPath);
            }

            //由于删除了文件，此时强制保存补丁信息
            _m_piPatchInfo.savePatchInfo();

            //开启剩余下载操作
            if (null != _updatePatchAssetList && _updatePatchAssetList.Count > 0)
            {
                //逐个对象开启加载
                for (int i = 0; i < _updatePatchAssetList.Count; i++)
                {
                    ALAssetBundleVersionInfo updateInfo = _updatePatchAssetList[i];
                    if (null == updateInfo)
                        continue;

#if AL_SEVEN_ZIP
                    //创建保存处理对象
                    if(isRemoteResCompress)
                    {
                        ALResourcePatchUpdateWWWCompressDelegate delegateObj = new ALResourcePatchUpdateWWWCompressDelegate(this, updateInfo);
                        string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(updateInfo);
                        //放入下载处理对象
                        _m_dmHttpDownloadMgr.download(updateInfo.assetPath, _m_lmRemoteResLoadMgr.rootURLProvider.getAssetPath(updateInfo.assetPath), updateInfo.fileSize, tempDownloadLocalFile, delegateObj);

                        //开启解压进度监控，非压缩处理不需要进行这个控制
                        ALResourcePatchUpdateUnzipMonitor unzipMonitor = new ALResourcePatchUpdateUnzipMonitor(_m_dmHttpDownloadMgr, delegateObj);
                        ALMonoTaskMgr.instance.addMonoTask(unzipMonitor);
                    }
                    else
                    {
#endif
                        ALResourcePatchUpdateWWWUnCompressDelegate delegateObj = new ALResourcePatchUpdateWWWUnCompressDelegate(this, updateInfo);
                        string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(updateInfo);
                        //放入下载处理对象
                        _m_dmHttpDownloadMgr.download(updateInfo.assetPath, _m_lmRemoteResLoadMgr.rootURLProvider.getAssetPath(updateInfo.assetPath), updateInfo.fileSize, tempDownloadLocalFile, delegateObj);
#if AL_SEVEN_ZIP
                    }
#endif
                }

                //开启监控操作，监控下载处理对象是否完成，以及进度更新
                ALResourceUpdateMonitor monitor = new ALResourceUpdateMonitor(_m_dmHttpDownloadMgr, _processDelegate, 
                    () => {
                        //保存缓存信息
                        _m_piPatchInfo.savePatchInfo();

                        //调用回调
                        if (null != _doneDelegate)
                        {
                            _doneDelegate(_m_dmHttpDownloadMgr.failCount);
                        }

                        //清理下载信息
                        _m_dmHttpDownloadMgr.clearDownloadInfo();
                    });
                //开启任务
                ALMonoTaskMgr.instance.addMonoTask(monitor);
            }
            else
            {
                //直接调用完成回调
                if (null != _doneDelegate)
                    _doneDelegate(0);
            }
        }

        /*****************
         * 判断是否需要更新资源信息，是则更新资源信息后调用对应的回调函数
         **/
        protected override void _dealLoadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            _dealLoadAsset(false, _assetPath, _delegate, _lateDelegate);
        }
        /********************
         * 必须同步加载资源的处理函数
         **/
        protected override void _dealSynLoadAsset(string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            _dealLoadAsset(true, _assetPath, _delegate, _lateDelegate);
        }

        /*****************
         * 判断是否需要更新资源信息，是则更新资源信息后调用对应的回调函数
         **/
        private void _dealLoadAsset(bool _isNeccessary, string _assetPath, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            //获取远程资源中的对应资源信息
            ALAssetBundleVersionInfo remoteVersionInfo = null;
            if (null != _m_lmRemoteResLoadMgr && _m_lmRemoteResLoadMgr.isEnable)
            {
                //当有远程信息的时候获取到的资源如为空，表示资源无效
                remoteVersionInfo = _m_lmRemoteResLoadMgr.getVersionInfo(_assetPath);
                if (null == remoteVersionInfo)
                {
                    //无版本信息，直接当作失败处理
                    if (null != _delegate)
                        _delegate(false, null);
                    if (null != _lateDelegate)
                        _lateDelegate(false, null);

                    //返回不进行后续处理
                    return;
                }
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
                    if (null != remoteVersionInfo && patchVersionInfo.versionNum != remoteVersionInfo.versionNum)
                    {
                        //当资源对象有效且版本不匹配时才需要删除补丁对象信息
                        _m_piPatchInfo.removePatchAsset(_assetPath);
                        //设置数据更改，保存数据
                        _m_piPatchInfo._setDataChg();
                    }
                    else
                    {
                        //无远程数据或版本号一致则直接加载补丁信息
                        _m_piPatchInfo.loadAsset(_assetPath, _isNeccessary, _delegate, _lateDelegate);
                        //直接返回，不进行后续操作
                        return;
                    }
                }

                //判断是否有打包信息，有则判断版本号是否为最新
                if (null != _m_lmLocalResLoadMgr)
                {
                    //获取本地打包版本信息
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                    if (null != localVersionInfo && (null == remoteVersionInfo || localVersionInfo.versionNum == remoteVersionInfo.versionNum))
                    {
                        _assetDownloadedDelegate lateDelegate = null;
                        //数据有效再声明
                        if(null != _lateDelegate)
                        {
                            lateDelegate =
                                (bool _isSuc, ALAssetBundleObj _assetObj) =>
                                    {
                                        //判断是否成功，只有成功或者没有远程资源版本信息才处理，因为如果失败会需要在delegate中直接进行远程资源尝试
                                        if(!_isSuc && null != remoteVersionInfo)
                                            return;

                                        //如加载成功则直接调用回调 
                                        if(null != _lateDelegate)
                                        {
                                            _lateDelegate(_isSuc, _assetObj);
                                        }
                                    };
                        }

                        //此时版本与本地版本一致直接使用本地加载即可
                        _m_lmLocalResLoadMgr.addLoadAssetInfo(_assetPath, false, _isNeccessary
                            , (bool _isSuc, ALAssetBundleObj _assetObj) =>
                            {
                                //判断是否成功，如果失败则需要调用远程处理
                                if(!_isSuc && null != remoteVersionInfo)
                                {
                                    _loadRmoteResForPatch(remoteVersionInfo, _isNeccessary, _delegate, _lateDelegate);
                                }
                                else
                                {
                                    //如加载成功则直接调用回调 
                                    if(null != _delegate)
                                    {
                                        _delegate(_isSuc, _assetObj);
                                    }
                                }
                            }
                            , lateDelegate);
                        return;
                    }
                }

                //如本地信息也不是最新，此时开启下载并在下载后，通过补丁管理器对象进行加载
                _loadRmoteResForPatch(remoteVersionInfo, _isNeccessary, _delegate, _lateDelegate);
            }
            else
            {
                //此处无Patch对象，表示不需要更新之后缓存，因此直接调用remote进行加载处理
                //判断是否有打包信息，有则判断版本号是否为最新
                if (null != _m_lmLocalResLoadMgr)
                {
                    //获取本地打包版本信息
                    ALAssetBundleVersionInfo localVersionInfo = _m_lmLocalResLoadMgr.getVersionInfo(_assetPath);
                    if (null == localVersionInfo || (null != remoteVersionInfo && localVersionInfo.versionNum != remoteVersionInfo.versionNum))
                    {
                        //本地无信息或不为最新，则直接开启加载
                        if (null != _m_lmRemoteResLoadMgr)
                        {
                            _m_lmRemoteResLoadMgr.addLoadAssetInfo(_assetPath, false, _isNeccessary, _delegate, _lateDelegate);
                        }
                        else
                        {
                            //直接当作失败处理
                            if (null != _delegate)
                                _delegate(false, null);
                        }
                    }
                    else
                    {
                        //此时版本与本地版本一致直接使用本地加载即可
                        _m_lmLocalResLoadMgr.addLoadAssetInfo(_assetPath, false, _isNeccessary, _delegate, _lateDelegate);
                    }
                }
                else
                {
                    //本地对象为空，则直接开启远程加载
                    if (null != _m_lmRemoteResLoadMgr)
                    {
                        _m_lmRemoteResLoadMgr.addLoadAssetInfo(_assetPath, false, _isNeccessary, _delegate, _lateDelegate);
                    }
                    else
                    {
                        //直接当作失败处理
                        if (null != _delegate)
                            _delegate(false, null);
                    }
                }
            }
        }

        /// <summary>
        /// 从远程资源下载文件，并写入补丁包中
        /// </summary>
        /// <param name="_remoteVersionInfo"></param>
        /// <param name="_isNeccessary"></param>
        /// <param name="_delegate"></param>
        /// <param name="_lateDelegate"></param>
        private void _loadRmoteResForPatch(ALAssetBundleVersionInfo _remoteVersionInfo, bool _isNeccessary, _assetDownloadedDelegate _delegate, _assetDownloadedDelegate _lateDelegate)
        {
            //调用事件函数
            _onLoadResWhichIsNotNewest(_remoteVersionInfo);

            //如本地信息也不是最新，此时开启下载并在下载后，通过补丁管理器对象进行加载
            if(null != _m_lmRemoteResLoadMgr && null != _remoteVersionInfo)
            {
#if AL_SEVEN_ZIP
                if(isRemoteResCompress)
                {
                    //创建保存处理对象
                    ALResourcePatchLoadCompressWWWDelegate delegateObj = new ALResourcePatchLoadCompressWWWDelegate(_isNeccessary, this, _remoteVersionInfo, _delegate, _lateDelegate);
                    string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(_remoteVersionInfo);
                    //放入下载处理对象
                    _m_dmHttpDownloadMgr.download(_remoteVersionInfo.assetPath, _m_lmRemoteResLoadMgr.rootURLProvider.getAssetPath(_remoteVersionInfo.assetPath), _remoteVersionInfo.fileSize, tempDownloadLocalFile, delegateObj);
                }
                else
                {
#endif
                    //创建保存处理对象
                    ALResourcePatchLoadUnCompressWWWDelegate delegateObj = new ALResourcePatchLoadUnCompressWWWDelegate(_isNeccessary, this, _remoteVersionInfo, _delegate, _lateDelegate);
                    string tempDownloadLocalFile = _m_piPatchInfo.getTempDownloadLocalFile(_remoteVersionInfo);
                    //放入下载处理对象
                    _m_dmHttpDownloadMgr.download(_remoteVersionInfo.assetPath, _m_lmRemoteResLoadMgr.rootURLProvider.getAssetPath(_remoteVersionInfo.assetPath), _remoteVersionInfo.fileSize, tempDownloadLocalFile, delegateObj);
#if AL_SEVEN_ZIP
                }
#endif
            }
            else
            {
                //直接当作失败处理
                if(null != _delegate)
                    _delegate(false, null);
            }
        }

        /// <summary>
        /// 在有资源不是最新且需要更新远程资源的时候的事件函数
        /// </summary>
        /// <param name="_versionInfo"></param>
        protected abstract void _onLoadResWhichIsNotNewest(ALAssetBundleVersionInfo _versionInfo);
    }
}
