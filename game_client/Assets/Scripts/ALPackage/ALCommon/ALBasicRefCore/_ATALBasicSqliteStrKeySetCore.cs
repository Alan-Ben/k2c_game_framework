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
     * id 是string类型
     **/
    public abstract class _ATALBasicSqliteStrKeySetCore<T> : _IALInitRefObj where T : new()
    {
        /** 初始化状态位 */
        private bool _m_bInit;
        /** 回调处理函数 */
        private Action _m_dDelegate;
        private Action<Type, _IALInitRefObj> _m_dFailDelegate;

        /** 存储对应id索引的数据映射表,dictionary不可以用模板，会导致ios报错 */
        private Dictionary<string, object> _m_dicRef;

        /** 取值失败时返回的默认值 */
        private T _m_oDefaultObj;

        protected _ATALBasicSqliteStrKeySetCore() 
        {
            _m_bInit = false;

            _m_dicRef = new Dictionary<string, object>();

            _m_oDefaultObj = default(T);
        }

        public bool isInit { get { return _m_bInit; } }
        public Action finalDelegate { get { return _m_dDelegate; } }

        /// <summary>
        /// 设置默认值对象
        /// </summary>
        /// <param name="_v"></param>
        public void setDefaultValue(T _v)
        {
            _m_oDefaultObj = _v;
        }

        /***********
         * 初始化操作，开始下载并加载对应信息
         **/
        public void init(Action _doneDelegate, System.Action<Type, _IALInitRefObj> _onFail)
        {
            if (_m_bInit)
            {
                if (null != _doneDelegate)
                    _doneDelegate();
                _reset();
                return;
            }

            //设置回调
            _m_dDelegate = _doneDelegate;
            _m_dFailDelegate = _onFail;

            _m_bInit = true;
            //初始化数据库连接，初始化完成调用成功处理
            _sqliteMgr.dealSqliteConn(_assetPath, _objName, _dealSuccessCallback);
        }

        /// <summary>
        /// 根据id获取对应的数据
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_checkFail"></param>
        /// <returns></returns>
        public T getRef(string _id, bool _checkFail = true)
        {
            //判断是否有缓存数据，如有则直接返回
            object tmpObj = null;
            if (_m_dicRef.TryGetValue(_id, out tmpObj))
            {
                return (T)tmpObj;
            }

            //尝试进行数据库检索
            ALSqliteInfo sqliteInfo = _sqliteMgr.synGetSqliteConn(_assetPath, _objName);
            if(null == sqliteInfo)
            {
                UnityEngine.Debug.LogError($"Can not find Sqlite DB {_assetPath} - {_objName}");
                return default(T);
            }

            //从数据库中进行检索
            List<T> list = sqliteInfo.selectObj<T>(_tableName, $" {_keyName}='{_id}' ");

            if (_checkFail && (null == list || list.Count <= 0))
            {
                _getRefFailed(_id);
                return _m_oDefaultObj;
            }

            //判断是否有多个数据，有则报错
            if(null != list && list.Count > 1)
            {
                UnityEngine.Debug.LogError($"Find multi Id for Sqlite DB{_objName} - id[{_id}]");
            }

            //数据放入数据集，避免二次查询
            _m_dicRef.Add(_id, list[0]);

            return list[0];
        }

        protected void _dealSuccessCallback(ALSqliteInfo _sqliteInfo)
        {
            try
            {
                if(null != _m_dDelegate)
                    _m_dDelegate();
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
                if(null != _m_dFailDelegate)
                    _m_dFailDelegate(typeof(T), this);
            }
            finally
            {
                _reset(); //try finally防止调用过程中有异常导致没置空
            }
        }
        
        protected void _reset()
        {
            _m_dDelegate = null;
            _m_dFailDelegate = null;
        }

        /// <summary>
        /// 当数据获取失败的时候调用的触发函数，可通过重载在子类处理
        /// </summary>
        /// <param name="_id"></param>
        protected virtual void _getRefFailed(string _id)
        {
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
