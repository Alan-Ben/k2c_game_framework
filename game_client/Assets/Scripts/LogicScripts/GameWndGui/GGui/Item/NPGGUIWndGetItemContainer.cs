using ALPackage;
using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 物品列表，支持根据物品类型显示不同样式
    /// </summary>
    public class NPGGUIWndGetItemContainer : _ATALBasicUISubWnd<NPGGUIMonoGetItemContainer>
    {
        private List<NPCommon_ItemInfo> _m_lItemList; //物品列表
        private int _m_iCurShowIndex;//当前显示的下标
        private NPGGUIWndCommonItemCache _m_wCommonItemWndCache; //通用物品缓存池，脚本直接配置预制体
        // private NPGGUIWndGetPetItemLoadCache _m_wGetPetItemWndCache; //宠物实例item缓存池，需要的时候才加载
        private ALCommonEnableTaskController _m_ftShowItemTask; //逐个显示item的任务
        private long _m_lShowSerialize;//显示序列号

        public NPGGUIWndGetItemContainer(NPGGUIMonoGetItemContainer _mono) : base(_mono)
        {
            initWnd();
        }

        public event Action onShowItem;
        public event Action onAllItemShowed;
        
        public int totalItemCount { get { return _m_lItemList == null ? 0 : _m_lItemList.Count; } }
        public int curShowIndex { get { return _m_iCurShowIndex; } }
        public bool allItemShowed { get { return _m_lItemList == null || _m_iCurShowIndex >= _m_lItemList.Count; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _discardShowItemTask();
        }

        protected override void _onReset()
        {
            _m_iCurShowIndex = 0;
            _m_lItemList?.Clear();

            //回收所有item
            _pushBackAllCacheItem();
        }

        protected override void _onDiscard()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_iCurShowIndex = 0;
            _m_lItemList?.Clear();
            _m_lItemList = null;

            //回收所有item
            _pushBackAllCacheItem();

            //销毁所有缓存池
            _discardAllItemCache();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lItemList = new List<NPCommon_ItemInfo>();

            //初始化所有缓存池
            _initAllItemCache();
        }


        #region 任务

        /// <summary>
        /// 初始化逐个显示item任务
        /// </summary>
        private void _initShowItemTask()
        {
            if (wnd == null)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            _discardShowItemTask();

            Action dealShowItem = () =>
            {
                if (wnd == null)
                    return;

                //没有间隔，直接全部显示
                if (wnd.showItemIntervalTime <= 0)
                {
                    showAllItem();
                }
                else
                {
                    _m_ftShowItemTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_showSingleItem, wnd.showItemIntervalTime);
                }
            };
            
            //第一个增加延时
            if (_m_iCurShowIndex == 0 && wnd.firstItemStartShowDelay > 0)
            {
                long serialize = _m_lShowSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                        return;

                    dealShowItem();
                }, wnd.firstItemStartShowDelay);
            }
            else
                dealShowItem();
        }

        /// <summary>
        /// 销毁逐个显示item任务
        /// </summary>
        private void _discardShowItemTask()
        {
            _m_ftShowItemTask.setDisable();
        }

        /// <summary>
        /// 显示一个item
        /// </summary>
        private void _showSingleItem()
        {
            if (wnd == null
                || _m_lItemList == null
                || _m_lItemList.Count <= 0)
            {
                _discardShowItemTask();
                onAllItemShowed?.Invoke();
                
                return;
            }

            //获取显示的itemWnd
            NPCommon_ItemInfo itemInfo = _m_lItemList.SafeGet(_m_iCurShowIndex);
            _IALBasicUIWndInterface itemWnd = _getItemWndByItemInfo(itemInfo);

            //调用单个item显示事件
            _onShowItem(itemWnd);
            onShowItem?.Invoke();

            //下标递增
            _m_iCurShowIndex++;

            //判断是否结束
            if (_m_iCurShowIndex >= _m_lItemList.Count)
            {
                _discardShowItemTask();
                onAllItemShowed?.Invoke();
            }
        }

        #endregion


        #region 功能方法

        /// <summary>
        /// 加载所有需要的特殊item模板
        /// </summary>
        /// <param name="_onLoaded"></param>
        protected void _loadAllItemTemp(Action _onLoaded)
        {
            //获取需要加载的对象列表
            List<_AALBasicLoadObj> loadObjList = _getNeedLoadObjList(_m_lItemList);

            //没有需要加载的模板，直接返回
            if (loadObjList == null || loadObjList.Count <= 0)
            {
                _onLoaded?.Invoke();
                return;
            }

            //开始加载
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_onLoaded);
            stepCounter.chgTotalStepCount(loadObjList.Count);
            for (int i = 0; i < loadObjList.Count; i++)
            {
                loadObjList[i].load(stepCounter.addDoneStepCount);
            }
        }

        /// <summary>
        /// 初始化所有缓存池
        /// </summary>
        private void _initAllItemCache()
        {
            if (wnd == null || wnd.cacheParent == null)
                return;

            //通用物品缓存池
            if (wnd.itemTemplate != null)
            {
                _m_wCommonItemWndCache = new NPGGUIWndCommonItemCache(wnd.cacheParent, 1, 20);
                _m_wCommonItemWndCache.init(wnd.itemTemplate);
            }

            //特殊物品缓存池
            if (wnd.itemTempParams != null)
            {
                for (int i = 0; i < wnd.itemTempParams.Count; i++)
                {
                    NPGGUIGetItemResParama temp = wnd.itemTempParams[i];
                    switch (temp.itemType)
                    {
                        //配置了不支持的类型，报个警告
                        default:
                            Debug.LogWarning(
                                $"【NPGGUIWndGetItemContainer._initAllItemCache Warning】:物品类型:{temp.itemType}配置了特殊样式路径信息，但代码还未支持，将使用CommonItemWnd显示");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取需要加载的对象列表
        /// </summary>
        /// <param name="_itemList"></param>
        /// <returns></returns>
        private List<_AALBasicLoadObj> _getNeedLoadObjList(List<NPCommon_ItemInfo> _itemList)
        {
            if (_itemList == null)
                return null;

            List<_AALBasicLoadObj> loadObjList = new List<_AALBasicLoadObj>();
            for (int i = 0; i < _itemList.Count; i++)
            {
                NPCommon_ItemInfo temp = _itemList[i];
                if (temp == null)
                    continue;

                //只加载特殊类型
                switch ((ENPItemType)temp.getItemType())
                {
                    //宠物实例类型
                    // case ENPItemType.PET:
                        // if (_m_wGetPetItemWndCache != null
                        //     && !_m_wGetPetItemWndCache.isLoaded
                        //     && !loadObjList.Contains(_m_wGetPetItemWndCache))
                        // {
                        //     loadObjList.Add(_m_wGetPetItemWndCache);
                        // }
                        // break;
                }
            }
            return loadObjList;
        }

        /// <summary>
        /// 解析物品信息，返回item窗体
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <returns></returns>
        private _IALBasicUIWndInterface _getItemWndByItemInfo(NPCommon_ItemInfo _itemInfo)
        {
            if (_itemInfo == null)
                return null;

            //根据类型从不同缓存池中获取
            switch ((ENPItemType)_itemInfo.getItemType())
            {
                //宠物实例类型
                // case ENPItemType.PET:
                    // if (_m_wGetPetItemWndCache != null)
                    // {
                    //     NPGGUIWndGetPetItem itemWnd = _m_wGetPetItemWndCache.popItem();
                    //     itemWnd?.setInfo(_itemInfo.toGetPetInfo());
                    //     return itemWnd;
                    // }
                    // break;

                //默认走CommonItemWnd
                default:
                    if (_m_wCommonItemWndCache != null)
                    {
                        NPGGUIWndCommonItem itemWnd = _m_wCommonItemWndCache.popItem();
                        itemWnd?.setItem(new CommonItemData(_itemInfo));
                        return itemWnd;
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// 回收所有item
        /// </summary>
        private void _pushBackAllCacheItem()
        {
            _m_wCommonItemWndCache?.pushBackAllItem();
            // _m_wGetPetItemWndCache?.pushBackAllItem();
        }

        /// <summary>
        /// 销毁所有缓存池
        /// </summary>
        private void _discardAllItemCache()
        {
            _m_wCommonItemWndCache?.discard();
            _m_wCommonItemWndCache = null;

            // _m_wGetPetItemWndCache?.discard();
            // _m_wGetPetItemWndCache = null;
        }

        /// <summary>
        /// 开始展示item列表
        /// </summary>
        private void _startShowItem()
        {
            showWnd();
            if (_m_lItemList != null && _m_lItemList.Count > 0)
            {
                _initShowItemTask();
            }
            else
            {
                onAllItemShowed?.Invoke();
            }
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 显示item时调用
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onShowItem(_IALBasicUIWndInterface _itemWnd)
        {
            if (_itemWnd == null || _itemWnd.getGameObj() == null || wnd == null || wnd.content == null)
                return;

            _itemWnd.getGameObj().transform.SetParent(wnd.content.transform, false);
            _itemWnd.getGameObj().transform.localPosition = Vector3.zero;
            _itemWnd.getGameObj().transform.localScale = Vector3.one;
            _itemWnd.showWnd();
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 显示物品列表
        /// </summary>
        /// <param name="_itemList"></param>
        public void showItemList(List<NPCommon_ItemInfo> _itemList)
        {
            if (_itemList == null || _m_lItemList == null)
                return;

            _pushBackAllCacheItem();
            _m_iCurShowIndex = 0;
            _m_lItemList.Clear();
            _m_lItemList.AddRange(_itemList);
            _loadAllItemTemp(_startShowItem);
        }
        
        /// <summary>
        /// 显示物品列表
        /// </summary>
        /// <param name="_itemList"></param>
        public void addItemList(List<NPCommon_ItemInfo> _itemList)
        {
            if (_itemList == null || _m_lItemList == null)
                return;
            _m_lItemList.AddRange(_itemList);
            _loadAllItemTemp(_startShowItem);
        }

        /// <summary>
        /// 显示所有item
        /// </summary>
        public void showAllItem()
        {
            if (_m_lItemList == null || _m_iCurShowIndex >= _m_lItemList.Count)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();

            //显示剩余item
            if (_m_lItemList != null)
            {
                for (; _m_iCurShowIndex < _m_lItemList.Count; _m_iCurShowIndex++)
                {
                    _IALBasicUIWndInterface itemWnd = _getItemWndByItemInfo(_m_lItemList.SafeGet(_m_iCurShowIndex));
                    _onShowItem(itemWnd);
                }
            }

            //销毁显示任务
            _discardShowItemTask();

            //手动调用全部显示，单独触发一次事件
            onShowItem?.Invoke();
            onAllItemShowed?.Invoke();
        }

        /// <summary>
        /// 物品容器滚动到最低端
        /// </summary>
        public void scrollMoveToBottom()
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if (wnd == null || wnd.scrollRect == null)
                    return;

                wnd.scrollRect.verticalNormalizedPosition = 0;
            });
        }

        #endregion
    }
}
