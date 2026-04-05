using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndSubScrollNum : _ATALBasicUISubWnd<GGUIMonoSubScrollNum>
    {
        //位数列表
        private List<GGUIWndSubScrollNumContainer> _m_lDigitList;
        //全部滚动结束事件
        private Action _m_aOnScrollDone;

        /// <summary>
        /// 是否正在滚动
        /// </summary>
        public bool isScrolling
        {
            get
            {
                if (_m_lDigitList == null || _m_lDigitList.Count <= 0 || _m_lDigitList[0] == null)
                    return false;

                return _m_lDigitList[0].isScrolling;
            }
        }

        public GGUIWndSubScrollNum(GGUIMonoSubScrollNum _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_lDigitList != null)
            {
                for (int i = 0; i < _m_lDigitList.Count; i++)
                {
                    if(_m_lDigitList[i]!= null)
                        _m_lDigitList[i].hideWnd();
                }
            }

            _m_aOnScrollDone = null;
        }

        protected override void _onReset()
        {
            if (_m_lDigitList != null)
            {
                for (int i = 0; i < _m_lDigitList.Count; i++)
                {
                    if (_m_lDigitList[i] != null)
                        _m_lDigitList[i].resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lDigitList != null)
            {
                for (int i = 0; i < _m_lDigitList.Count; i++)
                {
                    if (_m_lDigitList[i] != null)
                        _m_lDigitList[i].discard();
                }
                _m_lDigitList.Clear();
                _m_lDigitList = null;
            }

            _m_aOnScrollDone = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lDigitList = new List<GGUIWndSubScrollNumContainer>();
            if (wnd.monoDigitList != null)
            {
                for (int i = 0; i < wnd.monoDigitList.Count; i++)
                {
                    GGUIWndSubScrollNumContainer tempGrid = new GGUIWndSubScrollNumContainer(wnd.monoDigitList[i]);
                    tempGrid.onCarryBit += _onCarryBit;
                    if (i == 0)
                        tempGrid.onScrollDone += _onScrollDone;
                    _m_lDigitList.Add(tempGrid);
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_num"></param>
        public void initNum(long _num)
        {
            if (wnd == null)
                return;

            long numLength = _num.ToString().Length;
            for (int i = 0; i < _m_lDigitList.Count; i++)
            {
                if (_m_lDigitList[i] != null)
                {
                    _m_lDigitList[i].setShowNum(_num, i);
                    if (i < numLength || wnd.needShowAll)
                        _m_lDigitList[i].showWnd();
                    else
                        _m_lDigitList[i].hideWnd();
                }
            }
        }

        /// <summary>
        /// 设置新数字并开始滚动
        /// </summary>
        /// <param name="_addNum"></param>
        /// <param name="_checkCanContinue"></param>
        /// <param name="_onAddPerNum"></param>
        public void setAddNumAndScroll(long _addNum, Func<bool> _checkCanContinue, Action _onAddPerNum, Action _onAllDone)
        {
            if (_m_lDigitList == null || _m_lDigitList.Count <= 0 || _m_lDigitList[0] == null)
            {
                _onAllDone?.Invoke();
                return;
            }
            _m_aOnScrollDone = _onAllDone;
            _m_lDigitList[0].setAddNumAndScroll(_addNum, _checkCanContinue, _onAddPerNum);
        }

        //滚动到需要进位
        private void _onCarryBit(int _nextDigitIndex, long _addNum, Func<bool> _checkCanContinue)
        {
            if (_m_lDigitList == null || _m_lDigitList.Count <= _nextDigitIndex || _m_lDigitList[_nextDigitIndex] == null)
                return;

            _m_lDigitList[_nextDigitIndex].showWnd();
            _m_lDigitList[_nextDigitIndex].setAddNumAndScroll(_addNum, _checkCanContinue, null);
        }

        //全部滚动结束
        private void _onScrollDone()
        {
            _m_aOnScrollDone?.Invoke();
        }
    }
}