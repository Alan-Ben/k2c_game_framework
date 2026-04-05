using UnityEngine;
using System.IO;
using System;
using System.Text;

#if AL_SQLITE
using SQLite4Unity3d;
#endif

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 单个SQLite数据库信息对象
/// </summary>
namespace ALPackage
{
#if AL_SQLITE
    public class ALSqliteInfo
    {
        //sqlite数据连接管理对象
        private _AALSqliteMgr _m_smSqliteMgr;
        /// <summary>
        /// 数据库路径
        /// </summary>
        protected string _m_sDBPath;
        protected string _m_sAssetbundlePath;

        private string _m_sRealObjName;
        /// <summary>
        /// 实际保存的路径
        /// </summary>
        private string _m_sRealSavePath;
        //实际数据库文件版本
        private long _m_lRealDBFileVersion;

        //是否开始初始化
        private bool _m_bIsStartInit;
        //是否初始化过
        private bool _m_bIsInited;
        /// <summary>
        /// 连接对象
        /// </summary>
        private SQLiteConnection _m_sqConnection;

        /// <summary>
        /// 初始化完成的回调
        /// </summary>
        private Action<ALSqliteInfo> _m_dInitCall;

        /// <summary>
        /// 将对应的数据库信息带入
        /// </summary>
        /// <param name="_dbPath"></param>
        public ALSqliteInfo(_AALSqliteMgr _sqliteMgr, string _dbPath, string _assetbundlePath)
        {
            _m_smSqliteMgr = _sqliteMgr;

            _m_sDBPath = _dbPath;
            _m_sAssetbundlePath = _assetbundlePath;

            _m_sRealSavePath = string.Empty;
            _m_lRealDBFileVersion = 0;

            _m_bIsStartInit = false;
            _m_bIsInited = false;
            _m_sqConnection = null;
            _m_dInitCall = default(Action<ALSqliteInfo>);
        }

        public bool isStartInit { get { return _m_bIsStartInit; } }
        public bool isInited { get { return _m_bIsInited; } }

        /// <summary>
        /// 初始化本数据库信息的连接对象
        /// </summary>
        public void init(Action<ALSqliteInfo> _doneDelegate)
        {
            //判断是否已经进行了初始化
            if (_m_bIsStartInit)
            {
                //判断是否已经初始化完成，如果完成则直接调用，如果未完成则注册回调
                if (_m_bIsInited)
                {
                    if (null != _doneDelegate)
                        _doneDelegate(this);
                }
                else
                {
                    //设置回调
                    if(null != _doneDelegate)
                        _m_dInitCall += _doneDelegate;
                    return;
                }
            }

            //设置已经初始化
            _m_bIsStartInit = true;
            //设置回调
            if (null != _doneDelegate)
                _m_dInitCall += _doneDelegate;

#if UNITY_EDITOR
            if (ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal)
            {
                //获取数据路径
                string realDBPath = string.Format(@"{0}/" + ALLocalResLoaderMgr.instance.localResRootPath + "/Refdata/__DLExport/{1}", Application.dataPath, _m_sDBPath);

                if (File.Exists(realDBPath))
                {
                    try
                    {
                        _m_sqConnection = new SQLiteConnection(realDBPath, SQLiteOpenFlags.ReadOnly);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[SQLite]{_m_sRealSavePath}连接创建失败, ：{e}");
                    }
                    Debug.Log("sqlite DB PATH: " + realDBPath);
                }
                else
                {
                    Debug.LogError("Can not load sqlite DB: " + realDBPath);
                }

                //设置初始化完成
                _m_bIsInited = true;
                Action<ALSqliteInfo> tmpCall = _m_dInitCall;
                _m_dInitCall = default(Action<ALSqliteInfo>);
                if (tmpCall != null)
                    tmpCall(this);
            }
            else
#endif
            {
                //开始加载并处理
                if (null != _m_smSqliteMgr.resCore)
                {
                    //此时需要重写文件，先进行版本判断
                    long curFileVersion = ALSqliteVersionMgr.instance.getSqliteFileVersion(_m_sDBPath);
                    //获取当前版本信息
                    ALAssetBundleVersionInfo vInfo = _m_smSqliteMgr.resCore.getVersionInfo(_m_sAssetbundlePath);
                    //设置版本
                    if (null == vInfo)
                        _m_lRealDBFileVersion = 0;
                    else
                        _m_lRealDBFileVersion = vInfo.versionNum;

                    //获取对象名称
                    _m_sRealObjName = _m_sDBPath;
                    int lastCPos = _m_sDBPath.LastIndexOf('/');
                    if (lastCPos > 0 && _m_sDBPath.Length > lastCPos)
                    {
                        //获取之后的文件名称
                        _m_sRealObjName = _m_sDBPath.Substring(lastCPos + 1);
                    }

                    string folderPath = _m_smSqliteMgr.resCore.getPatchAssetPath("_ALDB");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    //拼凑写入路径
                    _m_sRealSavePath = string.Format("{0}/{1}_{2}", folderPath, _m_sRealObjName, curFileVersion);

                    //判断是否需要覆盖文件
                    if (curFileVersion == _m_lRealDBFileVersion && File.Exists(_m_sRealSavePath))
                    {
                        //文件一致，直接读取本地已存在文件
                        try
                        {
                            _m_sqConnection = new SQLiteConnection(_m_sRealSavePath, SQLiteOpenFlags.ReadOnly);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"[SQLite]{_m_sRealSavePath}连接创建失败, ：{e}");
                        }
#if UNITY_EDITOR
                        Debug.Log("sqlite DB PATH: " + _m_sRealSavePath);
#endif

                        //设置初始化完成
                        _m_bIsInited = true;
                        Action<ALSqliteInfo> tmpCall = _m_dInitCall;
                        _m_dInitCall = default(Action<ALSqliteInfo>);
                        if (tmpCall != null)
                            tmpCall(this);
                    }
                    else
                    {
                        //开始加载
                        _m_smSqliteMgr.resCore.loadAsset(_m_sAssetbundlePath, _onResCoreDBABLoaded, null);
                    }
                }
                else
                {
                    Debug.LogError("Load Sqlite fail because rescore is null!");
                    //设置初始化完成
                    _m_bIsInited = true;
                    //不进行加载处理
                    Action<ALSqliteInfo> tmpCall = _m_dInitCall;
                    _m_dInitCall = default(Action<ALSqliteInfo>);
                    if (tmpCall != null)
                        tmpCall(this);
                }
            }
        }

        /// <summary>
        /// AB数据加载后的处理
        /// </summary>
        protected void _onResCoreDBABLoaded(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            try
            {
                //失败则直接返回
                if (!_isSuc || null == _assetObj)
                    return;

                //加载对象
                TextAsset dbInfo = _assetObj.assetBundle.LoadAsset<TextAsset>(_m_sRealObjName);

                //无效对象则报错
                if (null == dbInfo)
                {
                    UnityEngine.Debug.LogError($"Load DB : {_m_sDBPath}   obj : {_m_sRealObjName}  fail!");
                    return;
                }

                //将文件写入对应文件中，写入前先删除旧文件
                ALFile.Delete(_m_sRealSavePath);

                //设置新文件路径
                _m_sRealSavePath = string.Format("{0}/{1}_{2}", _m_smSqliteMgr.resCore.getPatchAssetPath("_ALDB"), _m_sRealObjName, _m_lRealDBFileVersion);
                File.WriteAllBytes(_m_sRealSavePath, dbInfo.bytes);
                _m_sqConnection = new SQLiteConnection(_m_sRealSavePath, SQLiteOpenFlags.ReadOnly);
#if UNITY_EDITOR
                Debug.Log("Init Sqlite DB: " + _m_sRealSavePath);
#endif

                //更新旧版本数据
                ALSqliteVersionMgr.instance.switchFileVersion(_m_sDBPath, _m_lRealDBFileVersion);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SQLite] _onResCoreDBABLoaded发生异常{_isSuc}, {_m_sDBPath}, {_m_sRealObjName}, {_m_sRealSavePath}, {e}");
            }
            finally
            {
                //设置初始化完成
                _m_bIsInited = true;
                //调用回调处理
                Action<ALSqliteInfo> tmpCall = _m_dInitCall;
                _m_dInitCall = default(Action<ALSqliteInfo>);
                if (tmpCall != null)
                    tmpCall(this);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void discard()
        {
            try
            {
                if (_m_sqConnection != null)
                    _m_sqConnection.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError($"[SQLite]{_m_sRealSavePath}释放失败, ：{e}");
            }
            _m_sqConnection = null;
            _m_bIsStartInit = false;
            _m_bIsInited = false;
            _m_dInitCall = default(Action<ALSqliteInfo>);

            //判断是否本地加载，如果不是则删除文件
#if UNITY_EDITOR
            if (ALLocalResLoaderMgr.instance.isLoadRefdataFromLocal)//只有不从本地加载资源，才要删掉这个东西
            {
                //本地加载则不做其他处理
            }
            else
#endif
            {
                ALFile.Delete(_m_sRealSavePath);
            }
        }

        /// <summary>
        /// 执行查询处理
        /// </summary>
        /// <param name="_query"></param>
        /// <returns></returns>
        public List<T> selectObj<T>(string _tableName, string _conditionStr) where T : new()
        {
            if (null == _m_sqConnection)
            {
                UnityEngine.Debug.LogError($"Connect is not inited! for table[{_tableName}]");

                return null;
            }

            try
            {
                string sql = string.Empty;
                if (string.IsNullOrEmpty(_conditionStr))
                {
                    sql = string.Format($"select * from \"{_tableName}\"");
                }
                else
                {
                    sql = string.Format($"select * from \"{_tableName}\" where {_conditionStr}");
                }

                //返回查询结果
                List<T> resultList = _m_sqConnection.Query<T>(sql);
                return resultList;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
                UnityEngine.Debug.LogError($"Select from table[{_tableName}] get some errors!");

                return null;
            }
        }
    }
#endif
}

