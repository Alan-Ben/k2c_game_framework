using ALPackage;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 用于每个item显示的时候播放show动画的container基类
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _ATNPGGUIWndShowAnimContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ANPGGUIBasicSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _ATNPGGUIMonoShowAnimContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        protected _ATNPGGUIWndShowAnimContainer(_T_CONTAINER_MONO _containerMono) : base(_containerMono)
        {
            _m_isPlayAniOnlyOneDone = false;
        }

        // 是否是显示窗口后的第一次刷新Item
        private bool _m_bIsShowFirstRefresh = false;

        // 上一个item播放显示动画的时间
        private float _m_lastShowItemTime = 0;

        // 在延迟显示的item计数，用于计算延迟时间
        private int _m_showItemsCount = 0;

        // 上一个item刷新的时间， 用于每次都播放显示动画的功能中计算后续显示动画的延迟
        private float _m_fLastRefreshTime = 0;

        /** 显示操作序列号 */
        private int _m_iShowOpSerialize;

        //是否播放过一次动画了，这个数据最好放到ondisard里面重置一下
        private bool _m_isPlayAniOnlyOneDone = false;

        //是否强制设置动画到结束
        private bool _m_bIsForceSetAniToEnd;
        
        public int showOpSerialize { get { return _m_iShowOpSerialize; } }

        public override void showWnd(Action _delegate)
        {
            base.showWnd(_delegate);
            //已经有效则不进行后续操作
            if (wnd != null && wnd.openItemShowAnim && !_m_bIsShowFirstRefresh)
            {
                _m_iShowOpSerialize++;
                _m_bIsShowFirstRefresh = true;
                _m_bIsForceSetAniToEnd = false;
                _m_showItemsCount = 0;
                _m_lastShowItemTime = Time.time;
                // 解决已经加载item的container，第一帧闪的问题
                refreshAllItem(_wnd => {
                    if (_wnd != null)
                        _stopAnim(_wnd.wnd);
                });
                ALCommonTaskController.CommonActionAddNextFrameLaterTask(_onNextLateUpdate);
            }

            //刷新一次显示
            _onScrollValueChg(Vector2.zero);
            wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollValueChg);
        }

        public override void hideWnd(Action _delegate)
        {
            if (wnd != null && wnd.openItemShowAnim)
                _m_iShowOpSerialize++;

            _m_bIsForceSetAniToEnd = false;
            base.hideWnd(_delegate);
            wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
        }

        /// <summary>
        /// 强制设置每个item的动画结束
        /// </summary>
        public void forceSetAniToEnd()
        {
            _m_bIsForceSetAniToEnd = true;
            refreshAllItem(_wnd =>
            {
                if (_wnd != null)
                    _setAnimToEnd(_wnd.wnd);
            });
        }

        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {
            if (null == _itemWnd)
                return;

            // 仅在 showWnd 那一帧增加的item才播放显示动画
            if (wnd != null && wnd.openItemShowAnim && _m_bIsShowFirstRefresh)
                _stopAnim(_itemWnd.wnd);
        }

        /// <summary>
        /// 下一帧的时候播放显示动画，并根据显示时间间隔播放动画
        /// </summary>
        private void _onNextLateUpdate()
        {
            refreshAllItem(_wnd => { _playItemRefreshAnim(_wnd, _m_bIsShowFirstRefresh); });
            _m_bIsShowFirstRefresh = false;
            _m_isPlayAniOnlyOneDone = true;
        }

        protected void _playItemRefreshAnim(_T_ITEM_WND _itemWnd, bool _isFirstShowRefresh)
        {
            if (wnd == null || _itemWnd == null || !wnd.openItemShowAnim)
                return;

            // 是否是showWnd的第一次刷新
            if (!_isFirstShowRefresh)
            {
                _setAnimToEnd(_itemWnd.wnd);
            }
            //如果已经播放过动画，并且只播放一次，那就不播放了
            else if (wnd.isShowAniOnlyOne && _m_isPlayAniOnlyOneDone)
            {
                _setAnimToEnd(_itemWnd.wnd);
            }
            else
            {
                _stopAnim(_itemWnd.wnd);
                _m_fLastRefreshTime = Time.time;
                float passTime = _m_fLastRefreshTime - _m_lastShowItemTime;

                float delay = _m_showItemsCount * wnd.eachItemShowAnimInterval + wnd.firstItemStartShowAnimDelay;
                _m_showItemsCount++;

                /** 显示操作序列号 */
                int taskOp = showOpSerialize;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    /** 非本地显示操作的task不执行 */
                    if (showOpSerialize != taskOp || _m_bIsForceSetAniToEnd)
                        return;

                    _playAnim(_itemWnd.wnd);
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
            if (wnd.itemContainer is HorizontalLayoutGroup)
                value = wnd.scrollRect.horizontalNormalizedPosition;
            else
                value = wnd.scrollRect.verticalNormalizedPosition;

            //是否到最顶部(或最右)
            ALUGUICommon.setGameObjEnable(wnd.goScrollToTopShowList, value > 0.99f);
            ALUGUICommon.setGameObjEnable(wnd.goScrollToTopHideList, value <= 0.99f);
            //是否到最底部(或最左)
            ALUGUICommon.setGameObjEnable(wnd.goScrollToBottomShowList, value < 0.01f);
            ALUGUICommon.setGameObjEnable(wnd.goScrollToBottomHideList, value >= 0.01f);
        }
    }
}
