using System;
using System.Collections.Generic;
using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 仿造主工程_ATALStateFactory
    /// </summary>
    /// <typeparam name="STATE_T"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class _ATHotfixStateFactory<STATE_T, T> where T : Enum where STATE_T : _ATHotfixStateBase<T>
    {
        //缓存不同类型以及对应类型的状态构造对象
        private Dictionary<Type, _THotfixBasicStateCacheController<STATE_T, T>> _m_dicStateCacheDic;

        public _ATHotfixStateFactory()
        {
            _m_dicStateCacheDic = new Dictionary<Type, _THotfixBasicStateCacheController<STATE_T, T>>();
        }

        /// <summary>
        /// 注册一个对应的缓存处理对象，用于状态机统一的生成和销毁状态
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void regCacheController(Type _type, _THotfixBasicStateCacheController<STATE_T, T> _cacheController)
        {
            if (null == _type || null == _cacheController)
                return;

            if(_m_dicStateCacheDic.ContainsKey(_type))
            {
                ALLog.Error($"multi reg {_type}");
                return;
            }

            _m_dicStateCacheDic.Add(_type, _cacheController);
        }

        /// <summary>
        /// 取出缓存
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public STATE_T popItem(Type _type)
        {
            _THotfixBasicStateCacheController<STATE_T, T> cacheController;
            if(!_m_dicStateCacheDic.TryGetValue(_type, out cacheController))
            {
                ALLog.Error($"can not find state reg {_type} to pop item");
                return null;
            }

            return cacheController.popItem();
        }

        /// <summary>
        /// 放回缓存
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public void pushbackItem(Type _type, STATE_T _obj)
        {
            if (null == _obj)
                return;

            _THotfixBasicStateCacheController<STATE_T, T> cacheController;
            if (!_m_dicStateCacheDic.TryGetValue(_type, out cacheController))
            {
                ALLog.Error($"can not find state reg {_type}");
                return ;
            }

            cacheController.pushBackCacheItem(_obj);
        }
    }
}