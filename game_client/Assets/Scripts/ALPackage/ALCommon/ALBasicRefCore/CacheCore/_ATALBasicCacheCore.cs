using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace ALPackage
{
    /*********************
     * 数据集合中心管理对象
     * 此对象的数据不是在一开始就进行初始化的，而是根据子类的查询进行的缓存初始化，并存起来以备以后使用
     **/
    public abstract class _ATALBasicCacheCore<T> : _IALInitRefObj where T : new()
    {
        /** 初始化状态位 */
        private bool _m_bInit;
        /** 回调处理函数 */
        private Action _m_dDelegate;

        /** 存储对应id索引的数据映射表,dictionary不可以用模板，会导致ios报错 */
        private Dictionary<long, object> _m_dicRef;

        protected _ATALBasicCacheCore() 
        {
            _m_bInit = false;

            _m_dicRef = new Dictionary<long, object>();
        }

        public bool isInit { get { return _m_bInit; } }
        public Action finalDelegate { get { return _m_dDelegate; } }

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

            _m_bInit = true;

            //调用子类初始化函数
            _init(_doneDelegate, _onFail);
        }

        /// <summary>
        /// 根据id获取对应的数据
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_checkFail"></param>
        /// <returns></returns>
        public T getRef(long _id, bool _checkFail = true)
        {
            //判断是否有缓存数据，如有则直接返回
            object tmpObj = null;
            if (_m_dicRef.TryGetValue(_id, out tmpObj))
            {
                return (T)tmpObj;
            }

            //尝试进行数据库检索
            T searchRes;
            if(!_searchData(_id, out searchRes))
            {
                //如果检索失败则加入默认数据对象
                searchRes = _makeEmptyObj();
            }

            //加入数据集
            _m_dicRef.Add(_id, searchRes);

            //返回结果
            return searchRes;
        }
        
        protected void _reset()
        {
            _m_dDelegate = null;
        }

        /// <summary>
        /// 子类初始化函数
        /// 如数据库可能需要连接等
        /// </summary>
        protected abstract void _init(Action _doneDelegate, System.Action<Type, _IALInitRefObj> _onFail);

        /// <summary>
        /// 根据Id即时检索对应的数据对象，检索到的结果将会缓存起来进行处理
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_obj"></param>
        /// <returns></returns>
        protected abstract bool _searchData(long _id, out T _obj);

        /// <summary>
        /// 构造一个空对象，对检索失败的数据进行缓存处理
        /// </summary>
        /// <returns></returns>
        protected abstract T _makeEmptyObj();
    }
}
