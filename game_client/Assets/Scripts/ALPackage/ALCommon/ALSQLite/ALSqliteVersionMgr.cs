using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Sqlite所有写入本地的相关文件信息版本管理器，避免每次都要重新解压
/// </summary>
namespace ALPackage
{
#if AL_SQLITE
    public class ALSqliteVersionMgr : _AALBasicSettingInfo
    {
        private static ALSqliteVersionMgr _g_instance = new ALSqliteVersionMgr();
        public static ALSqliteVersionMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALSqliteVersionMgr();

                return _g_instance;
            }
        }

        /// <summary>
        /// 保存的DB文件版本信息
        /// </summary>
        protected class ALSqliteSaveDBFileInfo
        {
            public string dbPath;
            public long version;

            public ALSqliteSaveDBFileInfo(string _dbPath, long _version)
            {
                dbPath = _dbPath;
                version = _version;
            }
        }

        private List<ALSqliteSaveDBFileInfo> _m_lSaveDBFileInfo;

        protected ALSqliteVersionMgr()
            : base("__ALSqliteVersion")
        {
            _m_lSaveDBFileInfo = new List<ALSqliteSaveDBFileInfo>();
        }

        //检索对应数据库的版本信息
        public long getSqliteFileVersion(string _dbPath)
        {
            //判断是否有初始化，如未初始化则处理初始化
            if (!isInited)
                init();

            ALSqliteSaveDBFileInfo tmpInfo = null;
            for (int i = 0; i < _m_lSaveDBFileInfo.Count; i++)
            {
                //比较并返回
                tmpInfo = _m_lSaveDBFileInfo[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.dbPath.Equals(_dbPath, StringComparison.OrdinalIgnoreCase))
                    return tmpInfo.version;
            }

            return 0;
        }

        /// <summary>
        /// 删除对应数据库文件的版本信息
        /// </summary>
        public void removeFileVersion(string _dbPath)
        {
            if (!isInited)
                init();

            ALSqliteSaveDBFileInfo tmpInfo = null;
            for (int i = 0; i < _m_lSaveDBFileInfo.Count; i++)
            {
                //比较并返回
                tmpInfo = _m_lSaveDBFileInfo[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.dbPath.Equals(_dbPath, StringComparison.OrdinalIgnoreCase))
                {
                    _m_lSaveDBFileInfo.RemoveAt(i);

                    //保存
                    saveSetting();
                    return;
                }
            }
        }

        /// <summary>
        /// 添加对应数据库文件的版本信息
        /// </summary>
        public void addFileVersion(string _dbPath, long _version)
        {
            if (!isInited)
                init();

            _m_lSaveDBFileInfo.Add(new ALSqliteSaveDBFileInfo(_dbPath, _version));
            saveSetting();
        }

        /// <summary>
        /// 修改对应数据的版本号
        /// </summary>
        public void switchFileVersion(string _dbPath, long _version)
        {
            if (!isInited)
                init();

            ALSqliteSaveDBFileInfo tmpInfo = null;
            for (int i = 0; i < _m_lSaveDBFileInfo.Count; i++)
            {
                //比较并返回
                tmpInfo = _m_lSaveDBFileInfo[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.dbPath.Equals(_dbPath, StringComparison.OrdinalIgnoreCase))
                {
                    tmpInfo.version = _version;

                    //保存
                    saveSetting();
                    return;
                }
            }

            //添加一个新值
            _m_lSaveDBFileInfo.Add(new ALSqliteSaveDBFileInfo(_dbPath, _version));
            saveSetting();
        }

        /// <summary>
        /// 根据字符串初始化内容
        /// </summary>
        /// <param name="_infoStr"></param>
        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            //逐个数据检查
            ALStringReader reader = new ALStringReader(_infoStr);
            string curInfo = reader.readItem('|');

            while(!string.IsNullOrEmpty(curInfo))
            {
                string[] infoStrs = curInfo.Split('#');

                //放入数据集
                try
                {
                    _m_lSaveDBFileInfo.Add(new ALSqliteSaveDBFileInfo(infoStrs[0], long.Parse(infoStrs[1])));
                }
                catch (Exception _e)
                {
                    UnityEngine.Debug.LogError(_e);
                }

                //获取下一个
                curInfo = reader.readItem('|');
            }
        }

        //根据当前内容构建保存数据
        protected override string _makeSettingStr()
        {
            StringBuilder strBuild = new StringBuilder();

            for(int i = 0; i < _m_lSaveDBFileInfo.Count; i++)
            {
                //非第一个需要增加间隔符
                if (i > 0)
                    strBuild.Append('|');

                strBuild.Append(_m_lSaveDBFileInfo[i].dbPath)
                    .Append('#')
                    .Append(_m_lSaveDBFileInfo[i].version);
            }

            return strBuild.ToString();
        }
    }
#endif
}

