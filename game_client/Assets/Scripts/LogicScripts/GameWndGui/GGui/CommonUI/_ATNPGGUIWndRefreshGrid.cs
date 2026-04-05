using ALPackage;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    public abstract class _ATNPGGUIWndRefreshGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATNPGGUIWndShowAnimGrid<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _ATNPGGUIMonoRefreshGrid<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        private NPGGUIWndScrollRefreshMonitor _m_wRefreshMonitor;
        private long _m_lRefreshSerial;

        protected _ATNPGGUIWndRefreshGrid(_T_CONTAINER_MONO _mono) : base(_mono)
        {

        }

        public NPGGUIWndScrollRefreshMonitor refreshMonitor { get { return _m_wRefreshMonitor; } }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wRefreshMonitor?.discard();
            _m_wRefreshMonitor = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRefreshMonitor != null)
            {
                _m_wRefreshMonitor = new NPGGUIWndScrollRefreshMonitor(wnd.monoRefreshMonitor, _dealRefresh);
            }
        }

        /// <summary>
        /// 执行一次刷新操作
        /// </summary>
        protected void _dealRefresh()
        {
            //更改序列号
            long serialize = _m_lRefreshSerial = ALSerializeOpMgr.next();

            //无论手动调用还是滚动条自身触发，都标记为不可刷新状态
            _m_wRefreshMonitor?.setEnableCheck(false);

            //记录更新前的总长宽
            float lastHeight = allHeight;
            float lastWidth = allWidth;

            //执行刷新
            _refreshData(refreshDone);

            //刷新完成
            void refreshDone(bool _needRefresh)
            {
                //序列号不一致，不处理
                if (serialize != _m_lRefreshSerial)
                    return;

                _m_wRefreshMonitor?.setEnableCheck(_needRefresh);
                checkWndSize();
                _fixViewport(lastHeight, lastWidth);
            }
        }

        /// <summary>
        /// 执行刷新
        /// </summary>
        /// <param name="_complete">完成回调(是否还需要检测刷新)</param>
        protected abstract void _refreshData(Action<bool> _complete);

        /// <summary>
        /// 刷新完成之后，校准位置的操作；
        /// 默认在上/左拉刷新并添加item之后，会恢复到原来显示的位置；
        /// 如果不是增量刷新或者有其他需求，可以重载此方法
        /// </summary>
        /// <param name="_lastHeight">刷新前的总高度allHeight</param>
        /// <param name="_lastWidth">刷新前的总宽度allWidth</param>
        protected virtual void _fixViewport(float _lastHeight, float _lastWidth)
        {
            if (wnd == null || wnd.gridAreaUIObj == null || wnd.gridAreaMaskObj == null || wnd.monoRefreshMonitor == null)
                return;

            //一般情况下，从上/左方向加入item才需要做修正
            switch (wnd.monoRefreshMonitor.directType) 
            {
                case ENPScrollCheckDirectType.UP:
                    {
                        if (wnd.gridAreaUIObj.rect.height <= wnd.gridAreaMaskObj.rect.height)
                            return;

                        wnd.gridAreaUIObj.localPosition += new Vector3(0, allHeight - _lastHeight, 0);
                        _stopScrollDrag();
                    }
                    break;

                case ENPScrollCheckDirectType.LEFT:
                    {
                        if (wnd.gridAreaUIObj.rect.width <= wnd.gridAreaMaskObj.rect.width)
                            return;

                        wnd.gridAreaUIObj.localPosition -= new Vector3(allWidth - _lastWidth, 0, 0);
                        _stopScrollDrag();
                    }
                    break;
            }
        }

        /// <summary>
        /// 停止滚动条拖动，防止坐标修改和用户输入冲突
        /// </summary>
        protected void _stopScrollDrag()
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            // todo:找一个下移之后不影响操作手感的方法
            // 如果不停止拖动会有问题
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            // wnd.scrollRect.StopMovement();
            wnd.scrollRect.OnEndDrag(pointerEventData);
        }
    }
}
