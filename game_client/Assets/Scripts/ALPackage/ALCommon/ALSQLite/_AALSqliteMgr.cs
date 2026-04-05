using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// SQLite数据库管理器
/// </summary>
namespace ALPackage
{
#if AL_SQLITE
    public abstract class _AALSqliteMgr
    {
        protected Dictionary<string, ALSqliteInfo> _m_dicSqliteInfoDic;

        public _AALSqliteMgr()
        {
            _m_dicSqliteInfoDic = new Dictionary<string, ALSqliteInfo>();
        }

        //获取对应数据源
        public abstract _AALResourceCore resCore { get; }

        /// <summary>
        /// 初始化对应的数据库对象
        /// </summary>
        public void initSqliteConn(string _assetPath, string _objName)
        {
            ALSqliteInfo info = null;
            if(_m_dicSqliteInfoDic.TryGetValue(_objName, out info))
            {
                //直接处理回调，调用init里面会根据是否初始化完成在不同位置调用回调
                info.init(null);
                return;
            }

            //尝试创建对象并初始化处理
            info = new ALSqliteInfo(this, _objName, _assetPath);
            //放入数据集
            _m_dicSqliteInfoDic.Add(_objName, info);

            //开始初始化
            info.init(null);
        }

        /// <summary>
        /// 获取对应数据库连接处理对象的处理函数
        /// 实际的处理在回调中进行处理
        /// </summary>
        /// <param name="_objName"></param>
        /// <param name="_assetPath"></param>
        /// <param name="_dealAction"></param>
        public void dealSqliteConn(string _assetPath, string _objName, Action<ALSqliteInfo> _dealAction)
        {
            if (null == _dealAction)
                return;

            ALSqliteInfo info = null;
            if(_m_dicSqliteInfoDic.TryGetValue(_objName, out info))
            {
                //直接处理回调，调用init里面会根据是否初始化完成在不同位置调用回调
                info.init(_dealAction);
                return;
            }

            //尝试创建对象并初始化处理
            info = new ALSqliteInfo(this, _objName, _assetPath);
            //放入数据集
            _m_dicSqliteInfoDic.Add(_objName, info);

            //开始初始化
            info.init(_dealAction);
        }

        /// <summary>
        /// 同步获取对应数据库连接处理对象的处理函数
        /// 实际的处理在回调中进行处理
        /// 此时如果数据库未成功初始化则直接返回错误
        /// </summary>
        /// <param name="_objName"></param>
        /// <param name="_assetPath"></param>
        /// <param name="_dealAction"></param>
        public ALSqliteInfo synGetSqliteConn(string _assetPath, string _objName)
        {
            ALSqliteInfo info = null;
            if (!_m_dicSqliteInfoDic.TryGetValue(_objName, out info))
            {
                return null;
            }

            if(!info.isInited)
            {
                return null;
            }

            return info;
        }

        /// <summary>
        /// 必须要调用，不然数据库连接不会关闭
        /// </summary>
        public void discard()
        {
            foreach (ALSqliteInfo info in _m_dicSqliteInfoDic.Values)
            {
                if (info != null)
                    info.discard();
            }
        }
    }
#endif
}

