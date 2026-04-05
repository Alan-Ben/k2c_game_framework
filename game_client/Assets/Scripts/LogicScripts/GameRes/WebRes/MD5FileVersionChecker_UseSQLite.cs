using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using SQLite4Unity3d;
using UnityEngine.Networking;

public class MD5FileVersionChecker_UseSQLite : _AALMD5FileVersionChecker
{
    // 远端的版本信息数据库连接
    private SQLiteConnection _m_cRemoteSQLiteConnection;
    // 当前本地资源的版本信息数据库连接
    private SQLiteConnection _m_cLocalSQLiteConnection;
    // 随包资源的版本信息数据库连接
    private SQLiteConnection _m_cStreamingAssetsSQLiteConnection;
    
    private string _m_sStreamingAssetsDBPath;
    private string _m_sStreamingAssetsLocalDBPath;
    

    // 使用数据库文件的路径构建数据库
    public MD5FileVersionChecker_UseSQLite(string _remoteDBPath, string _localDBPath, string _streamingAssetsDBPath, string _streamingLocalPath)
    {
        try
        {
            _m_cRemoteSQLiteConnection = new SQLiteConnection(_remoteDBPath, SQLiteOpenFlags.ReadOnly);
        }
        catch(Exception _ex)
        {
            _m_cRemoteSQLiteConnection = null;
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到远端版本信息数据库失败，路径是：{_remoteDBPath}\n{_ex.ToString()}");
        }

        try
        {
            _m_cLocalSQLiteConnection = new SQLiteConnection(_localDBPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _m_cLocalSQLiteConnection.CreateTable<SQLiteObj>(); // 内部封装了如果存在就不处理
        }
        catch(Exception _ex)
        {
            _m_cLocalSQLiteConnection = null;
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到本地版本信息数据库失败，路径是：{_localDBPath}\n{_ex.ToString()}");
        }

        try
        {
            _m_sStreamingAssetsDBPath = _streamingAssetsDBPath;
            _m_sStreamingAssetsLocalDBPath = _streamingLocalPath;
        }
        catch (Exception _ex)
        {
            _m_cStreamingAssetsSQLiteConnection = null;
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到随包版本信息数据库失败，路径是：{_streamingAssetsDBPath}\n{_ex.ToString()}");
            }
        }
    }

    public void loadStreamingDB(Action _onLoadDone)
    {
#if UNITY_ANDROID
        ALCoroutineDealerMgr.instance.addCoroutine(new ALCoroutineWrapper(prepareStreamingAssetDb(_m_sStreamingAssetsDBPath, _m_sStreamingAssetsLocalDBPath, _onLoadDone)));
#else
        try
        {
            _m_cStreamingAssetsSQLiteConnection = new SQLiteConnection(_m_sStreamingAssetsDBPath, SQLiteOpenFlags.ReadOnly);
            _onLoadDone?.Invoke();
        }
        catch(Exception _ex)
        {
            _m_cStreamingAssetsSQLiteConnection = null;
            _onLoadDone?.Invoke();
            if(_AALMonoMain.instance.showDebugOutput)
                UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到StreamingAssets版本信息数据库失败，路径是：{_m_sStreamingAssetsDBPath}\n{_ex.ToString()}");
        }
  
#endif
    }

    IEnumerator prepareStreamingAssetDb(string _streamingAssetsDBPath, string _streamingAssetLocalPath, Action _onLoadDone)
    {
        if (File.Exists(_streamingAssetLocalPath))
            File.Delete(_streamingAssetLocalPath);

        using (UnityWebRequest req = UnityWebRequest.Get(_streamingAssetsDBPath))
        {
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                _m_cStreamingAssetsSQLiteConnection = null;
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到随包版本信息数据库失败，路径是：{_streamingAssetsDBPath}\n");
                }
                _onLoadDone?.Invoke();
                yield break;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(_streamingAssetLocalPath));
            File.WriteAllBytes(_streamingAssetLocalPath, req.downloadHandler.data);
        }
        
        try
        {
            _m_cStreamingAssetsSQLiteConnection = new SQLiteConnection(_streamingAssetLocalPath, SQLiteOpenFlags.ReadOnly);
            _onLoadDone?.Invoke();
        }
        catch (Exception _ex)
        {
            _m_cStreamingAssetsSQLiteConnection = null;
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 连接到随包版本信息拷贝到本地的数据库失败，路径是：{_streamingAssetLocalPath}\n{_ex.ToString()}");
            }
            _onLoadDone?.Invoke();
        }
    }
    protected override void _onDiscard()
    {
        _m_cRemoteSQLiteConnection?.Dispose();
        _m_cLocalSQLiteConnection?.Dispose();
        _m_cStreamingAssetsSQLiteConnection?.Dispose();
        _m_cRemoteSQLiteConnection = null;
        _m_cLocalSQLiteConnection = null;
        _m_cStreamingAssetsSQLiteConnection = null;
    }

    public void getRemoteAllVersionObj(Action<List<MD5VersionObj>> _doGetAllObj)
    {
        if (_doGetAllObj == null)
            return;

        if (_m_cRemoteSQLiteConnection == null)
        {
            _doGetAllObj(null);
            return;
        }
        List<MD5VersionObj> versionObjs = new List<MD5VersionObj>();

        try
        {
            string sql = "select * from \"SQLiteObj\"";
            List<SQLiteObj> resultList = _m_cRemoteSQLiteConnection.Query<SQLiteObj>(sql);
            if (resultList != null)
            {
                foreach (SQLiteObj obj in resultList)
                {
                    versionObjs.Add(obj);
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 尝试从数据库中读取全部数据对象\"失败\n{ex.ToString()}");
        }
        _doGetAllObj(versionObjs);
    }

    public void getLocalAllVersionObj(Action<List<MD5VersionObj>> _doGetAllObj)
    {
        if (_doGetAllObj == null)
            return;
        if (_m_cLocalSQLiteConnection == null)
        {
            _doGetAllObj(null);
            return;
        }
        List<MD5VersionObj> versionObjs = new List<MD5VersionObj>();
        try
        {
            string sql = "select * from \"SQLiteObj\"";
            List<SQLiteObj> resultList = _m_cLocalSQLiteConnection.Query<SQLiteObj>(sql);
            if (resultList != null)
            {
                foreach (SQLiteObj obj in resultList)
                {
                    versionObjs.Add(obj);
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 尝试从数据库中读取全部数据对象\"失败\n{ex.ToString()}");
        }
        _doGetAllObj(versionObjs);
    }

    // 从远端的数据库文件中获取对应的MD5数据对象
    protected override void _getRemoteVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
    {
        if (_got == null)
            return;

        if (_m_cRemoteSQLiteConnection == null)
        {
            _got(null);
            return;
        }

        MD5VersionObj obj = null;
        try
        {
            string sql = string.Format("select * from \"SQLiteObj\" where fileKey = \'{0}\'", _fileKey.Trim());
            List<SQLiteObj> resultList = _m_cRemoteSQLiteConnection.Query<SQLiteObj>(sql);
            if (resultList != null && resultList.Count > 0)
            {
                obj = resultList[0];
            }
            else
            {
                obj = null;
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 尝试从数据库中读取key：\"{_fileKey.Trim()}\"失败\n{ex.ToString()}");
        }

        _got(obj);
    }

    // 从本地的数据库文件中获取对应的MD5数据对象
    protected override void _getLocalVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
    {
        if (_got == null)
            return;

        if (_m_cLocalSQLiteConnection == null)
        {
            _got(null);
            return;
        }

        MD5VersionObj obj = null;
        try
        {
            string sql = string.Format("select * from \"SQLiteObj\" where fileKey = \'{0}\'", _fileKey.Trim());
            List<SQLiteObj> resultList = _m_cLocalSQLiteConnection.Query<SQLiteObj>(sql);
            if (resultList != null && resultList.Count > 0)
            {
                // 如果取到了，加入到缓存中
                obj = resultList[0];
            }
            else
            {
                obj = null;
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 尝试从数据库中读取key：\"{_fileKey.Trim()}\"失败\n{ex.ToString()}");
        }

        _got(obj);
    }

    // 保存新的MD5版本数据到本地的数据库中
    protected override void _saveLocalVersionObjToSourceData(string _fileKey, MD5VersionObj _obj)
    {
        SQLiteObj sqliteObj = _obj as SQLiteObj;
        if (sqliteObj == null)
        {
            if (_obj == null)
                return;

            sqliteObj = new SQLiteObj();
            sqliteObj.fileKey = _obj.fileKey;
            sqliteObj.fileMD5 = _obj.fileMD5;
            sqliteObj.fileSize = _obj.fileSize;
        }

        // 把数据加入到数据库中，下一次就直接查表而不用直接生成md5（不知道会不会有直接生成md5比查表还快的情况）
        _m_cLocalSQLiteConnection.InsertOrReplace(sqliteObj);
        // 可以测试一下需不需要这一句
        _m_cLocalSQLiteConnection.Commit();
    }

    // 从本地的数据库中删除数据
    protected override void _deleteLocalVersionObjFromSourceData(string _fileKey)
    {
        _m_cLocalSQLiteConnection.Delete<SQLiteObj>(_fileKey);
        // 可以测试一下需不需要这一句
        _m_cLocalSQLiteConnection.Commit();
    }

    protected override void _getStreamingAssetsVersionObjFromSourceData(string _fileKey, Action<MD5VersionObj> _got)
    {
        if (_got == null)
            return;

        if (_m_cStreamingAssetsSQLiteConnection == null)
        {
            _got(null);
            return;
        }

        MD5VersionObj obj = null;
        try
        {
            string sql = string.Format("select * from \"SQLiteObj\" where fileKey = \'{0}\'", _fileKey.Trim());
            List<SQLiteObj> resultList = _m_cStreamingAssetsSQLiteConnection.Query<SQLiteObj>(sql);
            if (resultList != null && resultList.Count > 0)
            {
                // 如果取到了，加入到缓存中
                obj = resultList[0];
            }
            else
            {
                obj = null;
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[MD5FileVersionChecker_UseSQLite] 尝试从数据库中读取key：\"{_fileKey.Trim()}\"失败\n{ex.ToString()}");
        }

        _got(obj);
    }

    /// <summary>
    /// 内部使用的数据库对象
    /// </summary>
    [TableAttribute("SQLiteObj")]
    private class SQLiteObj : MD5VersionObj
    {
        [PrimaryKey]
        public override string fileKey { get { return _m_sFileKey; } set { _m_sFileKey = value; } }
    }
}