using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /************
     * 缓存池模板对象
     **/
    public abstract class _AALCacheUnExpandController<T, TEMP>
    {
        //模板对象
        private TEMP _m_tTemplateObj;

        //创建的对象缓存池，默认创建最少数量，超出最大数量则删除过多缓存
        private int _m_iMinCacheCount = 10;
        private int _m_iMaxCacheCount = 50;
        /** 是否警告 */
        //private int _m_iIsWarningCount;

        /** 总的缓存队列 */
        private List<T> _m_lTotalCacheList;
        /** 还可使用的缓存队列 */
        private List<T> _m_lEnableCacheList;
        /** 已经被使用的缓存队列 */
        private List<T> _m_lUsedItemList;

        protected _AALCacheUnExpandController(int _minCount, int _maxCount)
        {
            _m_tTemplateObj = default(TEMP);

            _m_iMinCacheCount = _minCount;
            _m_iMaxCacheCount = _maxCount;

            //_m_iIsWarningCount = _m_iMaxCacheCount;

            _m_lTotalCacheList = new List<T>(_maxCount);
            _m_lEnableCacheList = new List<T>(_maxCount);
            _m_lUsedItemList = new List<T>(_maxCount);
        }

        public int totalCount { get { return _m_lTotalCacheList.Count; } }
        public List<T> usedItemList { get { return _m_lUsedItemList; } }

        /****************
         * 带入模板对象进行初始化
         **/
        public void init(TEMP _template)
        {
            if (null != _m_tTemplateObj)
            {
                //输出错误
#if UNITY_EDITOR
                Debug.LogError("Init Cache Controller multiple times!");
#endif
                return;
            }

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
                    if (null == newItem)
                        break;

                    //先重置对象
                    _resetItem(newItem);
                    //将对象加入缓存队列
                    _m_lEnableCacheList.Add(newItem);
                    _m_lTotalCacheList.Add(newItem);
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
            for (int i = 0; i < _m_lTotalCacheList.Count; i++)
            {
                T item = _m_lTotalCacheList[i];
                //释放资源
                _discardItem(item);
            }
            _m_lTotalCacheList.Clear();

            //清空队列
            _m_lEnableCacheList.Clear();
            //重置模板
            _m_tTemplateObj = default(TEMP);

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
            //增加特效超出上限的警告
            //if (_m_lUsedItemList != null && _m_lUsedItemList.Count >= _m_iMaxCacheCount)
            //{
                //UnityEngine.Debug.LogError("cache over max num: " + _m_iMaxCacheCount + "! " + _warningTxt);
            //}

            //判断缓存是否有对象，有则直接返回
            if (_m_lEnableCacheList.Count > 0)
            {
                //取出最后一个对象
                T firstItem = _m_lEnableCacheList[_m_lEnableCacheList.Count - 1];
                _m_lEnableCacheList.RemoveAt(_m_lEnableCacheList.Count - 1);
                //放入使用队列
                _m_lUsedItemList.Add(firstItem);
                return firstItem;
            }

            //如无缓存对象则需要创建一个新的名称对象
            T newItem = _createItem(_m_tTemplateObj);
            //先重置对象
            _resetItem(newItem);
            //放入总缓存队列
            _m_lTotalCacheList.Add(newItem);
            //放入使用队列
            _m_lUsedItemList.Add(newItem);

            //返回结果
            return newItem;
        }

        /*****************
         * 将名称操作对象放回缓存队列
         **/
        public void pushBackCacheItem(T _item)
        {
            if (null == _item)
                return;

            //从使用队列删除 判断是否成功删除，未成功删除则不处理
            if(!_m_lUsedItemList.Remove(_item))
                return;

            //判断总缓存队列是否超出最大数量，是则删除这个对象
            if (_m_lTotalCacheList.Count > _m_iMaxCacheCount)
            {
                _m_lTotalCacheList.Remove(_item);
                _discardItem(_item);
                return;
            }

            //设置对象无效
            _resetItem(_item);
            //放入缓存队列
            _m_lEnableCacheList.Add(_item);
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
