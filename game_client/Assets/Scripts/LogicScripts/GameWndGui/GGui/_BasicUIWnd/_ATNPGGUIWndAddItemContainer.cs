using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 逐个显示item的container抽象模板基类
    /// </summary>
    public abstract class _ATNPGGUIWndAddItemContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ANPGGUIBasicSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _ATNPGGUIMonoAddItemContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        private List<_T_ITEM_WND> _m_lItemList;//子窗体列表
        private bool _m_bIsShowingItem;//是否正在逐个显示item
        private Action _m_aDoneAction;//逐个显示结束回调
        private int _m_iCurShowIndex;//当前显示到的item下标
        private ALCommonEnableTaskController _m_ftShowItemTask;//逐个显示item的任务


        /// <summary> 是否正在逐个显示item </summary>
        public bool isShowingItem { get => _m_bIsShowingItem; }

        protected _ATNPGGUIWndAddItemContainer(_T_CONTAINER_MONO _mono) : base(_mono)
        {
            initWnd();
        }

        protected sealed override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            //隐藏窗体时视为跳过流程，直接显示所有item
            showAllItem();

            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            if (_m_lItemList != null)
                _m_lItemList.Clear();
            _m_lItemList = null;

            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            _m_lItemList = new List<_T_ITEM_WND>();

            _onWndInitDoneEx();
        }


        #region 外部调用

        /// <summary>
        /// 显示item列表，根据脚本相关配置逐个显示
        /// </summary>
        /// <typeparam name="_T_ITEM_DATA"></typeparam>
        /// <param name="_dataList">数据列表</param>
        /// <param name="_setDataAction">设置数据</param>
        /// <param name="_doneAction">全部item显示完成回调</param>
        public void showItemList<_T_ITEM_DATA>(List<_T_ITEM_DATA> _dataList, Action<_T_ITEM_WND, _T_ITEM_DATA> _setDataAction, Action _doneAction = null)
            where _T_ITEM_DATA : _IItem
        {
            if (_dataList == null || _m_lItemList == null)
            {
                _doneAction?.Invoke();
                return;
            }

            //强制结束未完成的显示流程
            _forceEndLastShowTask();

            //设置结束回调
            _m_aDoneAction = _doneAction;

            //设置窗体数据并隐藏
            _T_ITEM_WND itemWnd = null;
            _T_ITEM_DATA itemData = default(_T_ITEM_DATA);
            int count = 0;
            //遍历数据
            for (int i = 0; i < _dataList.Count; i++)
            {
                itemData = _dataList[i];
                if (null == itemData)
                    continue;

                //判断该道具类型是否需要展示
                if (!GCommon.itemCanShowInRewardPreview(itemData.getItemType(), itemData.subId))
                    continue;

                //如果容器内部个数不足则新增视图
                if (count >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;

                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                {
                    itemWnd = _m_lItemList[count];
                }

                //设置窗体数据并隐藏
                _setDataAction?.Invoke(itemWnd, itemData);
                itemWnd.hideWndWithoutAni();

                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            //设置窗体数据结束
            _onSetDataDone();

            //初始化逐个显示任务
            _initShowItemTask();

            //标志正在显示item
            _m_bIsShowingItem = true;
        }

        /// <summary>
        /// 显示所有item，同时会调用完成回调，并标记isShowingItem为false
        /// </summary>
        public void showAllItem()
        {
            //未在显示，不处理
            if (!_m_bIsShowingItem)
                return;

            //显示剩余item
            if (_m_lItemList != null)
            {
                for (int i = _m_iCurShowIndex; i < _m_lItemList.Count; i++)
                {
                    if (i < 0)
                        continue;

                    //显示item
                    _m_lItemList[i]?.showWnd();

                    //调用跳过时显示item事件
                    _onSkipShowItem(i, _m_lItemList[i]);
                }
            }

            //执行显示结束事件
            _onAllItemShowDone();
        }

        #endregion


        #region 任务

        /// <summary>
        /// 初始化逐个显示item任务
        /// </summary>
        private void _initShowItemTask()
        {
            if (wnd == null)
            {
                _onAllItemShowDone();
                return;
            }

            _discardShowItemTask();
            _m_ftShowItemTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_showSingleItem, wnd.showItemIntervalTime);
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
            //判断数据是否有误
            if (_m_lItemList == null
                || _m_lItemList.Count <= 0
                || _m_iCurShowIndex < 0
                || _m_iCurShowIndex >= _m_lItemList.Count)
            {
                _onAllItemShowDone();
                return;
            }

            //显示一个item
            _m_lItemList[_m_iCurShowIndex]?.showWnd();

            //调用显示事件
            _onShowItem(_m_iCurShowIndex, _m_lItemList[_m_iCurShowIndex]);

            //下标递增
            _m_iCurShowIndex++;

            //判断是否结束
            if (_m_iCurShowIndex >= _m_lItemList.Count)
            {
                _onAllItemShowDone();
            }
        }

        #endregion


        /// <summary>
        /// 所有item显示结束
        /// </summary>
        private void _onAllItemShowDone()
        {
            //执行结束回调
            _m_aDoneAction?.Invoke();
            _m_aDoneAction = null;

            //销毁逐个显示item任务
            _discardShowItemTask();

            //重置显示数据
            _resetShowData();
        }

        /// <summary>
        /// 强制结束上一次的显示任务
        /// </summary>
        private void _forceEndLastShowTask()
        {
            _onAllItemShowDone();
        }

        /// <summary>
        /// 重置显示数据
        /// </summary>
        private void _resetShowData()
        {
            _m_iCurShowIndex = 0;
            _m_bIsShowingItem = false;
            _m_aDoneAction = null;
            _onResetShowData();
        }


        #region 滚动条相关
        public void scrollMoveToBottom()
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                moveToBottom();
            });
        }

        public void scrollMoveToTop()
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                moveToTop();
            });
        }

        public void scrollMoveToLeft()
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                moveToLeft();
            });
        }

        public void scrollMoveToRight()
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                moveToRight();
            });
        }
        #endregion


        #region 子类实现
        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        /// <summary>
        /// 逐个显示item时调用
        /// </summary>
        /// <param name="_itemIndex">item下标</param>
        /// <param name="_itemWnd">窗体</param>
        protected virtual void _onShowItem(int _itemIndex, _T_ITEM_WND _itemWnd) { }
        /// <summary>
        /// 调用showAllItem时，对每个item调用
        /// </summary>
        /// <param name="_itemIndex">item下标</param>
        /// <param name="_itemWnd">窗体</param>
        protected virtual void _onSkipShowItem(int _itemIndex, _T_ITEM_WND _itemWnd) { }
        /// <summary>
        /// showItemList结束调用
        /// </summary>
        protected virtual void _onSetDataDone() { }
        /// <summary>
        /// 重置显示数据时调用
        /// </summary>
        protected virtual void _onResetShowData() { }
        #endregion
    }
}
