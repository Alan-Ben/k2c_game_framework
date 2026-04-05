using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 游戏中的基本信息对象存储对象
    /// </summary>
    public abstract class _ATHotfixBasicRefListCore<T> : _ATHotfixTextDefaultRefCore<T> where T : _IBaseHotfixRefObj, new()
    {
        //存储对应id索引的数据映射表
        private List<T> _m_refList;
        //是否已经初始化数据，没有初始化的时候就访问的话会报错
        private bool _m_bIsInit = false;
        
        public List<T> refList
        {
            get
            {
                _checkInit();
                
                return _m_refList;
            }
        }
        
        
        /// <summary>
        /// 直接带入数据集进行初始化
        /// </summary>
        /// <param name="_refList"></param>
        protected override void _initData(List<T> _refList)
        {
            if(null == _refList)
                return;

            _m_bIsInit = true;

            _m_refList = new List<T>(_refList.Count);
            //遍历数据集插入映射表
            for(int i = 0; i < _refList.Count; i++)
            {
                T refObj = _refList[i];
                if(null == refObj)
                    continue;

                //放入映射表
                _m_refList.Add(refObj);
            }
        }

        /*******************
         * 根据id获取对应的数据
         **/
        public T getRef(long _id)
        {
            _checkInit();

            if (null == _m_refList)
                return default;
            
            T tmp = default(T);
            for (int i = 0; i < _m_refList.Count; i++)
            {
                tmp = _m_refList[i];
                if(tmp == null)
                    continue;

                if (tmp._refId == _id)
                    return _m_refList[i];
            }

            return default;
        }
        
        public T getRef(Func<T, bool> _findFunc)
        {
            _checkInit();
            
            if (null == _m_refList)
                return default;
            
            for(int i = 0; i < _m_refList.Count; i++)
            {
                if (_findFunc(_m_refList[i]))
                    return _m_refList[i];
            }

            return default(T);
        }

        /*******************
         * 逐个处理
         **/
        public void dealAllRef(Action<T> _action)
        {
            if (null == _action)
                return;

            _checkInit();
            
            if (null == _m_refList)
                return;
            
            for(int i = 0; i < _m_refList.Count; i++)
            {
                _action(_m_refList[i]);
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        protected void _clear()
        {
            if(null != _m_refList)
                _m_refList.Clear();
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
