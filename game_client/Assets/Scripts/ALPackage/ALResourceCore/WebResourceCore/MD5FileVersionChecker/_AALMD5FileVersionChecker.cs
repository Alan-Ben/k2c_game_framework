
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 基于MD5的文件版本比对器:
    ///     该比对器会对本地的文件生成MD5数据并存档，进行版本比对时会将这个数据和远端的数据进行比对，以此判断远端的版本状态
    /// </summary>
    public abstract class _AALMD5FileVersionChecker
    {
        // 远程资源数据源的缓存，不用每次去数据源中获取
        private Dictionary<string, MD5VersionObj> _m_dRemoteCache;
        // 本地资源数据源的缓存，不用每次去数据源中获取
        private Dictionary<string, MD5VersionObj> _m_dLocalCache;
        // 随包资源数据源的缓存，不用每次去数据源中获取
        private Dictionary<string, MD5VersionObj> _m_dStreamingAssetsCache;

        public _AALMD5FileVersionChecker()
        {
            _m_dRemoteCache = new Dictionary<string, MD5VersionObj>();
            _m_dLocalCache = new Dictionary<string, MD5VersionObj>();
            _m_dStreamingAssetsCache = new Dictionary<string, MD5VersionObj>();
        }



        public void clearCache()
        {
            _m_dRemoteCache.Clear();
            _m_dLocalCache.Clear();
            _m_dStreamingAssetsCache.Clear();
        }



        public void discard()
        {
            clearCache();
            _onDiscard();
        }


        
        /// <summary>
        /// 尝试获取对应远程文件的md5
        /// </summary>
        public void tryGetRemoteFileMD5(string _fileKey, Action<string> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dRemoteCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getRemoteVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dRemoteCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? string.Empty : _md5Obj.fileMD5);
                });
            }
            else
            {
                _got(_obj == null ? string.Empty : _obj.fileMD5);
            }
        }
        /// <summary>
        /// 尝试获取对应远程文件的大小
        /// </summary>
        public void tryGetRemoteFileSizeByte(string _fileKey, Action<long> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dRemoteCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getRemoteVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dRemoteCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? 0L : _md5Obj.fileSize);
                });
            }
            else
            {
                _got(_obj == null ? 0L : _obj.fileSize);
            }
        }



        /// <summary>
        /// 尝试获取对应本地文件的md5
        /// </summary>
        public void tryGetLocalFileMD5(string _fileKey, Action<string> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dLocalCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getLocalVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dLocalCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? string.Empty : _md5Obj.fileMD5);
                });
            }
            else
            {
                _got(_obj == null ? string.Empty : _obj.fileMD5);
            }
        }
        /// <summary>
        /// 尝试获取对应本地文件的大小
        /// </summary>
        public void tryGetLocalFileSizeByte(string _fileKey, Action<long> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dLocalCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getLocalVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dLocalCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? 0L : _md5Obj.fileSize);
                });
            }
            else
            {
                _got(_obj == null ? 0L : _obj.fileSize);
            }
        }
        /// <summary>
        /// 为对应的本地文件生成MD5值，并根据fileKey保存到本地的数据源中
        /// </summary>
        public void generateLocalFileMD5(FileInfo _file, string _fileKey, Action<string> _onComplete)
        {
            Thread thread = new Thread(_generateLocalFileMD5Thread);
            thread.Start(new object[] { _file, _fileKey, _onComplete });
        }
        private void _generateLocalFileMD5Thread(object _obj)
        {
            object[] paramList = _obj as object[];
            if (paramList == null || paramList.Length < 3)
                return;

            FileInfo file = paramList[0] as FileInfo;
            string fileKey = paramList[1] as String;
            Action<string> onComplete = paramList[2] as Action<string>;

            MD5VersionObj obj = null;
            if (file != null && file.Exists)
            {
                obj = new MD5VersionObj();
                obj.fileKey = fileKey;
                obj.fileMD5 = ALCommon.generateMD5(file);
                obj.fileSize = file.Length;
            }
            // 回到主线程，保存信息到本地
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                // 如果文件不存在就加入null到缓存里，如果存在就生成md5相关信息加入到缓存中
                // 覆盖旧的数据
                _m_dLocalCache[fileKey] = obj;

                if(null != obj)
                    _saveLocalVersionObjToSourceData(fileKey, obj);

                //调用完成回调
                onComplete?.Invoke(obj == null ? string.Empty : obj.fileMD5);
            });
        }
        /// <summary>
        /// 删除本地文件记录的MD5值
        /// </summary>
        public void deleteLocalFileMD5(string _fileKey)
        {
            // 从本地缓存中删除
            _m_dLocalCache.Remove(_fileKey);
            // 从数据源中删除
            _deleteLocalVersionObjFromSourceData(_fileKey);
        }


        /// <summary>
        /// 尝试获取对应本地文件的md5
        /// </summary>
        public void tryGetStreamingAssetsFileMD5(string _fileKey, Action<string> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dStreamingAssetsCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getStreamingAssetsVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dStreamingAssetsCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? string.Empty : _md5Obj.fileMD5);
                });
            }
            else
            {
                _got(_obj == null ? string.Empty : _obj.fileMD5);
            }
        }
        /// <summary>
        /// 尝试获取对应本地文件的大小
        /// </summary>
        public void tryGetStreamingAssetsFileSizeByte(string _fileKey, Action<long> _got)
        {
            if (_got == null)
                return;

            // 先尝试从缓存中获取
            if(!_m_dStreamingAssetsCache.TryGetValue(_fileKey, out MD5VersionObj _obj))
            {
                // 如果取不到则从数据源中取，数据源不管取到取不到，都把结果加入缓存中
                _getStreamingAssetsVersionObjFromSourceData(_fileKey, _md5Obj =>
                {
                    _m_dStreamingAssetsCache[_fileKey] = _md5Obj;
                    _got(_md5Obj == null ? 0L : _md5Obj.fileSize);
                });
            }
            else
            {
                _got(_obj == null ? 0L : _obj.fileSize);
            }
        }



        /// <summary>
        /// 通过比对本地的MD5值和远端的MD5值，检查一个资源的版本状态
        /// </summary>
        public virtual void checkResVersion(string _fileKey, Action<VersionCheckResult> _checkResult) // 虚方法，需要特殊的判断机制，override覆盖掉即可
        {
            if (_checkResult == null)
                return;

            string remoteMD5 = string.Empty;
            string localMD5 = string.Empty;
            string streamingAssetsMD5 = string.Empty;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(3);

            // 尝试取出三个资源来源的对应md5值
            tryGetRemoteFileMD5(_fileKey, _md5 =>
            {
                remoteMD5 = _md5;
                stepCounter.addDoneStepCount();
            });
            tryGetLocalFileMD5(_fileKey, _md5 =>
            {
                localMD5 = _md5;
                stepCounter.addDoneStepCount();
            });
            tryGetStreamingAssetsFileMD5(_fileKey, _md5 =>
            {
                streamingAssetsMD5 = _md5;
                stepCounter.addDoneStepCount();
            });

            // MD5都取到了之后的比对方法
            stepCounter.regAllDoneDelegate(() =>
            {
                // 如果远端数据不存在
                if(string.IsNullOrEmpty(remoteMD5))
                {
                    // 并且本地数据也不存在
                    if(string.IsNullOrEmpty(localMD5))
                        _checkResult(VersionCheckResult.FileNotExist); // 那么其实查询了一个完全不存在的文件
                    else
                        _checkResult(VersionCheckResult.DeleteLocalFile);       // 远端不存在而本地存在的话，要把本地删除
                }
                // 如果远端数据存在，那么先在随包资源里找有没有符合版本的信息的资源，如果有，就使用随包资源
                else
                {
                    if(!string.IsNullOrEmpty(streamingAssetsMD5) && streamingAssetsMD5 == remoteMD5)
                        _checkResult(VersionCheckResult.UseStreamingAssetsFile);
                    // 如果随包资源中没有，那么就去看看本地的下载缓存有没有
                    else if(string.IsNullOrEmpty(localMD5) || localMD5 != remoteMD5)
                        _checkResult(VersionCheckResult.UpdateFromRemote);
                    else
                        _checkResult(VersionCheckResult.UseLocalFile); // 否则资源校验通过，使用本地资源
                }
            });
        }

        public enum VersionCheckResult
        {
            NONE,
            DeleteLocalFile,        // 远端文件已被删除
            UpdateFromRemote,       // 远端文件已更新
            UseLocalFile,           // 版本比对通过可以直接使用本地文件
            UseStreamingAssetsFile, // 版本比对通过可以直接使用随包文件
            FileNotExist,           // 文件压根就不存在
        }

        /// <summary>
        /// 从远程URL上下载对应的md5文件并从中获取对应的文件md5信息
        /// </summary>
        /// <param name="_fileKey"></param>
        /// <param name="_got"></param>
        protected abstract void _getRemoteVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got);
        /// <summary>
        /// 从本地文件直接加载对应存储md5信息的文件信息
        /// </summary>
        /// <param name="_fileKey"></param>
        /// <param name="_got"></param>
        protected abstract void _getLocalVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got);
        /// <summary>
        /// 从随包streamingAsset中加载对应相对文件路径的文件md5信息
        /// </summary>
        /// <param name="_fileKey"></param>
        /// <param name="_got"></param>
        protected abstract void _getStreamingAssetsVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got);
        // 对本地的数据源做存储和删除操作
        protected abstract void _saveLocalVersionObjToSourceData(string _fileKey, MD5VersionObj _obj);
        protected abstract void _deleteLocalVersionObjFromSourceData(string _fileKey);
        // 当该对象被销毁时
        protected abstract void _onDiscard();
    }
}