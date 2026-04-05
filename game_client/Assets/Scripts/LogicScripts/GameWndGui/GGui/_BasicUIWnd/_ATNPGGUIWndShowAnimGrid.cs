using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 用于每个item显示的时候播放show动画的grid基类
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _ATNPGGUIWndShowAnimGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ANPGGUIBasicGridSubWnd<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _ATNPGGUIMonoShowAnimGrid<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        protected _ATNPGGUIWndShowAnimGrid(_T_CONTAINER_MONO _wnd) : base(_wnd) 
        { 

        }

        // 是否是显示窗口后的第一次刷新Item
        private bool _m_bIsShowFirstRefresh = false;
        private HashSet<int> _m_hsPlaySet = new HashSet<int>();

        // 上一个item播放显示动画的时间
        private float _m_lastShowItemTime = 0;

        // 在延迟显示的item计数，用于计算延迟时间
        private int _m_showItemsCount = 0;

        // 上一个item刷新的时间， 用于每次都播放显示动画的功能中计算后续显示动画的延迟
        private float _m_fLastRefreshTime = 0;

        //是否播放过一次动画了，这个数据最好放到ondisard里面重置一下
        [NotNull]private HashSet<int> _m_hasPlayAniOnlyOneDoneSet = new HashSet<int>();

        private int _m_iShowAnimOpSerialize;

        public override void showWnd(Action _delegate)
        {
            base.showWnd(_delegate);

            // 如果未开启动画，不执行后续操作
            if (wnd == null || !wnd.openItemShowAnim)
                return;

            if (!_m_bIsShowFirstRefresh) // 避免单帧开启多次showWnd 导致item未刷新
            {
                _m_iShowAnimOpSerialize++;
                _m_bIsShowFirstRefresh = true;
                _m_showItemsCount = 0;
                _m_lastShowItemTime = Time.time;
                _m_hsPlaySet.Clear();
                // 显示窗口的时候先重置动画状态，解决延迟一帧刷新item导致跳帧的bug
                refreshAllItem((_wnd, _i) => {
                    if (_wnd != null)
                        _stopAnim(_wnd.wnd);
                });
            }

            //刷新一次显示
            _onScrollValueChg(Vector2.zero);
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollValueChg);
        }

        public override void hideWnd(Action _delegate)
        {
            _m_iShowAnimOpSerialize++;
            base.hideWnd(_delegate);
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
        }

        //设置所有为最后一帧
        public void setAllAniToEnd()
        {
            if (_m_showItemsCount > 0)
            {
                refreshAllItem((_wnd, _i) =>
                {
                    if (_wnd != null)
                        _setAnimToEnd(_wnd.wnd);
                });
            }
        }

        protected override void _onFrameRefresh()
        {
            // 如果未开启动画，不执行后续操作
            if (wnd == null || !wnd.openItemShowAnim)
                return;
            _m_bIsShowFirstRefresh = false;
            //清空集合，集合只在每一帧进行处理，避免刷新错误
            _m_hsPlaySet.Clear();
        }

        // 这里屏蔽掉，改用_onRefreshItemWnd
        protected sealed override void _refreshItemwnd(_T_ITEM_WND _itemMono, int _itemIdx)
        {
            _onRefreshItemWnd(_itemMono, _itemIdx);
            // 如果未开启动画，不执行后续操作
            if (wnd == null || !wnd.openItemShowAnim)
                return;
            _playItemRefreshAnim(_itemMono, _itemIdx, _m_bIsShowFirstRefresh);
        }

        protected void _playItemRefreshAnim(_T_ITEM_WND _itemWnd, int _itemIdx, bool _isFirstShowRefresh)
        {
            if (wnd == null || _itemWnd == null)
                return;

            //如果数据在集合内则不做处理。避免重复处理
            if (_m_hsPlaySet.Contains(_itemIdx))
                return;

            // 是否是showWnd的第一次刷新
            if (!_isFirstShowRefresh)
            {
                _setAnimToEnd(_itemWnd.wnd);
                return;
            }

            //如果只播放一次，并且已经播放过了，则不再播放
            if (wnd.isShowAniOnlyOne && _m_hasPlayAniOnlyOneDoneSet.Contains(_itemIdx))
            {
                _setAnimToEnd(_itemWnd.wnd);
                return;
            }
            
            _m_hasPlayAniOnlyOneDoneSet.Add(_itemIdx);
            
            //添加到播放数据集
            _m_hsPlaySet.Add(_itemIdx);

            _stopAnim(_itemWnd.wnd);
            _m_fLastRefreshTime = Time.time;
            float passTime = _m_fLastRefreshTime - _m_lastShowItemTime;
            // 是否是首个播放动画，是则直接播放动画，否则延迟播放
            if (passTime >= wnd.firstItemShowAnimDelay)
            {
                _playAnim(_itemWnd.wnd);
                _m_lastShowItemTime = _m_fLastRefreshTime;
            }
            else
            {
                // 在延迟显示的item计数，用于计算延迟时间
                _m_showItemsCount++;
                float delay = _m_showItemsCount * wnd.firstItemShowAnimDelay - passTime;
                /** 显示操作序列号 */
                int taskOp = _m_iShowAnimOpSerialize;

                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {

                    /** 非本地显示操作的task不执行 */
                    if (_m_iShowAnimOpSerialize != taskOp)
                        return;

                    // （快速滚动的时候会出现的情况，一个item接连设置为id1 ，12 ，则1的task就不再需要执行）当itemId变了说明这个task已经无效了，但需要减少计数，和更新上一个item显示的时间
                    if (_itemWnd.itemIdx != _itemIdx)
                    {
                        _m_showItemsCount--;
                        _m_lastShowItemTime = Time.time;
                        return;
                    }
                    _playAnim(_itemWnd.wnd);
                    _m_lastShowItemTime = Time.time;
                    _m_showItemsCount--;
                }, delay);
            }
        }

        // 动画设置为0，并暂停动画
        private void _stopAnim(_AALBasicUIWndMono _wnd)
        {
            if (null == _wnd || null == _wnd.wndAnimation)
                return;

            _wnd.wndAnimation.Play(_wnd.showAniName, PlayMode.StopAll);
            AnimationState state = _wnd.wndAnimation[_wnd.showAniName];
            if (state != null)
            {
                state.normalizedTime = 0;
                state.speed = 0;
            }
        }

        // 播放动画，并设置速度为1
        private void _playAnim(_AALBasicUIWndMono _wnd)
        {
            if (_wnd == null || _wnd.wndAnimation == null)
                return;

            AnimationState state = _wnd.wndAnimation[_wnd.showAniName];
            if (state != null)
                state.speed = 1;
        }

        // 直接设置动画为最后一帧
        private void _setAnimToEnd(_AALBasicUIWndMono _wnd)
        {
            if (null == _wnd || null == _wnd.wndAnimation)
                return;

            _wnd.wndAnimation.Play(_wnd.showAniName, PlayMode.StopAll);
            AnimationState state = _wnd.wndAnimation[_wnd.showAniName];
            if (state != null)
            {
                state.normalizedTime = 1;
                state.speed = 1;
            }
        }

        //列表滚动变化控制显隐
        private void _onScrollValueChg(Vector2 arg0)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            float value = 0;
            if (wnd.layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                value = wnd.scrollRect.verticalNormalizedPosition;
            else
                value = wnd.scrollRect.horizontalNormalizedPosition;

            //是否到最顶部(或最右)
            ALUGUICommon.setGameObjEnable(wnd.goScrollToTopShowList, value > 0.99f);
            ALUGUICommon.setGameObjEnable(wnd.goScrollToTopHideList, value <= 0.99f);
            //是否到最底部(或最左)
            ALUGUICommon.setGameObjEnable(wnd.goScrollToBottomShowList, value < 0.01f);
            ALUGUICommon.setGameObjEnable(wnd.goScrollToBottomHideList, value >= 0.01f);
        }

        protected abstract void _onRefreshItemWnd(_T_ITEM_WND _itemMono, int _itemIdx);
    }
}
