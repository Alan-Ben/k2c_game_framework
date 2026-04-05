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
    public abstract class _AALAsyncResourceCore : _AALResourceCore
    {
        //创建单纯从远程下载资源的处理对象
        public _AALAsyncResourceCore(string _remoteURL, bool _useCache)
            : base(_remoteURL, _useCache)
        {
        }
        //创建单纯从远程下载资源的处理对象
        public _AALAsyncResourceCore(string _remoteURL, int _maxLoadingCount, int _maxRetryCount, bool _useCache)
            : base(_remoteURL, _maxLoadingCount, _maxRetryCount, _useCache)
        {
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALAsyncResourceCore(string _remoteURL, string _localPatchFolerPath)
            : base(_remoteURL, _localPatchFolerPath)
        {
        }
        //创建从远程下载资源并存放到本地对应数据位置的处理对象
        public _AALAsyncResourceCore(string _remoteURL, string _localPatchFolerPath, int _maxLoadingCount, int _maxRetryCount)
            : base(_remoteURL, _localPatchFolerPath, _maxLoadingCount, _maxRetryCount)
        {
        }
        //创建仅有本地资源处理对象的数据处理对象
        public _AALAsyncResourceCore(string _localPatchFolerPath)
            : base(_localPatchFolerPath)
        {
        }
        //创建仅有本地资源处理对象的数据处理对象
        public _AALAsyncResourceCore(string _localPatchFolerPath, int _maxRetryCount)
            : base(_localPatchFolerPath, _maxRetryCount)
        {
        }

        /// <summary>
        /// 获取对应资源的下载进度万分比
        /// </summary>
        /// <param name="_asset"></param>
        /// <returns></returns>
        public long getDownLoadProcess(string _asset)
        {
            if(null == _m_dmHttpDownloadMgr)
                return 0;

            return _m_dmHttpDownloadMgr.getAssetProgressTenThouthan(_m_lmRemoteResLoadMgr.rootURLProvider.getAssetPath(_asset));
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
                if(null != patchVersionInfo)
                {
                    if (null != remoteVersionInfo && patchVersionInfo.versionNum != remoteVersionInfo.versionNum)
                    {
                        //当资源对象有效且版本不匹配时才需要删除补丁对象信息
                        _m_piPatchInfo.removePatchAsset(_assetPath);
                        //设置数据更改，保存数据
                        _m_piPatchInfo._setDataChg();
                    }
                    else if(!File.Exists(_m_piPatchInfo.assetBundleLoadMgr.getAssetRealURL(patchVersionInfo.assetPath)))
                    {
                        //当资源文件对象不存在时需要删除补丁对象信息
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
                        _assetDownloadedDelegate failDelegate = null;
                        //数据有效再声明
                        if(null != _lateDelegate)
                        {
                            failDelegate =
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
                            , failDelegate);
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
                if(null != _m_lmLocalResLoadMgr)
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
    }
}
