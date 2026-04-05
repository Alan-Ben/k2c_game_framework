using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 游戏中的基本信息对象存储对象
    /// </summary>
    public abstract class _ATHotfixBasicRefMapCore<T> : _ATHotfixTextDefaultRefCore<T> where T : _IBaseHotfixRefObj, new()
    {
        //存储对应id索引的数据映射表
        private Dictionary<long, T> _m_refMap;
        //是否已经初始化数据，没有初始化的时候就访问的话会报错
        private bool _m_bIsInit = false;
        
        /// <summary>
        /// 直接带入数据集进行初始化
        /// </summary>
        /// <param name="_refList"></param>
        protected override void _initData(List<T> _refList)
        {
            if(null == _refList)
                return;

            _m_bIsInit = true;

            _m_refMap = new Dictionary<long, T>(_refList.Count);
            //遍历数据集插入映射表
            for(int i = 0; i < _refList.Count; i++)
            {
                T refObj = _refList[i];
                if(null == refObj)
                    continue;

                if (_m_refMap.ContainsKey(refObj._refId))
                {
#if UNITY_EDITOR
                    Debug.LogError($"[EDITOR]重复配表id！！！: {refObj._refId} 配表是这个 : {typeof(T)}");
#endif
                    continue;
                }
                
                //放入映射表
                _m_refMap.Add(refObj._refId, refObj);
            }
        }

        /// <summary>
        /// 增加一个新的配表对象
        /// </summary>
        /// <param name="_refObj"></param>
        public void addRef(T _refObj)
        {
            if (null == _refObj)
                return;
            
            _checkInit();
            
            if(null == _m_refMap)
                return;
            
            if (_m_refMap.ContainsKey(_refObj._refId))
            {
#if UNITY_EDITOR
                Debug.LogError($"[EDITOR]重复配表id！！！: {_refObj._refId} 配表是这个 : {typeof(T)}");
#endif
                return;
            }
                
            //放入映射表
            _m_refMap.Add(_refObj._refId, _refObj);
        }

        
        /// <summary>
        /// 根据id获取对应的数据
        /// </summary>
        /// <param name="_id"></param>
        public T getRef(long _id)
        {
            _checkInit();
            
            if(null == _m_refMap)
                return default;
            
            if (!_m_refMap.ContainsKey(_id))
                return default;
            
            return _m_refMap[_id];
        }
        

        /// <summary>
        /// 获取所有数据对象的队列
        /// </summary>
        /// <returns></returns>
        public List<T> makeNewAllRefList()
        {
            _checkInit();
            
            if(null == _m_refMap)
                return null;
            
            List<T> list = new List<T>();
            list.AddRange(_m_refMap.Values);

            return list;
        }
        
        /// <summary>
        /// 逐个处理
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllRef(Action<T> _action)
        {
            if (null == _action)
                return;

            _checkInit();
            
            if(null == _m_refMap)
                return;
            
            foreach (T obj in _m_refMap.Values)
            {
                _action(obj);
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        protected void _clear()
        {
            if (_m_refMap != null) 
                _m_refMap.Clear();
        }
        
        private void _checkInit()
        {
            if (_m_bIsInit == false)
            {
                UnityEngine.Debug.LogError($"在配表还没初始化就尝试访问：{this.GetType()}");
            }
        }
    }
}