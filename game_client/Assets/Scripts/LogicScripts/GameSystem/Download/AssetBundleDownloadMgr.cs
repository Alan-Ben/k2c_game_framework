using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// AssetBundle 下载管理器
    /// </summary>
    public class AssetBundleDownloadMgr
    {
        // 管理资源下载和加载的管理器
        private _AALResourceCore _m_resCore;
        // 资源的路径和目录列表
        private readonly List<string> _m_pathList;

        // 当前已下载的资源
        [ItemNotNull, NotNull] private readonly List<string> _m_loadedAssetList;
        // 当前未下载的资源
        [ItemNotNull, NotNull] private readonly List<string> _m_unLoadedAssetList;
        // 加载失败的资源列表
        [ItemNotNull, NotNull] private readonly List<string> _m_loadFailedAssetList;
        
        // 是否正在下载
        private bool _m_isDownloading;
        // 总共的大小和已经下载的大小
        private long _m_totalSize;
        private long _m_loadedSize;
        // 总共的文件数和已经下载的文件数
        private int _m_totalFileCount;
        private int _m_loadedFileCount;
        // 当前正在下载的数据内容
        private _LoadingObj _m_loadingObj;
        private Action _m_loadComplete;


        public AssetBundleDownloadMgr(_AALResourceCore _resCore, List<string> _packPaths)
        {
            _m_resCore = _resCore;
            _m_pathList = _packPaths;
            
            _m_loadedAssetList = new List<string>();
            _m_unLoadedAssetList = new List<string>();
            _m_loadFailedAssetList = new List<string>();

            _refreshAssetState();
        }
        

        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool isDownloading { get { return _m_isDownloading; } }
        /// <summary>
        /// 总大小
        /// </summary>
        public long totalSize { get { return _m_totalSize; } }
        /// <summary>
        /// 已下载大小
        /// </summary>
        public long loadedSize { get { return _m_loadedSize + (_m_loadingObj?.loadedSize ?? 0); } }
        /// <summary>
        /// 进度
        /// </summary>
        public float progress { get { return (float)loadedSize / totalSize; } }
        /// <summary>
        /// 总文件数
        /// </summary>
        public int totalFileCount { get { return _m_totalFileCount; } }
        /// <summary>
        /// 已下载文件数
        /// </summary>
        public int loadedFileCount { get { return _m_loadedFileCount; } }
        /// <summary>
        /// 下载失败文件数
        /// </summary>
        public int loadFailedFileCount { get { return _m_loadFailedAssetList.Count; } }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isComplete { get { return loadedFileCount >= totalFileCount; } }


        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="_complete">这个下载完成的回调如果调用了 abort 后，会被作废</param>
        public void startDownload(Action _complete = null)
        {
            if (_m_resCore == null)
            {
                ALLog.Warning("AssetBundleDownloadMgr.startDownload: _AALResourceCore is null.");
                return;
            }
            if (_m_isDownloading)
            {
                ALLog.Warning("AssetBundleDownloadMgr.startDownload: AssetBundleDownloadMgr is already downloading.");
                return;
            }

            _m_isDownloading = true;
            _m_loadComplete = _complete;
            
            // 把上一轮下载失败的资源重新加入到未下载资源列表中
            _m_unLoadedAssetList.AddRange(_m_loadFailedAssetList);
            _m_loadFailedAssetList.Clear();

            // 开始尝试下载
            _tryDownloadNext();
        }
        /// <summary>
        /// 中止下载
        /// </summary>
        public void abortDownload()
        {
            if (!_m_isDownloading)
            {
                ALLog.Warning("AssetBundleDownloadMgr.abortDownload: AssetBundleDownloadMgr is not downloading.");
                return;
            }

            _m_isDownloading = false;
        }
        /// <summary>
        /// 重新检查一下未下载的资源是不是在别的地方被偷偷下好了
        /// </summary>
        public void checkUnloadedAsset()
        {
            if (_m_resCore == null)
            {
                ALLog.Warning("AssetBundleDownloadMgr.checkUnloadedAsset: _AALResourceCore is null.");
                return;
            }
            
            // 检查一遍未下载的列表
            for (int i = _m_unLoadedAssetList.Count - 1; i >= 0; i--)
            {
                string assetPath = _m_unLoadedAssetList[i];
                if (_m_resCore.isLocalAssetNewest(assetPath))
                {
                    _m_unLoadedAssetList.RemoveAt(i);
                    _m_loadedAssetList.Add(assetPath);
                    ALAssetBundleVersionInfo versionInfo = _m_resCore.getVersionInfo(assetPath);
                    _m_loadedSize += versionInfo.fileSize;
                    _m_loadedFileCount += 1;
                }
            }

            // 检查一遍下载失败的列表
            for (int i = _m_loadFailedAssetList.Count - 1; i >= 0; i--)
            {
                string assetPath = _m_loadFailedAssetList[i];
                if (_m_resCore.isLocalAssetNewest(assetPath))
                {
                    _m_loadFailedAssetList.RemoveAt(i);
                    _m_loadedAssetList.Add(assetPath);
                    ALAssetBundleVersionInfo versionInfo = _m_resCore.getVersionInfo(assetPath);
                    _m_loadedSize += versionInfo.fileSize;
                    _m_loadedFileCount += 1;
                }
            }
        }


        /// <summary>
        /// 刷新所有资源状态
        /// </summary>
        private void _refreshAssetState()
        {
            _m_loadedAssetList.Clear();
            _m_unLoadedAssetList.Clear();
            _m_loadFailedAssetList.Clear();

            if (_m_resCore == null || _m_pathList == null)
                return;

            // 使用 HashSet 来去重
            HashSet<string> assetPathList = new HashSet<string>();
            // 遍历传入的路径列表，获取所有资源路径
            foreach (string path in _m_pathList)
            {
                List<string> unitAssetPathList = _m_resCore.getAssetPaths(path);
                if (unitAssetPathList == null)
                    continue;
                
                foreach (string assetPath in unitAssetPathList)
                {
                    assetPathList.Add(assetPath);
                }
            }
            foreach (string assetPath in assetPathList)
            {
                ALAssetBundleVersionInfo versionInfo = _m_resCore.getVersionInfo(assetPath);
                if (versionInfo == null)
                    continue;

                _m_totalSize += versionInfo.fileSize;
                _m_totalFileCount += 1;
                if (_m_resCore.isLocalAssetNewest(assetPath))
                {
                    _m_loadedAssetList.Add(assetPath);
                    _m_loadedSize += versionInfo.fileSize;
                    _m_loadedFileCount += 1;
                }
                else
                    _m_unLoadedAssetList.Add(assetPath);
            }
        }
        /// <summary>
        /// 尝试下载下一个资源
        /// </summary>
        private void _tryDownloadNext()
        {
            // 如果 resCore 没了就返回
            if (_m_resCore == null)
                return;
            // 如果当前不在下载状态，或者正在下载就不处理
            if (!_m_isDownloading || _m_loadingObj != null)
                return;

            if (_m_unLoadedAssetList.Count == 0)
            {
                _m_isDownloading = false;
                ALLog.Sys("AssetBundleDownloadMgr: All asset download complete.");
                
                _m_loadComplete?.Invoke();
                _m_loadComplete = null;
                
                WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
                
                return;
            }

            // 从未下载资源列表中取出第一个资源进行下载
            string loadAsset = _m_unLoadedAssetList.GetFirstAndRemove();
            ALAssetBundleVersionInfo versionInfo = _m_resCore.getVersionInfo(loadAsset);
            _m_loadingObj = new _LoadingObj(_m_resCore, loadAsset);
            _m_loadingObj.loadAsset(_isSuc =>
            {
                if (_isSuc)
                {
                    _m_loadedAssetList.Add(loadAsset);
                    _m_loadedSize += versionInfo.fileSize;
                    _m_loadedFileCount += 1;
                }
                else
                    _m_loadFailedAssetList.Add(loadAsset);
                
                _m_loadingObj = null;
                _tryDownloadNext();
            });
        }
        
        /// <summary>
        /// 下载对象
        /// </summary>
        private class _LoadingObj
        {
            [NotNull] private readonly _AALResourceCore _m_resCore;
            [NotNull] private readonly ALAssetBundleVersionInfo _m_versionInfo;
            
            private _IALLoadingAssetInterface _m_loadingInfo;
            
            
            public _LoadingObj([NotNull] _AALResourceCore _resCore, string _assetPath)
            {
                _m_resCore = _resCore;
                _m_versionInfo = _resCore.getVersionInfo(_assetPath);
            }
            

            public long loadedSize { get { return (long) progress * _m_versionInfo.fileSize; } }
            public float progress { get { if (_m_loadingInfo == null) return 0; return _m_loadingInfo.progress; } }


            public void loadAsset([NotNull] Action<bool> _action)
            {
                _m_resCore.loadAsset(_m_versionInfo.assetPath, (_isSuc, _) => _action.Invoke(_isSuc), null);
                _m_loadingInfo = _m_resCore.getAssetLoadingInfo(_m_versionInfo.assetPath);
            }
        }
    }
}