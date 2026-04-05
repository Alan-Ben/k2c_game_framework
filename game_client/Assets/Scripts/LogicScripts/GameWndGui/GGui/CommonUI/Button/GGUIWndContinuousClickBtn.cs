using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 连续点击触发特殊处理按钮
    /// </summary>
    public class GGUIWndContinuousClickBtn : _ANPGGUIBasicSubWnd<GGUIMonoContinuousClickBtn>
    {
        [NotNull] private Queue<long> _m_recordMillTimeQueue = new Queue<long>();
        private Action _m_aNormalClick;
        private Action _m_aSpecialClick;

        /// <summary>
        /// 正常点击事件
        /// </summary>
        public Action onNormalClick
        {
            get { return _m_aNormalClick; }
            set { _m_aNormalClick = value; }
        }

        /// <summary>
        /// 规定时间内多次点击后事件
        /// </summary>
        public Action onSpecialClick
        {
            get { return _m_aSpecialClick; }
            set { _m_aSpecialClick = value; }
        }

        public GGUIWndContinuousClickBtn(GGUIMonoContinuousClickBtn _wnd) : base(_wnd)
        {
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
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_recordMillTimeQueue.Clear();

            _m_aNormalClick = null;
            _m_aSpecialClick = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_aNormalClick = null;
            _m_aSpecialClick = null;
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        //点击事件
        private void _onClick(GameObject _go)
        {
            if (wnd == null)
                return;

            if (wnd.durationTimeSec > 0 && wnd.continuousClickCount > 1)
            {
                if (_m_recordMillTimeQueue.Count == wnd.continuousClickCount - 1)
                {
                    //来到第目标次点击，判断是否在时间区间内
                    long firstMillTime = _m_recordMillTimeQueue.Dequeue();
                    long curMillTime = ALCommon.getNowTimeMill();
                    float durationTime = (curMillTime / 1000f) - (firstMillTime / 1000f);

                    if (durationTime <= wnd.durationTimeSec)
                    {
                        //在区间内，触发特殊点击时间，并清空时间列表
                        _m_recordMillTimeQueue.Clear();
                        _m_aSpecialClick?.Invoke();
                    }
                    else
                    {
                        //不在区间内，加入这次时间
                        _m_recordMillTimeQueue.Enqueue(curMillTime);
                    }
                }
                else
                {
                    long curMillTime = ALCommon.getNowTimeMill();
                    _m_recordMillTimeQueue.Enqueue(curMillTime);
                    _m_aNormalClick?.Invoke();
                }
            }
            else
                _m_aNormalClick?.Invoke();
        }
    }
}