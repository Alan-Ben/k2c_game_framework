using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ALPackage;
using SQLite4Unity3d;
using UnityEngine.Networking;

public class LocalFileVersionAutoSaveMonoTask : _IALBaseMonoTask
{
    private MD5FileVersionChecker_UseFile _m_piPatchInfo;

    public LocalFileVersionAutoSaveMonoTask(MD5FileVersionChecker_UseFile _patchInfo)
    {
        _m_piPatchInfo = _patchInfo;
    }

    /**************
     * 函数主体
     **/
    public void deal()
    {
        if (null == _m_piPatchInfo)
            return;

        _m_piPatchInfo.savePatchInfo();
    }
}
public class MD5FileVersionChecker_UseFile : _IALMD5FileVersionChecker
{
    private string _m_sRemoteVersionPath;
    private string _m_sLocalVersionPath;
    private string _m_sStreamingAssetsVersionPath;
    
    private Dictionary<string, MD5VersionObj> _m_dicRemoteAssetsInfoDic;
    private Dictionary<string, MD5VersionObj> _m_dicStreamingAssetsAssetsInfoDic;
    
    private Dictionary<string, MD5VersionObj> _m_dicLocalAssetsInfoDic;
    
    private List<MD5VersionObj> _m_lSaveVersionInfoList;
    /** 是否在保存 */
    private bool _m_bIsSaving;
    /** 数据是否有变更 */
    private bool _m_bIsDataChg;


    
    
    // 使用数据库文件的路径构建数据库
    public MD5FileVersionChecker_UseFile(string _remoteVersionPath, string _localVersionPath, string _streamingAssetsVersionPath, string _streamingLocalPath)
    {
        _m_sRemoteVersionPath = _remoteVersionPath;
        _m_sLocalVersionPath = _localVersionPath;
        _m_sStreamingAssetsVersionPath = _streamingAssetsVersionPath;
        
        _m_dicRemoteAssetsInfoDic = new Dictionary<string, MD5VersionObj>();
        _m_dicStreamingAssetsAssetsInfoDic = new Dictionary<string, MD5VersionObj>();
        _m_dicLocalAssetsInfoDic = new Dictionary<string, MD5VersionObj>();
        _m_lSaveVersionInfoList = new List<MD5VersionObj>();

        _m_bIsSaving = false;
        _m_bIsDataChg = false;
    }
    
    public void init(Action _onLoadDone)
    {
        _updateVersionInfoToDic(_m_dicRemoteAssetsInfoDic, _m_sRemoteVersionPath, null);
        _updateVersionInfoToDic(_m_dicLocalAssetsInfoDic, _m_sLocalVersionPath, null);

#if UNITY_ANDROID
        ALCoroutineDealerMgr.instance.addCoroutine(new ALCoroutineWrapper(initStreamingAssetVersion(_m_sStreamingAssetsVersionPath, _onLoadDone)));
#else
        _m_dicStreamingAssetsAssetsInfoDic = new Dictionary<string, MD5VersionObj>();
        _updateVersionInfoToDic(_m_dicStreamingAssetsAssetsInfoDic, _m_sStreamingAssetsVersionPath, null);
        _onLoadDone?.Invoke();
#endif
    }
    public List<MD5VersionObj> getRemoteVersions()
    {
        return _m_dicRemoteAssetsInfoDic?.Values.ToList();
    }
    
    public List<MD5VersionObj> getLocalVersions()
    {
        return _m_dicLocalAssetsInfoDic?.Values.ToList();
    }
    
    
    /// <summary>
    /// 从本地把生成好的md5信息取出来
    /// </summary>
    /// <param name="_fullPath"></param>
    /// <param name="_finalDelegate"></param>
    protected void _updateVersionInfoToDic(Dictionary<string, MD5VersionObj> _dic, string _fullPath, Action _finalDelegate)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(_fullPath);
            
            if(_dic != null && fileInfo.Exists)
            {
                using(StreamReader reader = fileInfo.OpenText())
                {
                    string perLineStr;

                    while (true)
                    {
                        perLineStr = reader.ReadLine();
                        //如读取出内容为null表示到文件结尾了
                        if (null == perLineStr)
                            break;
                        string[] saveData = perLineStr.Split('|');
                        if (saveData.Length < 3)
                        {
                            UnityEngine.Debug.LogError(
                                $"[getMd5FileData]{fileInfo.FullName} version数据格式不对，应该是\"filename|md5值|文件大小\"的格式");
                            continue;
                        }
                        string fileKey = saveData[0];

                        string fileMD5 = saveData[1];
                        if (!long.TryParse(saveData[2], out long fileSize))
                        {
                            UnityEngine.Debug.LogError(
                                $"[getMd5FileData]{fileInfo.FullName} 下载下来的临时文件的数据内容不对，应该是\"filename|md5值|文件大小\"的格式");
                            continue;
                        }

                    
                        if (!string.IsNullOrEmpty(fileKey))
                        {
                            if (!_dic.ContainsKey(fileKey))
                            {
                                MD5VersionObj obj = new MD5VersionObj();
                                obj.fileKey = fileKey;
                                obj.fileMD5 = fileMD5;
                                obj.fileSize = fileSize;
                                _dic[fileKey] = obj;
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            if(null != _finalDelegate)
                _finalDelegate();
        }
    }
    
    IEnumerator initStreamingAssetVersion(string _streamingAssetsDBPath,  Action _onLoadDone)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(_streamingAssetsDBPath))
        {
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseFile] 连接到随包版本信息数据库失败，路径是：{_streamingAssetsDBPath}\n");
                }
                _onLoadDone?.Invoke();
                yield break;
            }
            string textFileContent = req.downloadHandler.text;
            // 按行分割文本内容
            string[] lines = textFileContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                string[] saveData = line.Split('|');
                if (saveData.Length < 3)
                {
                    UnityEngine.Debug.LogError(
                        $"[getMd5FileData]{_streamingAssetsDBPath} version数据格式不对，应该是\"filename|md5值|文件大小\"的格式");
                    continue;
                }
                string fileKey = saveData[0];

                string fileMD5 = saveData[1];
                if (!long.TryParse(saveData[2], out long fileSize))
                {
                    UnityEngine.Debug.LogError(
                        $"[getMd5FileData]{_streamingAssetsDBPath} 下载下来的临时文件的数据内容不对，应该是\"filename|md5值|文件大小\"的格式");
                    continue;
                }
                
                if (_m_dicStreamingAssetsAssetsInfoDic != null && !string.IsNullOrEmpty(fileKey))
                {
                    if (!_m_dicStreamingAssetsAssetsInfoDic.ContainsKey(fileKey))
                    {
                        MD5VersionObj obj = new MD5VersionObj();
                        obj.fileKey = fileKey;
                        obj.fileMD5 = fileMD5;
                        obj.fileSize = fileSize;
                        _m_dicStreamingAssetsAssetsInfoDic[fileKey] = obj;
                    }
                }
            }
            _onLoadDone?.Invoke();
        }
    }
    
    public void discard()
    {
        _m_dicRemoteAssetsInfoDic?.Clear();
        _m_dicLocalAssetsInfoDic?.Clear();
        _m_dicStreamingAssetsAssetsInfoDic?.Clear();
        _m_dicRemoteAssetsInfoDic = null;
        _m_dicLocalAssetsInfoDic = null;
        _m_dicStreamingAssetsAssetsInfoDic = null;
    }


    
    /// <summary>
    /// 尝试获取对应远程文件的md5
    /// </summary>
    public void tryGetRemoteFileMD5(string _fileKey, Action<string> _got)
    {
        if (_got == null)
            return;

        // 先尝试从缓存中获取
        if(!_m_dicRemoteAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
        {
            _got(string.Empty);
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
        if(!_m_dicRemoteAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
        {
            _got(0L);
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
        if(!_m_dicLocalAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
            _got(string.Empty);
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
        if(!_m_dicLocalAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
        {
            _got(0L);
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
            _m_dicLocalAssetsInfoDic[fileKey] = obj;

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
        _m_dicLocalAssetsInfoDic.Remove(_fileKey);
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
        if(!_m_dicStreamingAssetsAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
        {
            _got(string.Empty);
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
        if(!_m_dicStreamingAssetsAssetsInfoDic.TryGetValue(_fileKey, out MD5VersionObj _obj))
        {
            _got(0L);
        }
        else
        {
            _got(_obj == null ? 0L : _obj.fileSize);
        }
    }



    /// <summary>
    /// 通过比对本地的MD5值和远端的MD5值，检查一个资源的版本状态
    /// </summary>
    public virtual void checkResVersion(string _fileKey, Action<_IALMD5FileVersionChecker.VersionCheckResult> _checkResult) // 虚方法，需要特殊的判断机制，override覆盖掉即可
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
                // 优先用本地数据，如果本地没有则用StreamingAssets数据
                if(!string.IsNullOrEmpty(localMD5))
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.UseLocalFile); // 否则资源校验通过，使用本地资源
                else if(!string.IsNullOrEmpty(streamingAssetsMD5))
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.UseStreamingAssetsFile);
                else
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.FileNotExist); // 那么其实查询了一个完全不存在的文件
            }
            // 如果远端数据存在，那么先在随包资源里找有没有符合版本的信息的资源，如果有，就使用随包资源
            else
            {
                if(!string.IsNullOrEmpty(streamingAssetsMD5) && streamingAssetsMD5 == remoteMD5)
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.UseStreamingAssetsFile);
                // 如果随包资源中没有，那么就去看看本地的下载缓存有没有
                else if(string.IsNullOrEmpty(localMD5) || localMD5 != remoteMD5)
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.UpdateFromRemote);
                else
                    _checkResult(_IALMD5FileVersionChecker.VersionCheckResult.UseLocalFile); // 否则资源校验通过，使用本地资源
            }
        });
    }

    /// <summary>
    /// 是否需要删除本地文件
    /// </summary>
    /// <param name="_fileKey"></param>
    /// <returns></returns>
    public bool needDeleteLocalVersionObjFile(MD5VersionObj _versionObj)
    {
        // 如果远端有这个文件并且版本一致则不需要删除，否则需要删除
        if (_m_dicRemoteAssetsInfoDic.TryGetValue(_versionObj.fileKey, out MD5VersionObj _obj))
        {
            if (_obj != null && _obj.fileMD5 == _versionObj.fileMD5)
                return false;
            else
                return true;
        }
        else
            return true;
    }
    
    // 保存新的MD5版本数据到本地的数据库中
    protected void _saveLocalVersionObjToSourceData(string _fileKey, MD5VersionObj _obj)
    {
        _setDataChg();
    }

    // 从本地的数据库中删除数据
    protected void _deleteLocalVersionObjFromSourceData(string _fileKey)
    {
        _setDataChg();
    }

    #region 保存
    
    protected internal void _setDataChg()
    {
        //如果已经是变动状态则不处理
        if (_m_bIsDataChg)
            return;

        //否则修改状态，并延迟保存版本号信息
        _m_bIsDataChg = true;
        //延迟10秒开启任务
        ALMonoTaskMgr.instance.addMonoTask(new LocalFileVersionAutoSaveMonoTask(this), 5);
    }
    /********************
     * 保存本信息对象
     **/
    public void savePatchInfo()
    {
        //判断是否正在保存，是则延迟到下次开启任务
        if(_m_bIsSaving)
        {
            //延迟10秒开启任务
            ALMonoTaskMgr.instance.addMonoTask(new LocalFileVersionAutoSaveMonoTask(this), 5);
            //退出函数
            return;
        }

        //将所有对象取出，放入队列，并带入保存线程
        lock(_m_lSaveVersionInfoList)
        {
            //清除原先数据集
            _m_lSaveVersionInfoList.Clear();

            lock (_m_dicLocalAssetsInfoDic)
            {
                //加入新数据集
                _m_lSaveVersionInfoList.AddRange(_m_dicLocalAssetsInfoDic.Values);
            }

            //设置保存状态
            _m_bIsDataChg = false;
            _m_bIsSaving = true;

            //开启线程进行处理
            ALThread saveThread = new ALThread(_savePatchThread);
            saveThread.startThread();
        }
    }

    protected void _savePatchThread()
    {
        /** 写入对象 */
        StreamWriter writer;

        string tempLocalVersionPath = _m_sLocalVersionPath + ".tmp";
        //判断文件是否存在，是则将文件转移到临时位置
        if(File.Exists(_m_sLocalVersionPath))
        {
            //删除临时位置文件
            if(File.Exists(tempLocalVersionPath))
                File.Delete(tempLocalVersionPath);
            //拷贝到临时位置
            File.Copy(_m_sLocalVersionPath, tempLocalVersionPath);
            //删除原位置文件
            ALFile.Delete(_m_sLocalVersionPath);
        }

        //创建新文件
        writer = File.CreateText(_m_sLocalVersionPath);

        lock (_m_lSaveVersionInfoList)
        {
            //逐个写入版本信息
            for (int i = 0; i < _m_lSaveVersionInfoList.Count; i++)
            {
                MD5VersionObj versionInfo = _m_lSaveVersionInfoList[i];
                if (null == versionInfo)
                    continue;
                string fileInfo = $"{versionInfo.fileKey}|{versionInfo.fileMD5}|{versionInfo.fileSize}";

                //写入信息
                writer.WriteLine(fileInfo);
            }
        }

        //关闭文件流
        writer.Close();
        writer.Dispose();

        //删除临时位置文件
        if(File.Exists(tempLocalVersionPath))
            File.Delete(tempLocalVersionPath);

        //设置线程状态
        _m_bIsSaving = false;
    }

    #endregion
}