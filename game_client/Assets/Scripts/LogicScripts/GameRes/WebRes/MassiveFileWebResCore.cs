using System;
using System.Collections.Generic;
using ALPackage;


    /// <summary>
    /// 拥有大量数据的一个WebResCore
    /// </summary>
    public class MassiveFileWebResCore : _AFileWebResCore
    {
        private string _m_sRemoteDBPath;
        private string _m_sLocalDBPath;
        private string _m_sStreamingAssetsDBPath;
        private string _m_sStreamingAssetsLocalDBPath;

        MD5FileVersionChecker_UseFile _m_cFileMD5VersionChecker;

        public MassiveFileWebResCore(string _remoteDBPath, string _localDBPath, string _streamingAssetsDBPath,
            string _streamingAssetLocalDBPath
            , string _remoteRootPath, string _localRootPath, string _streamingAssetsRootPath, int _failRetryCount,
            int _downloadMaxDealingCount = 3)
            : base(_remoteRootPath, _localRootPath, _streamingAssetsRootPath, _failRetryCount, _downloadMaxDealingCount)
        {
            _m_sRemoteDBPath = _remoteDBPath;
            _m_sLocalDBPath = _localDBPath;
            _m_sStreamingAssetsDBPath = _streamingAssetsDBPath;
            _m_sStreamingAssetsLocalDBPath = _streamingAssetLocalDBPath;
        }


        public void init(Action _onInitDone)
        {
            if (_m_cMD5VersionChecker == null || _m_cFileMD5VersionChecker == null)
            {
                Debug.LogError($"[{GetType().FullName}] 资源版本比对器是空");
                _onInitDone?.Invoke();
                return;
            }
            _m_cFileMD5VersionChecker.init(_onInitDone);
        }

        public void updateAllRes(Action<long, long, long, bool> _processDelegate, Action<int> _doneDelegate,
            Action<long, Action> _askDelegate)
        {
            if (_m_cMD5VersionChecker == null || _m_cFileMD5VersionChecker == null)
            {
                Debug.LogError($"[{GetType().FullName}] 资源版本比对器是空");
                return;
            }

            List<MD5VersionObj> remoteVersions = _m_cFileMD5VersionChecker.getRemoteVersions();
            List<MD5VersionObj> localVersions = _m_cFileMD5VersionChecker.getLocalVersions();
            
            long totalUpdateSize = 0;

            List<MD5VersionObj> needUpdateVersionObjs = new List<MD5VersionObj>();
            List<MD5VersionObj> removeVersionObjs = new List<MD5VersionObj>();
            if (remoteVersions != null)
                for (int i = 0; i < remoteVersions.Count; i++)
                {
                    MD5VersionObj remote = remoteVersions[i];
                    if (remote == null) continue;
                    _m_cFileMD5VersionChecker.checkResVersion(remote.fileKey, _result =>
                    {
                        if (_result == _IALMD5FileVersionChecker.VersionCheckResult.UpdateFromRemote)
                        {
                            totalUpdateSize += remote.fileSize;
                            needUpdateVersionObjs.Add(remote);
                        }
                    });
                }

            // 如果远端版本信息有，才处理本地版本删除操作
            if (localVersions != null && remoteVersions != null && remoteVersions.Count > 0)
                for (int i = 0; i < localVersions.Count; i++)
                {
                    MD5VersionObj localVersion = localVersions[i];
                    if (localVersion == null) continue;
                    if (_m_cFileMD5VersionChecker.needDeleteLocalVersionObjFile(localVersion))
                    {
                        removeVersionObjs.Add(localVersion);
                    }
                }

            if (null == _askDelegate)
            {
                _updateFunc(needUpdateVersionObjs, removeVersionObjs, _processDelegate, _doneDelegate);
            }
            else
            {
                _askDelegate(totalUpdateSize,
                    () =>
                    {
                        _updateFunc(needUpdateVersionObjs, removeVersionObjs, _processDelegate, _doneDelegate);
                    });
            }
        }

        private void _updateFunc(List<MD5VersionObj> _updatePatchAssetList, List<MD5VersionObj> _removeAssetList,
            Action<long, long, long, bool> _processDelegate, Action<int> _doneDelegate)
        {
            // 删除本地不需要的资源
            if (_removeAssetList != null)
                foreach (MD5VersionObj versionObj in _removeAssetList)
                {
                    if (versionObj != null)
                        deleteLocalCache(versionObj.fileKey);
                }

            //开启剩余下载操作
            if (null != _updatePatchAssetList && _updatePatchAssetList.Count > 0)
            {
                foreach (MD5VersionObj versionObj in _updatePatchAssetList)
                {
                    if (versionObj != null)
                        loadFromCacheOrDownload(versionObj.fileKey, null);
                }

                //开启监控操作，监控下载处理对象是否完成，以及进度更新
                ALResourceUpdateMonitor monitor = new ALResourceUpdateMonitor(_m_lFileDownloadDealer, _processDelegate,
                    () =>
                    {

                        //调用回调
                        if (null != _doneDelegate)
                        {
                            _doneDelegate(_m_lFileDownloadDealer.failCount);
                        }

                        //清理下载信息
                        _m_lFileDownloadDealer.clearDownloadInfo();
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

        // 数据量巨大的Core，使用SQLite来管理版本信息
        protected override _IALMD5FileVersionChecker _createMD5FileVersionChecker()
        {
            _m_cFileMD5VersionChecker = new MD5FileVersionChecker_UseFile(_m_sRemoteDBPath, _m_sLocalDBPath,
                _m_sStreamingAssetsDBPath, _m_sStreamingAssetsLocalDBPath);
            return _m_cFileMD5VersionChecker;
        }
    }
