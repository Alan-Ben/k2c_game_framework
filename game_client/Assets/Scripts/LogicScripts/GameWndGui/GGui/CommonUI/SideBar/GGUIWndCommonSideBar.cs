using System;
using ALPackage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用侧边栏
    /// </summary>
    public class GGUIWndCommonSideBar : _ANPGGUIBasicSubWnd<GGUIMonoCommonSideBar>
    {
        private ESideBarType _m_eSideBarType;//侧边栏类型
        private int _m_iShowActiveCount;//展示个数
        private bool _m_isSpread;//侧边栏是否展开
        private Vector2 _m_doTweenSize = Vector2.zero;//侧边栏大小
        private Tweener _m_tweener;//处理变化
        private ALCommonEnableTaskController _m_tickTask;//检查数量的每帧任务

        public GGUIWndCommonSideBar(GGUIMonoCommonSideBar _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_tweener != null)
                _m_tweener.Kill();

            _m_tickTask.setDisable();
        }

        protected override void _onReset()
        {
            if (_m_tweener != null)
                _m_tweener.Kill();
        }

        protected override void _onDiscard()
        {
            if (_m_tweener != null)
                _m_tweener.Kill();

            _m_tickTask.setDisable();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSpread, _onBtnSpreadClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnSpread, _onBtnSpreadClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_type">类型</param>
        /// <param name="_isOpenTask">是否需要开启任务，用于监听子物体数量变化</param>
        public void setInfo(ESideBarType _type, bool _isOpenTask = false)
        {
            _m_eSideBarType = _type;
            _m_iShowActiveCount = _calculateActiveCount();
            _m_isSpread = AccountSettingMgr.instance.accountSetting.isSideBarSpread(_m_eSideBarType);

            //是否打开任务
            if (_isOpenTask)
                _m_tickTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_refreshSidebarElement);
            else
                _m_tickTask.setDisable();

            _refreshWnd();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_type">类型</param>
        /// <param name="_isSpread">是否展开</param>
        /// <param name="_isOpenTask">是否需要开启任务，用于监听子物体数量变化</param>
        public void setInfo(ESideBarType _type, bool _isSpread, bool _isOpenTask = false)
        {
            _m_eSideBarType = _type;
            _m_iShowActiveCount = _calculateActiveCount();
            _m_isSpread = _isSpread;

            //记录是否展开
            if (_m_isSpread)
                AccountSettingMgr.instance.accountSetting.setSpreadSideBarType(_m_eSideBarType);
            else
                AccountSettingMgr.instance.accountSetting.removeSpreadSideBarType(_m_eSideBarType);

            //是否打开任务
            _m_tickTask.setDisable();
            if (_isOpenTask)
                _m_tickTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_refreshSidebarElement);

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshSpreadState();
            _doChgSize(0);
        }

        //刷新展开状态
        private void _refreshSpreadState()
        {
            if (wnd == null || wnd.sidebarBg == null)
                return;

            if (_m_isSpread)
            {
                ALUGUICommon.setUIObjScale(wnd.onSpreadShow, 1f);
                ALUGUICommon.setUIObjScale(wnd.onPackUpShow, 0f);
            }
            else
            {
                ALUGUICommon.setUIObjScale(wnd.onSpreadShow, 0f);
                ALUGUICommon.setUIObjScale(wnd.onPackUpShow, 1f);
            }
        }

        //播放展开收缩动画
        private void _doChgSize(float _time)
        {
            if (wnd == null || wnd.sidebarBg == null)
                return;

            Vector2 targetSize = Vector2.zero;
            float calValue = _calculateLength(_m_isSpread ? _m_iShowActiveCount : 0);
            _setCanScroll(calValue >= wnd.bgLimit);

            switch (wnd.axisType)
            {
                case GridLayoutGroup.Axis.Horizontal:
                    targetSize = new Vector2(calValue, wnd.sidebarBg.sizeDelta.y);
                    break;
                case GridLayoutGroup.Axis.Vertical:
                    targetSize = new Vector2(wnd.sidebarBg.sizeDelta.x, calValue);
                    break;
            }

            _doTweenSize(targetSize, _time);
        }

        //缩放动画
        private void _doTweenSize(Vector2 _targetSize, float _chgTime, Action _onComplete = null)
        {
            if (null == wnd || rectTransform == null)
                return;

            _m_doTweenSize = rectTransform.sizeDelta;
            _m_tweener = DOTween.To(() => _m_doTweenSize, _v =>
            {
                _m_doTweenSize = _v;
                // 设置窗口大小
                ALUGUICommon.setUIObjSize(wnd.sidebarBg, _m_doTweenSize);
            }, _targetSize, _chgTime).SetEase(wnd.animationCurve).OnComplete(() =>
            {
                // 设置窗口大小
                ALUGUICommon.setUIObjSize(wnd.sidebarBg, _targetSize);
                if (_onComplete != null)
                    _onComplete();
            });
        }

        //计算背景长度
        private float _calculateLength(int _activeCount)
        {
            if (wnd == null || _activeCount < 0)
                return 0;

            if (_activeCount > 0)
            {
                float height = wnd.btnHeight * _activeCount + wnd.btnSpace * (_activeCount - 1) + wnd.shrinkHeight;
                return Math.Min(height, wnd.bgLimit);
            }
            else
                return Math.Min(wnd.shrinkHeight, wnd.bgLimit);
        }

        //计算显示的的子物体数量
        private int _calculateActiveCount()
        {
            if (wnd == null || wnd.sidebarElements == null)
                return 0;
            int activeCount = 0;
            for (int i = 0; i < wnd.sidebarElements.Count; i++)
            {
                if (wnd.sidebarElements[i] == null)
                    continue;
                if (wnd.sidebarElements[i].activeSelf == true)
                    activeCount++;
            }
            return activeCount;
        }

        //设置是否可以拖动
        private void _setCanScroll(bool _isTrue)
        {
            if (wnd == null || wnd.sidebarScrollRect == null)
                return;

            wnd.sidebarScrollRect.horizontal = false;
            wnd.sidebarScrollRect.vertical = false;

            if (_isTrue)
            {
                switch (wnd.axisType)
                {
                    case GridLayoutGroup.Axis.Horizontal:
                        wnd.sidebarScrollRect.horizontal = true;
                        break;
                    case GridLayoutGroup.Axis.Vertical:
                        wnd.sidebarScrollRect.vertical = true;
                        break;
                    default:
#if UNITY_EDITOR
                        Debug.LogError($"Error: {GetType()} wnd.axisType is {wnd.axisType}");
#endif
                        break;
                }
            }
        }

        //刷新子物体数量
        private void _refreshSidebarElement()
        {
            if (wnd == null || wnd.sidebarElements == null)
            {
                _m_tickTask.setDisable();
                return;
            }
            int activeCount = _calculateActiveCount();
            if (activeCount != _m_iShowActiveCount)
            {
                _m_iShowActiveCount = activeCount;
                _doChgSize(0);
            }
        }

        //侧边栏展开按钮点击事件
        private void _onBtnSpreadClick(GameObject _btn)
        {
            _m_isSpread = !_m_isSpread;
            if (wnd != null)
            {
                if (_m_isSpread)
                {
                    ALUGUICommon.setUIObjScale(wnd.onSpreadShow, 1f);
                    ALUGUICommon.setUIObjScale(wnd.onPackUpShow, 0f);

                    //根据配置设置列表默认位置
                    if (wnd.sidebarScrollRect != null)
                    {
                        switch (wnd.defaultSpreadState)
                        {
                            case EScrollRectMoveType.LEFT:
                                wnd.sidebarScrollRect.horizontalNormalizedPosition = 0;
                                break;
                            case EScrollRectMoveType.RIGHT:
                                wnd.sidebarScrollRect.horizontalNormalizedPosition = 1;
                                break;
                            case EScrollRectMoveType.TOP:
                                wnd.sidebarScrollRect.verticalNormalizedPosition = 1;
                                break;
                            case EScrollRectMoveType.BOTTOM:
                                wnd.sidebarScrollRect.verticalNormalizedPosition = 0;
                                break;
                        }
                    }
                }
                else
                {
                    ALUGUICommon.setUIObjScale(wnd.onSpreadShow, 0f);
                    ALUGUICommon.setUIObjScale(wnd.onPackUpShow, 1f);
                }
                _doChgSize(wnd.chgSizeTime);
            }

            //记录是否展开
            if(_m_isSpread)
                AccountSettingMgr.instance.accountSetting.setSpreadSideBarType(_m_eSideBarType);
            else
                AccountSettingMgr.instance.accountSetting.removeSpreadSideBarType(_m_eSideBarType);
        }
    }
}