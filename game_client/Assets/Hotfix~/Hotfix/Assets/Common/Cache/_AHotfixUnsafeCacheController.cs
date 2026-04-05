using System;
using System.Collections.Generic;

using UnityEngine;

namespace Hotfix
{
    /************
     * 缓存池模板对象
     * Unsafe的定义是允许外部创建的对象放入缓存池
     **/
    public abstract class _AHotfixUnsafeCacheController<T, TEMP>
    {
        //是否初始化状态
        private bool _m_bIsInit;
        //模板对象
        private TEMP _m_tTemplateObj;

        //创建的对象缓存池，默认创建最少数量，超出最大数量则删除过多缓存
        private int _m_iMinCacheCount = 10;
        private int _m_iMaxCacheCount = 50;
        private int _m_iAddUnit = 1;
        /** 是否警告 */
        private int _m_iIsWarningCount;

        /** 总的缓存队列 */
        private int _m_iTotalCreateCount;
        /** 还可使用的缓存队列 */
        private List<T> _m_lEnableCacheList;
        private int _m_iEnableCount;

        protected _AHotfixUnsafeCacheController(int _minCount, int _maxCount)
        {
            _m_bIsInit = false;
            _m_tTemplateObj = default(TEMP);

            _m_iMinCacheCount = _minCount;
            _m_iMaxCacheCount = _maxCount;

            _m_iIsWarningCount = _m_iMaxCacheCount;

            _m_iTotalCreateCount = 0;
            _m_lEnableCacheList = new List<T>(_maxCount);
            _m_iEnableCount = 0;

            _m_iAddUnit = 1;
        }
        protected _AHotfixUnsafeCacheController(int _minCount, int _maxCount, int _addUnit)
        {
            _m_bIsInit = false;
            _m_tTemplateObj = default(TEMP);

            _m_iMinCacheCount = _minCount;
            _m_iMaxCacheCount = _maxCount;

            _m_iIsWarningCount = _m_iMaxCacheCount;

            _m_iTotalCreateCount = 0;
            _m_lEnableCacheList = new List<T>(_maxCount);
            _m_iEnableCount = 0;

            _m_iAddUnit = _addUnit;
        }

        /****************
         * 带入模板对象进行初始化
         **/
        public void init(TEMP _template)
        {
            if (_m_bIsInit)
            {
                //输出错误
#if UNITY_EDITOR
                Debug.LogError("Init Cache Controller multiple times!");
#endif
                return;
            }

            //设置初始化状态
            _m_bIsInit = true;
            //设置模板对象
            _m_tTemplateObj = _template;
            //创建名称显示对象池
            if (null != _m_tTemplateObj)
            {
                //逐个实例化子窗口对象
                for (int i = 0; i < _m_iMinCacheCount; i++)
                {
                    //创建控制对象
                    T newItem = _createItem(_m_tTemplateObj);
                    if (ReferenceEquals(null, newItem))
                        break;

                    //先重置对象
                    _resetItem(newItem);
                    //将对象加入缓存队列
                    _m_lEnableCacheList.Add(newItem);
                    _m_iTotalCreateCount++;
                    _m_iEnableCount++;
                }
            }

            //调用事件函数
            _onInit(_template);
        }

        /******************
         * 释放资源
         **/
        public void discard()
        {
            //释放所有cache对象队列
            for(int i = 0; i < _m_lEnableCacheList.Count; i++)
            {
                //释放资源
                _discardItem(_m_lEnableCacheList[i]);
            }

            //清空队列
            _m_lEnableCacheList.Clear();
            _m_iEnableCount = 0;
            //重置模板
            _m_tTemplateObj = default(TEMP);

            //触发事件函数
            _discard();
        }
        /// <summary>
        /// 本Cache销毁时触发函数
        /// </summary>
        protected virtual void _discard()
        {

        }

        /***************
         * 取出一个对象名称显示对象
         **/
        public T popItem()
        {
            if (_m_iEnableCount <= 0)
            {
                //根据增量创建
                _addCache();

                //增加特效超出上限的警告
                if (_m_iTotalCreateCount > _m_iIsWarningCount)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogWarning("cache over max num: " + _m_iIsWarningCount + "! " + _warningTxt);
                    //判断是否超过需要报错的数量1000
                    if (_m_iTotalCreateCount > 1000)
                    {
                        UnityEngine.Debug.LogError("cache over max num: " + _m_iTotalCreateCount + "! " + _warningTxt);
                    }
#endif
                    _m_iIsWarningCount = _m_iIsWarningCount + (_m_iIsWarningCount / 2);
                    _m_iMaxCacheCount = _m_iIsWarningCount;
                }
            }

            //判断缓存是否有对象，有则直接返回
            if (_m_iEnableCount > 0)
            {
                //取出最后一个对象
                T firstItem = _m_lEnableCacheList[_m_iEnableCount - 1];
                _m_iEnableCount--;
                return firstItem;
            }

            //此时还无数据则返回结果
            return default(T);
        }

        /*****************
         * 将名称操作对象放回缓存队列
         **/
        public void pushBackCacheItem(T _item)
        {
            if (ReferenceEquals(null, _item))
                return;

            //设置对象无效
            _resetItem(_item);
            //放入缓存队列
            if (_m_iEnableCount < _m_lEnableCacheList.Count)
                _m_lEnableCacheList[_m_iEnableCount] = _item;
            else
                _m_lEnableCacheList.Add(_item);
            _m_iEnableCount++;
        }

        protected void _addCache()
        {
            //根据增量创建
            for (int i = 0; i < _m_iAddUnit; i++)
            {
                //如无缓存对象则需要创建一个新的名称对象
                T newItem = _createItem(_m_tTemplateObj);
                //先重置对象
                _resetItem(newItem);
                //放入总缓存队列
                _m_iTotalCreateCount++;
                //放入缓存队列
                if (_m_iEnableCount < _m_lEnableCacheList.Count)
                    _m_lEnableCacheList[_m_iEnableCount] = newItem;
                else
                    _m_lEnableCacheList.Add(newItem);
                _m_iEnableCount++;
            }
        }

        //警告信息文字
        protected abstract string _warningTxt { get; }

        //初始化时的事件函数
        protected abstract void _onInit(TEMP _template);
        //根据模板创建对象的函数
        protected abstract T _createItem(TEMP _template);
        //释放创建出来的对象的资源
        protected abstract void _discardItem(T _item);
        //设置对象无效
        protected abstract void _resetItem(T _item);
    }
}
