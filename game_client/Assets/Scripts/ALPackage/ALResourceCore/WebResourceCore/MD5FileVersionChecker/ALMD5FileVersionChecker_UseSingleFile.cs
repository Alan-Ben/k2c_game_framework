
using System;
using System.IO;

namespace ALPackage
{
    // 适用于MD5是存放在目标文件相同目录，相同名字，只是后缀加了一个.md5的形式
    public class ALMD5FileVersionCheck_UseSingleFile : _AALMD5FileVersionChecker
    {
        // 远程资源的根目录
        private string _m_sRemoteRootURL;
        // 本地资源的根目录
        private string _m_sLocalRootURL;
        // 随包资源的根目录
        private string _m_sStreamingAssetsRootURL;

        public ALMD5FileVersionCheck_UseSingleFile(string _remoteRootURL, string _localRootURL, string _streamingAssetsRootURL)
        {
            _m_sRemoteRootURL = ALCommon.directoryInsure(_remoteRootURL);
            _m_sLocalRootURL = ALCommon.directoryInsure(_localRootURL);
            _m_sStreamingAssetsRootURL = ALCommon.directoryInsure(_streamingAssetsRootURL);
        }

        protected override void _onDiscard()
        {
        }

        // 用远程路径上直接把目标文件的md5文件下载下来
        protected override void _getRemoteVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
        {
            // 发送http请求，下载对应文件的md5值
            new ALHttpSingleDownloadDealer(_getRemoteMD5FileFullPath(_fileKey), _getLocalMD5TempFileFullePath(_fileKey),
                // 下载成功的处理（主线程）
                () =>
                {
                    FileInfo localTmpFile = new FileInfo(_getLocalMD5TempFileFullePath(_fileKey));
                    MD5VersionObj md5Obj = _getMd5FileData(_fileKey, localTmpFile
                        , () => 
                        {
                            try
                            {
                                if(localTmpFile.Exists)
                                    localTmpFile.Delete();
                            }
                            catch(Exception) { }
                        });
                    _got(md5Obj);
                },
                // 下载失败的处理
                (int _errStat) =>
                {
                    try
                    {
                        // 尝试删除本地的临时文件
                        if (File.Exists(_getLocalMD5TempFileFullePath(_fileKey)))
                            File.Delete(_getLocalMD5TempFileFullePath(_fileKey));
                    }
                    catch(Exception) { }
                    _got(null);
                }).startLoad();
        }

        // 从本地把生成好的md5文件取出来
        protected override void _getLocalVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
        {
            MD5VersionObj md5Obj = _getMd5FileData(_fileKey, _getLocalMD5FileFullPath(_fileKey), null);
            _got(md5Obj);
        }

        /// <summary>
        /// 从随包streamingAsset中加载对应相对文件路径的文件md5信息
        /// </summary>
        /// <param name="_fileKey"></param>
        /// <param name="_got"></param>
        protected override void _getStreamingAssetsVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
        {
            MD5VersionObj md5Obj = _getMd5FileData(_fileKey, _getStreamingAssetsMD5FileFullPath(_fileKey), null);
            _got(md5Obj);
        }

        /// <summary>
        /// 从本地把生成好的md5信息取出来
        /// </summary>
        /// <param name="_fileKey"></param>
        /// <param name="_got"></param>
        protected MD5VersionObj _getMd5FileData(string _fileKey, string _fullPath, Action _finalDelegate)
        {
            FileInfo localFile = new FileInfo(_fullPath);
            return _getMd5FileData(_fileKey, localFile, _finalDelegate);
        }
        protected MD5VersionObj _getMd5FileData(string _fileKey, FileInfo _fileInfo, Action _finalDelegate)
        {
            try
            {
                if(null == _fileInfo)
                    return null;
                
                if(_fileInfo.Exists)
                {
                    using(StreamReader reader = _fileInfo.OpenText())
                    {
                        if(reader == null) // 如果reader不存在，则删除本地临时文件返回失败
                        {
                            UnityEngine.Debug.LogError($"[getMd5FileData]{_fileInfo.FullName} FileInfo.OpenText取到的返回值为null，还有这种事？！？");
                            return null;
                        }

                        string content = reader.ReadToEnd();
                        string[] saveData = content.Split('|');
                        if(saveData.Length < 2)
                        {
                            UnityEngine.Debug.LogError($"[getMd5FileData]{_fileInfo.FullName} 下载下来的临时文件的数据内容不对，应该是\"md5值|文件大小\"的格式");
                            return null;
                        }

                        string fileMD5 = saveData[0];
                        if(!long.TryParse(saveData[1], out long fileSize))
                        {
                            UnityEngine.Debug.LogError($"[getMd5FileData]{_fileInfo.FullName} 下载下来的临时文件的数据内容不对，应该是\"md5值|文件大小\"的格式");
                            return null;
                        }

                        MD5VersionObj obj = new MD5VersionObj();
                        obj.fileKey = _fileKey;
                        obj.fileMD5 = fileMD5;
                        obj.fileSize = fileSize;

                        return obj;
                    }
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if(null != _finalDelegate)
                    _finalDelegate();
            }
        }

        // 保存新的MD5版本数据到本地
        protected override void _saveLocalVersionObjToSourceData(string _fileKey, MD5VersionObj _obj)
        {
            if (_obj == null)
                return;

            // 获取文件路径
            string savePath = _getLocalMD5FileFullPath(_fileKey);
            // 获取文件目录
            string saveDirectory = Path.GetDirectoryName(savePath);

            // 如果目录不存在则创建
            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            // 生成要保存的字符串
            string outputStr = _obj.fileMD5 + "|" + _obj.fileSize;

            // 删除旧的同名文件，并创建新的文件写入字符串
            if (File.Exists(savePath))
                File.Delete(savePath);
            File.WriteAllText(savePath, outputStr);
        }

        // 从本地的数据库中删除数据
        protected override void _deleteLocalVersionObjFromSourceData(string _fileKey)
        {
            // 获取文件路径
            string savePath = _getLocalMD5FileFullPath(_fileKey);
            // 删除对应文件
            if (File.Exists(savePath))
                File.Delete(savePath);
        }

        // 获取远端文件的md5文件完整路径
        private string _getRemoteMD5FileFullPath(string _fileKey)
        {
            string fullPath = _m_sRemoteRootURL + _fileKey + ".md5";
            //fullPath = Path.ChangeExtension(fullPath, ".md5"); 不采用，因为文件名可能会重复
            return fullPath;
        }

        // 获取远端下载后的临时文件的完整路径
        private string _getLocalMD5TempFileFullePath(string _fileKey)
        {
            string fullPath = _m_sLocalRootURL + _fileKey + ".md5Temp";
            //fullPath = Path.ChangeExtension(fullPath, ".md5Temp"); 不采用，因为文件名可能会重复
            return fullPath;
        }

        // 获取本地文件的md5文件完整路径
        private string _getLocalMD5FileFullPath(string _fileKey)
        {
            string fullPath = _m_sLocalRootURL + _fileKey + ".md5";
            //fullPath = Path.ChangeExtension(fullPath, ".md5");
            return fullPath;
        }

        // 获取随包文件的md5文件完整路径
        private string _getStreamingAssetsMD5FileFullPath(string _fileKey)
        {
            string fullPath = _m_sStreamingAssetsRootURL + _fileKey + ".md5";
            //fullPath = Path.ChangeExtension(fullPath, ".md5");
            return fullPath;
        }
    }
}