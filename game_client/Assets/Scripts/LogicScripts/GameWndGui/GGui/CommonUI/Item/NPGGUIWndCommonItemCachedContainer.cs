using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 通用物品带缓存的容器container
    /// </summary>
    public class NPGGUIWndCommonItemCachedContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoCommonItem, NPGGUIMonoCommonItemCachedContainer, NPGGUIWndCommonItem>
    {
        private NPGGUIWndCommonItemCache _m_ItemCache;
        //展示的数据队列存储，拷贝到本队列
        private List<CommonItemData> _m_lShowDataList;
        //当前展示的索引
        private int _m_iCurShowIndex;
        //展示操作序列号
        private long _m_lShowOpSerialize;

        //显示所有的最后回调
        private Action _m_dShowAllDelegate;

        public NPGGUIWndCommonItemCachedContainer(NPGGUIMonoCommonItemCachedContainer _containerMono) : base(_containerMono)
        {
            _m_ItemCache = new NPGGUIWndCommonItemCache(wnd.transform,0);
            _m_ItemCache.init(_containerMono.itemTemplate);

            _m_lShowDataList = new List<CommonItemData>();
            _m_iCurShowIndex = 0;
            _m_lShowOpSerialize = ALSerializeOpMgr.next();
            _m_dShowAllDelegate = null;
            
            initWnd();
        }

        public long curOpSerialize { get { return _m_lShowOpSerialize; } }

        protected override NPGGUIMonoCommonItem _instantiateItemMono(NPGGUIMonoCommonItem _template)
        {
            //返回空
            return null;
        }

        protected override NPGGUIWndCommonItem _createItemWnd(NPGGUIMonoCommonItem _itemMono)
        {
            return _m_ItemCache.popItem();
        }

        protected override void _discardItem(NPGGUIWndCommonItem _itemWnd)
        {
            _m_ItemCache.pushBackCacheItem(_itemWnd);
        }

        protected override void _releaseItemMono(NPGGUIMonoCommonItem _mono) { }

        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd() { }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            clearAllItem();
        }

        /// <summary>
        /// 清理所有显示数据，并重置相关属性
        /// </summary>
        public void clearAllItem()
        {
            clearAll();
            //拷贝数据集
            _m_lShowDataList.Clear();
            //重置下标
            _m_iCurShowIndex = 0;
            _m_lShowOpSerialize = ALSerializeOpMgr.next();
            //此时调用执行回调，保证回调会被处理
            _dealFinalDelegate();
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset() { }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard() { }

        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone() { }

        /// <summary>
        /// 展示对应的奖励数据
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_doneAction"></param>
        public void showRewardItemList(List<CommonItemData> _itemDataList, Action _doneAction = null)
        {
            if(_itemDataList == null)
            {
                if(_doneAction != null)
                    _doneAction();
                return;
            }
            clearAll();

            //拷贝数据集
            _m_lShowDataList.Clear();
            _m_lShowDataList.AddRange(_itemDataList);

            //过滤不需要展示的类型
            for (int i = _m_lShowDataList.Count - 1; i >= 0; i--)
            {
                if(!GCommon.itemCanShowInRewardPreview(_m_lShowDataList[i].getItemType(), _m_lShowDataList[i].subId))
                    _m_lShowDataList.RemoveAt(i);
            }

            //重置下标
            _m_iCurShowIndex = 0;
            _m_lShowOpSerialize = ALSerializeOpMgr.next();
            _m_dShowAllDelegate = _doneAction;

            //开启任务进行处理，第一个直接处理
            ALMonoTaskMgr.instance.addMonoTask(new NPGGUIWndCommonItemCachedGridAddItemDuration(this, _m_lShowOpSerialize, wnd.delayShowDuration));
        }

        /// <summary>
        /// 判断对应显示操作是否有效，是则显示下一个数据，如果无数据则返回false
        /// </summary>
        /// <param name="_opSerialize"></param>
        protected bool _showNextItem(long _opSerialize)
        {
            if (_m_lShowOpSerialize != _opSerialize)
            {
                //调用最后回调
                _dealFinalDelegate();
                return false;
            }

            //判断是否超出数据，超出则返回不需要继续
            if (_m_iCurShowIndex >= _m_lShowDataList.Count)
            {
                //调用最后回调
                _dealFinalDelegate();
                return false;
            }

            //显示当前数据
            CommonItemData itemData = _m_lShowDataList[_m_iCurShowIndex];
            NPGGUIWndCommonItem itemWnd = null;
            if (null != itemData)
            {
                itemWnd = addItemWnd();

                if (itemWnd != null)
                {
                    itemWnd.setItem(itemData);
                }
            }
            else
            {
                //报错
                ALLog.Error($"Show NPGGUIWndCommonItemCachedGrid Item err! when item over list index!");
            }

            //增加下标
            _m_iCurShowIndex++;

            //判断是否超出数据，超出则返回不需要继续
            if (_m_iCurShowIndex >= _m_lShowDataList.Count)
            {
                //调用最后回调
                _dealFinalDelegate();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 调用显示完成回调并重置回调
        /// </summary>
        protected void _dealFinalDelegate()
        {
            //调用最后回调
            if (null != _m_dShowAllDelegate)
                _m_dShowAllDelegate();
            _m_dShowAllDelegate = null;
        }

        /// <summary>
        /// 逐个添加显示项的定时处理函数
        /// </summary>
        protected class NPGGUIWndCommonItemCachedGridAddItemDuration : _IALBaseMonoTask
        {
            private NPGGUIWndCommonItemCachedContainer _m_gGrid;
            private long _m_lOpSerialize;
            private float _m_fDuration;

            public NPGGUIWndCommonItemCachedGridAddItemDuration(NPGGUIWndCommonItemCachedContainer _grid, long _opSerialize, float _duration)
            {
                _m_gGrid = _grid;
                _m_lOpSerialize = _opSerialize;
                _m_fDuration = _duration;
            }

            public void deal()
            {
                if (null == _m_gGrid)
                    return;

                //判断操作序列号是否一致
                if (_m_lOpSerialize != _m_gGrid.curOpSerialize)
                    return;

                //处理显示下一个对象，如果处理失败则不继续处理
                if (!_m_gGrid._showNextItem(_m_lOpSerialize))
                    return;

                //循环下一个周期处理
                ALMonoTaskMgr.instance.addMonoTask(this, _m_fDuration);
            }
        }
    }
}
