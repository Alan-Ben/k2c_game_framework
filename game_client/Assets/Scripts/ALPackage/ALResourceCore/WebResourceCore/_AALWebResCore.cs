
using System;
using System.IO;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 远程资源的数据管理核心，会将远程资源下载，缓存于本地，提供本地缓存的资源路径给外部使用
    /// </summary>
    public abstract class _AALWebResCore
    {
        protected string remoteResRootURL { get { return _m_sRemoteResRootURL; } }
        protected string localFileRootURL { get { return _m_sLocalFileRootURL; } }
        protected string streamingAssetsRootURL { get { return _m_sStreamingAssetsFileRootURL; } }

        // 远程资源的下载管理器
        protected ALHttpDownloadMgr _m_lFileDownloadDealer;

        // 远程资源文件路径的根目录
        private string _m_sRemoteResRootURL;
        // 本地缓存文件路径的根目录
        private string _m_sLocalFileRootURL;
        // 随包文件路径的根目录
        private string _m_sStreamingAssetsFileRootURL;

        // 下载失败时重试的次数
        private int _m_iRetryCount;

        // 这个对象是否已经被销毁了
        private bool _m_bIsDiscarded;

        // 资源版本的比对器
        protected _AALMD5FileVersionChecker _m_cMD5VersionChecker
        {
            get
            {
                if (__m_cMD5VersionChecker == null)
                    __m_cMD5VersionChecker = _createMD5FileVersionChecker();
                return __m_cMD5VersionChecker;
            }
        }
        private _AALMD5FileVersionChecker __m_cMD5VersionChecker;



        /// <param name="_remoteResRoot">远程资源的根目录</param>
        /// <param name="_localCacheRoot">本地缓存资源的根目录</param>
        /// <param name="_retryFailCount">下载失败后重试的次数</param>
        public _AALWebResCore(string _remoteResRoot, string _localCacheRoot, string _streamingAssetsRoot, int _failRetryCount, int _downloadMaxDealingCount = 3)
        {
            //创建下载对象对象
            //获取当前系统安全的下载过程大小
            long safeLoadingSizeMB = 10;
            if (SystemInfo.systemMemorySize > 720)
                safeLoadingSizeMB = 30;
            if (SystemInfo.systemMemorySize > 1024)
                safeLoadingSizeMB = 50;
            if (SystemInfo.systemMemorySize > 1720)
                safeLoadingSizeMB = 200;
            // retryCount写0，内部的重试不用，重试在外部进行使用 
            _m_lFileDownloadDealer = new ALHttpDownloadMgr(0, safeLoadingSizeMB * 1024 * 1024, _downloadMaxDealingCount);

            // 保证目录是最后带斜杠的
            _m_sLocalFileRootURL = ALCommon.directoryInsure(_localCacheRoot);
            _m_sRemoteResRootURL = ALCommon.directoryInsure(_remoteResRoot);
            _m_sStreamingAssetsFileRootURL = ALCommon.directoryInsure(_streamingAssetsRoot);
            _m_iRetryCount = _failRetryCount;

            _m_bIsDiscarded = false;
        }



        /// <summary>
        /// 这个销毁是不可逆的
        /// </summary>
        public void discard()
        {
            _m_lFileDownloadDealer.discard();
            __m_cMD5VersionChecker?.discard();
            __m_cMD5VersionChecker = null;

            _m_bIsDiscarded = true;
        }



        /// <summary>
        /// 从缓存或者远端中获取对应资源，传入的_resUrl是相对路径
        /// </summary>
        public void loadFromCacheOrDownload(string _resUrl, _AALWebResCoreLoadedDealer _loadDelegate)
        {
            if (_m_bIsDiscarded)
            {
                Debug.LogError($"[{GetType().FullName}] 这个对象已经被销毁了，它的discard方法是不可逆的");
                _loadDelegate?.onLoadFail();
                return;
            }

            if (_m_cMD5VersionChecker == null)
            {
                Debug.LogError($"[{GetType().FullName}] 资源版本比对器是空");
                _loadDelegate?.onLoadFail();
                return;
            }

            // 检查版本并做之后的处理
            // 先比对资源版本是否一致，如果不一致要重新下载
            _m_cMD5VersionChecker.checkResVersion(_resUrl, result =>
            {
                switch (result)
                {
                    case _AALMD5FileVersionChecker.VersionCheckResult.DeleteLocalFile:
                        // 删除本地文件

                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] MD5比对结果，要删除下载的本地文件");

                        deleteLocalCache(_resUrl);
                        _loadDelegate?.onLoadFail();
                        break;
                    case _AALMD5FileVersionChecker.VersionCheckResult.UpdateFromRemote:
                        // 下载远端文件到本地并使用

                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] MD5比对结果，远端的文件有更新，要更新远端文件到本地");

                        _m_cMD5VersionChecker.tryGetRemoteFileSizeByte(_resUrl, fileSize =>
                        {
                            _downloadAndDeal(_resUrl, fileSize, _m_iRetryCount, _loadDelegate);
                        });
                        break;
                    case _AALMD5FileVersionChecker.VersionCheckResult.UseLocalFile:
                        // 直接使用本地文件返回

                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] MD5比对结果，比对通过，直接使用本地的路径");

                        // 判断本地的资源文件到底存不存在，如果存在则返回，不存在则转去下载
                        if (File.Exists(_m_sLocalFileRootURL + _resUrl))
                            _loadDelegate?.onLoadSuccess(_m_sLocalFileRootURL + _resUrl);
                        else
                        {
                            _m_cMD5VersionChecker.tryGetRemoteFileSizeByte(_resUrl, fileSize =>
                            {
                                _downloadAndDeal(_resUrl, fileSize, _m_iRetryCount, _loadDelegate);
                            });
                        }
                        break;
                    case _AALMD5FileVersionChecker.VersionCheckResult.UseStreamingAssetsFile:
                        // 直接使用本地文件返回

                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] MD5比对结果，比对通过，直接使用本地的路径");

                        _loadDelegate?.onLoadSuccess(_m_sStreamingAssetsFileRootURL + _resUrl);
                        break;
                    case _AALMD5FileVersionChecker.VersionCheckResult.FileNotExist:
                        //if (_AALMonoMain.instance.showDebugOutput)
                        //{
                        Debug.Log($"[{GetType().FullName}] 尝试下载了一个无论是远端还是本地都不存在的文件：{_resUrl}");
                        //}
                        _loadDelegate?.onLoadFail();
                        break;
                    default:
                        Debug.LogError($"[{GetType().FullName}] 资源版本比对返回出问题了，请连续程序，它返回了{result}");
                        _loadDelegate?.onLoadFail();
                        break;
                }
            });
        }



        // 删除本地文件
        public void deleteLocalCache(string _resUrl)
        {
            if (_m_bIsDiscarded)
            {
                Debug.LogError($"[{GetType().FullName}] 这个对象已经被销毁了，它的discard方法是不可逆的");
                return;
            }

            try
            {
                ALFile.Delete(_m_sLocalFileRootURL + _resUrl);
                _m_cMD5VersionChecker.deleteLocalFileMD5(_resUrl);
            }
            catch (Exception _ex)
            {
                Debug.LogError($"[{GetType().FullName}] 尝试删除本地文件错误：{_m_sLocalFileRootURL + _resUrl}\n" + _ex.ToString());
            }
        }



        // 下载远程资源并做之后的处理
        private void _downloadAndDeal(string _resUrl, long _fileSize, int _retryCountRemain, _AALWebResCoreLoadedDealer _loadDelegate)
        {
            // 如果这个对象被销毁了，那么不进行后续操作
            if (_m_bIsDiscarded)
                return;


            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[{GetType().FullName}] 开始下载远端的文件，路径是{_m_sRemoteResRootURL + _resUrl}\n下载到本地的路径是{_m_sLocalFileRootURL + _resUrl}");

            _HttpDownloadDelegate downloadDelegate = new _HttpDownloadDelegate(this, _retryCountRemain, _resUrl, _fileSize, _loadDelegate);
            _m_lFileDownloadDealer.download(_resUrl, _m_sRemoteResRootURL + _resUrl, _fileSize, _m_sLocalFileRootURL + _resUrl, downloadDelegate);
        }



        // 交由子类生成对应的版本检查器
        protected abstract _AALMD5FileVersionChecker _createMD5FileVersionChecker();



        /// <summary>
        /// 内部Http下载的回调
        /// 使用了_AALWebResCore对象的内部处理函数，不对外开放
        /// </summary>
        private class _HttpDownloadDelegate : _AALHttpDownloadDelegate
        {
            private _AALWebResCore _m_cResCore;
            private int _m_iRetryCount;
            private string _m_sResUrl;
            private long _m_lFileSize;
            private _AALWebResCoreLoadedDealer _m_dLoadDelegate;

            public _HttpDownloadDelegate(_AALWebResCore _resCore, int _retryCountRemain, string _resUrl, long _fileSize, _AALWebResCoreLoadedDealer _loadDelegate) 
                : base()
            {
                _m_cResCore = _resCore;
                _m_iRetryCount = _retryCountRemain;
                _m_sResUrl = _resUrl;
                _m_lFileSize = _fileSize;
                _m_dLoadDelegate = _loadDelegate;
            }

            // 下载出错，出错后根据重试机会，进行重试
            protected internal override void _onHttpFail()
            {
                if (_m_iRetryCount > 0)
                {
                    _m_iRetryCount--;
                    if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                    {
                        Debug.LogError($"[{_m_cResCore.GetType().FullName}] 下载文件出错，重试次数还剩余{_m_iRetryCount}次，现在尝试重新下载：{_m_cResCore._m_sRemoteResRootURL + _m_sResUrl}");
                    }
                    _m_cResCore._downloadAndDeal(_m_sResUrl, _m_lFileSize, _m_iRetryCount, _m_dLoadDelegate);
                }
                else
                {
                    if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                        Debug.Log($"[{GetType().FullName}] 下载文件失败");

                    //失败会调用外部的函数，这里加上task回主线程执行
                    if (_m_dLoadDelegate != null)
                        ALCommonActionMonoTask.addMonoTask(_m_dLoadDelegate.onLoadFail);
                }
            }

            protected internal override void _onHttpLoaded(string _localPath)
            {
                FileInfo file = null;
                try
                {
                    file = new FileInfo(_m_cResCore._m_sLocalFileRootURL + _m_sResUrl);
                    if (!file.Exists)
                    {
                        Debug.LogError($"[{_m_cResCore.GetType().FullName}] 尝试打开下载好的本地文件不存在：{_m_cResCore._m_sLocalFileRootURL + _m_sResUrl}\n");
                        file = null;
                    }
                }
                catch(Exception _ex)
                {
                    Debug.LogError($"[{_m_cResCore.GetType().FullName}] 尝试打开下载好的本地文件错误：{_m_cResCore._m_sLocalFileRootURL + _m_sResUrl}\n" + _ex.ToString());
                    file = null;
                }

                // 报错会导致file是null，然后就走重试逻辑
                if (file == null)
                {
                    _onHttpFail();
                    return;
                }

                // 文件正常的话，生成MD5值并做最后的校验
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(2);

                string localMD5 = string.Empty;
                string remoteMD5 = string.Empty;
                // 生成下载下来的文件的md5信息，并将md5信息保存在本地
                _m_cResCore._m_cMD5VersionChecker.generateLocalFileMD5(file, _m_sResUrl, _md5 =>
                {
                    //这里是主线程
                    localMD5 = _md5;
                    stepCounter.addDoneStepCount();
                });
                // 尝试获取远端文件的md5信息
                _m_cResCore._m_cMD5VersionChecker.tryGetRemoteFileMD5(_m_sResUrl, _md5 =>
                {
                    //这里不一定，看子类怎么实现
                    remoteMD5 = _md5;
                    stepCounter.addDoneStepCount();
                });

                stepCounter.regAllDoneDelegate(() =>
                {
                    if (localMD5 == remoteMD5)
                    {
                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] 下载文件后的文件和远端的MD5比对成功，md5值分别是local:{localMD5}，remote:{remoteMD5}");

                        //调用外部方法，回主线程防止错误
                        if (_m_dLoadDelegate != null)
                            ALCommonActionMonoTask.addMonoTask(() => { _m_dLoadDelegate.onLoadSuccess(_m_cResCore._m_sLocalFileRootURL + _m_sResUrl); });
                    }
                    else // 如果下载下来的文件md5值和远端不一致，那么就当做出错处理
                    {

                        if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                            Debug.Log($"[{GetType().FullName}] 下载文件后的文件和远端的MD5比对失败，md5值分别是local:{localMD5}，remote:{remoteMD5}");

                        _onHttpFail();
                    }
                });
                
            }
        }
    }
}