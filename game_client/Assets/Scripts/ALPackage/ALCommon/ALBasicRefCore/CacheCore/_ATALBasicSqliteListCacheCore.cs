using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace ALPackage
{
#if AL_SQLITE
    /*********************
     * 数据集合中心管理对象
     * 本类型对象使用Sqlite进行数据管理，每个id对应一个数据
     **/
    public abstract class _ATALBasicSqliteListCacheCore<T> : _ATALBasicCacheCore<List<T>>, _IALInitRefObj where T : new()
    {
        /** 回调处理函数 */
        private Action<Type, _IALInitRefObj> _m_dFailDelegate;

        protected _ATALBasicSqliteListCacheCore() 
        {
        }

        /// <summary>
        /// 子类初始化函数
        /// 如数据库可能需要连接等
        /// </summary>
        protected override void _init(Action _doneDelegate, System.Action<Type, _IALInitRefObj> _onFail)
        {
            //设置回调
            _m_dFailDelegate = _onFail;

            //初始化数据库连接，初始化完成调用成功处理
            _sqliteMgr.dealSqliteConn(_assetPath, _objName, _dealSuccessCallback);
        }

        protected void _dealSuccessCallback(ALSqliteInfo _sqliteInfo)
        {
            try
            {
                if (null != finalDelegate)
                    finalDelegate();
            }
            finally
            {
                _reset(); //try finally防止调用过程中有异常导致没置空
            }
        }

        protected void _dealFailCallback()
        {
            try
            {
                if (null != _m_dFailDelegate)
                    _m_dFailDelegate(typeof(T), this);
            }
            finally
            {
                _reset(); //try finally防止调用过程中有异常导致没置空
            }
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        protected new void _reset()
        {
            base._reset();

            _m_dFailDelegate = null;
        }

        /// <summary>
        /// 根据Id即时检索对应的数据对象，检索到的结果将会缓存起来进行处理
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_obj"></param>
        /// <returns></returns>
        protected override bool _searchData(long _id, out List<T> _obj)
        {
            //尝试进行数据库检索
            ALSqliteInfo sqliteInfo = _sqliteMgr.synGetSqliteConn(_assetPath, _objName);
            if (null == sqliteInfo)
            {
                UnityEngine.Debug.LogError($"Can not find Sqlite DB{_objName}");
                _obj = null;

                return false;
            }

            //从数据库中进行检索
            _obj = sqliteInfo.selectObj<T>(_tableName, $" {_keyName}={_id} ");

            return true;
        }

        /// <summary>
        /// 构造一个空对象，对检索失败的数据进行缓存处理
        /// </summary>
        /// <returns></returns>
        protected override List<T> _makeEmptyObj()
        {
            return new List<T>();
        }

        /** 获取资源加载的对象 */
        protected abstract _AALSqliteMgr _sqliteMgr { get; }
        /** 获取加载资源对象的路径 */
        protected abstract string _assetPath { get; }
        protected abstract string _objName { get; }

        /// <summary>
        /// 数据库查询需要的变量
        /// </summary>
        protected abstract string _tableName { get; }
        protected abstract string _keyName { get; }
    }
#endif
}
