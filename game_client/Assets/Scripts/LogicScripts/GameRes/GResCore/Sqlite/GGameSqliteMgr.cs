using UnityEngine;
using System.IO;
using System;
using System.Text;
using SQLite4Unity3d;
using System.Collections.Generic;
using ALPackage;

/// <summary>
/// SQLite数据库管理器
/// </summary>
namespace GOE
{
    public class GGameSqliteMgr : _AALSqliteMgr
    {
        private static GGameSqliteMgr _g_instance = new GGameSqliteMgr();
        public static GGameSqliteMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGameSqliteMgr();
                return _g_instance;
            }
        }

        public GGameSqliteMgr()
        {
        }

        //获取对应数据源
        public override _AALResourceCore resCore { get { return RefdataResCore.instance; } }
    }
}

