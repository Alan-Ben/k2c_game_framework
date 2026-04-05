using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 监视滚动条并触发刷新事件的窗体
    /// </summary>
    public class NPGGUIWndScrollRefreshMonitor : _ATALBasicUISubWnd<NPGGUIMonoScrollRefreshMonitor>
    {
        private bool _m_bEnableCheck = false;//是否开启检测
        private RectTransform _m_rtScrollRectTrans;//滚动条的RectTransform，减少获取组件次数用
        private Action _m_aRefreshDelegate;//刷新委托，触发后会执行一次该事件

        public NPGGUIWndScrollRefreshMonitor(NPGGUIMonoScrollRefreshMonitor _wnd, Action _refreshDelegate) : base(_wnd)
        {
            _m_aRefreshDelegate = _refreshDelegate;
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_bEnableCheck = false;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (wnd.scrollRect != null)
            {
                wnd.scrollRect.onValueChanged.RemoveListener(_onScrollRectValueChg);
            }
            _m_rtScrollRectTrans = null;

            _m_bEnableCheck = false;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.scrollRect != null)
            {
                wnd.scrollRect.onValueChanged.AddListener(_onScrollRectValueChg);
                _m_rtScrollRectTrans = wnd.scrollRect.GetComponent<RectTransform>();
            }
        }

        private void _onScrollRectValueChg(Vector2 _value)
        {
            //不检测不处理
            if (!_m_bEnableCheck
                || wnd == null || wnd.scrollRect == null || wnd.scrollRect.content == null || _m_rtScrollRectTrans == null)
                return;

            //判断是否需要刷新，判断依据为“往对应方向拖动”&“超过边界一定距离”
            bool needRefresh = false;
            switch (wnd.directType)
            {
                case ENPScrollCheckDirectType.UP:
                    needRefresh = wnd.scrollRect.velocity.y < 0 && wnd.scrollRect.content.anchoredPosition.y <= -wnd.checkDistance;
                    break;

                case ENPScrollCheckDirectType.DOWN:
                    needRefresh = wnd.scrollRect.velocity.y > 0 && wnd.scrollRect.content.anchoredPosition.y >= wnd.scrollRect.content.rect.height - _m_rtScrollRectTrans.rect.height + wnd.checkDistance;
                    break;

                case ENPScrollCheckDirectType.LEFT:
                    needRefresh = wnd.scrollRect.velocity.x > 0 && wnd.scrollRect.content.anchoredPosition.x >= wnd.checkDistance;
                    break;

                case ENPScrollCheckDirectType.RIGHT:
                    needRefresh = wnd.scrollRect.velocity.x < 0 && -wnd.scrollRect.content.anchoredPosition.x >= wnd.scrollRect.content.rect.width - _m_rtScrollRectTrans.rect.width + wnd.checkDistance;
                    break;
            }

            //触发对应事件
            if (needRefresh)
            {
                _m_bEnableCheck = false;
                _m_aRefreshDelegate?.Invoke();
            }
        }


        #region 外部调用

        /// <summary>
        /// 设置是否开启检测
        /// </summary>
        /// <param name="_enable"></param>
        public void setEnableCheck(bool _enable)
        {
            _m_bEnableCheck = _enable;
        }

        #endregion
    }
}
