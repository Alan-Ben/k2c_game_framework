using System;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 热更工程用于每个item显示的时候播放show动画的container基类
    /// </summary>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _AHotfixBaseShowAnimContainerWnd<_T_CONTAINER_MONO, _T_ITEM_WND> : _AHotfixBaseContainerWnd<_T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_CONTAINER_MONO : _AHotfixShowAnimContainerBaseMono, new()
        where _T_ITEM_WND : _AGGUIHotfixBasicSubWnd
    {
        protected _AHotfixBaseShowAnimContainerWnd(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {

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

        public int showOpSerialize { get { return _m_iShowOpSerialize; } }

        public override void showWnd(Action _delegate)
        {
            base.showWnd(_delegate);
            //已经有效则不进行后续操作
            if (wnd != null && hotfixWnd != null && hotfixWnd.openItemShowAnim && !_m_bIsShowFirstRefresh)
            {
                _m_iShowOpSerialize++;
                _m_bIsShowFirstRefresh = true;
                _m_showItemsCount = 0;
                _m_lastShowItemTime = Time.time;
                // 解决已经加载item的container，第一帧闪的问题
                refreshAllItem(_wnd => {
                    if (_wnd != null)
                        _stopAnim(_wnd.wnd);
                });
                CommonTaskController.CommonActionAddNextFrameLaterTask(_onNextLateUpdate);
            }
        }

        public override void hideWnd(Action _delegate)
        {
            if (wnd != null && hotfixWnd != null && hotfixWnd.openItemShowAnim)
                _m_iShowOpSerialize++;
            base.hideWnd(_delegate);
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
            if (wnd != null && hotfixWnd != null && hotfixWnd.openItemShowAnim && _m_bIsShowFirstRefresh)
                _stopAnim(_itemWnd.wnd);
        }

        /// <summary>
        /// 下一帧的时候播放显示动画，并根据显示时间间隔播放动画
        /// </summary>
        private void _onNextLateUpdate()
        {
            refreshAllItem(_wnd => { _playItemRefreshAnim(_wnd, _m_bIsShowFirstRefresh); });
            _m_bIsShowFirstRefresh = false;
        }

        protected void _playItemRefreshAnim(_T_ITEM_WND _itemWnd, bool _isFirstShowRefresh)
        {
            if (wnd == null || _itemWnd == null || hotfixWnd == null || !hotfixWnd.openItemShowAnim)
                return;

            // 是否是showWnd的第一次刷新
            if (!_isFirstShowRefresh)
            {
                _setAnimToEnd(_itemWnd.wnd);
            }
            else
            {
                _stopAnim(_itemWnd.wnd);
                _m_fLastRefreshTime = Time.time;
                float passTime = _m_fLastRefreshTime - _m_lastShowItemTime;
                // 是否是首个播放动画，是则直接播放动画，否则延迟播放
                if (passTime >= hotfixWnd.firstItemShowAnimDelay)
                {
                    _playAnim(_itemWnd.wnd);
                    _m_lastShowItemTime = _m_fLastRefreshTime;
                }
                else
                {
                    // 在延迟显示的item计数，用于计算延迟时间
                    _m_showItemsCount++;
                    float delay = _m_showItemsCount * hotfixWnd.firstItemShowAnimDelay - passTime;
                    /** 显示操作序列号 */
                    int taskOp = showOpSerialize;

                    CommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        /** 非本地显示操作的task不执行 */
                        if (showOpSerialize != taskOp)
                            return;

                        _playAnim(_itemWnd.wnd);
                        _m_lastShowItemTime = Time.time;
                        _m_showItemsCount--;
                    }, delay);
                }
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
    }
}