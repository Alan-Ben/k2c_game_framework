using UnityEngine;
using System.Collections;
using System;


namespace ALPackage
{
#if AL_SQLITE
    public class ALBasicSqliteSetRefCore<T> : _ATALBasicSqliteSetCore<T> where T : new()
    {
        private _AALSqliteMgr _m_rcSqliteMgr;
        private string _m_sAssetPath;
        private string _m_sObjName;

        private string _m_sTablePath;
        private string _m_sKeyName;

        public ALBasicSqliteSetRefCore(_AALSqliteMgr _sqliteMgr, string _assetPath, string _objName, string _tableName, string _keyName)
        {
            _m_rcSqliteMgr = _sqliteMgr;
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;

            _m_sTablePath = _tableName;
            _m_sKeyName = _keyName;
        }

        /** 获取资源加载的对象 */
        protected override _AALSqliteMgr _sqliteMgr { get { return _m_rcSqliteMgr; } }
        /** 获取加载资源对象的路径 */
        protected override string _assetPath { get { return _m_sAssetPath; } }
        protected override string _objName { get { return _m_sObjName; } }

        /// <summary>
        /// 数据库查询需要的变量
        /// </summary>
        protected override string _tableName { get { return _m_sTablePath; } }
        protected override string _keyName { get { return _m_sKeyName; } }
    }
#endif
}

