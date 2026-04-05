using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 展示滚动文本列表
    /// </summary>
    public class GGUIWndSubScrollNumContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoSubScrollNumContainerItem, GGUIMonoSubScrollNumContainer, GGUIWndSubScrollNumContainerItem>
    {
        //窗口容器
        protected List<GGUIWndSubScrollNumContainerItem> _m_lItemList;
        //当前展示的数字
        private long _m_lCurShowNum;
        private long _m_lTargetNum;
        //在第几位
        private int _m_digitIndex;
        //数字滚动控制
        private Tweener _m_tweener;
        //数字更新控制
        private Tweener _m_numTweener;
        //需要进位事件
        private Action<int, long, Func<bool>> _m_aOnCarryBit;
        //滚动结束事件
        private Action _m_aOnScrollDone;
        //操作序列号
        private long _m_lOpSerialize;
        //是否正在滚动中
        private bool _m_bIsScrolling;
        //完成步骤处理
        [NotNull]private ALStepCounter _m_stepCounter;

        /// <summary>
        /// 是否正在滚动中
        /// </summary>
        public bool isScrolling
        {
            get { return _m_bIsScrolling; }
        }

        /// <summary>
        /// 需要进位事件
        /// </summary>
        public Action<int, long, Func<bool>> onCarryBit
        {
            get { return _m_aOnCarryBit; }
            set { _m_aOnCarryBit = value; }
        }

        /// <summary>
        /// 滚动结束事件
        /// </summary>
        public Action onScrollDone
        {
            get { return _m_aOnScrollDone; }
            set { _m_aOnScrollDone = value; }
        }

        public GGUIWndSubScrollNumContainer(GGUIMonoSubScrollNumContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndSubScrollNumContainerItem _createItemWnd(GGUIMonoSubScrollNumContainerItem _itemMono)
        {
            GGUIWndSubScrollNumContainerItem item = new GGUIWndSubScrollNumContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            _m_stepCounter = new ALStepCounter();

            List<string>  numList = new List<string>();
            numList.Add("0");
            numList.Add("1");
            numList.Add("2");
            numList.Add("3");
            numList.Add("4");
            numList.Add("5");
            numList.Add("6");
            numList.Add("7");
            numList.Add("8");
            numList.Add("9");
            numList.Add("0");

            _m_lItemList = new List<GGUIWndSubScrollNumContainerItem>();
            GGUIWndSubScrollNumContainerItem itemWnd = null;
            for (int i = 0; i < numList.Count; i++)
            {
                itemWnd = addItemWnd();
                if (null == itemWnd)
                    continue;
                itemWnd.showWnd();
                itemWnd.setInfo(numList[i]);
                _m_lItemList.Add(itemWnd);
            }
        }

        /// <summary>
        /// 设置当前显示的数字
        /// </summary>
        /// <param name="_num">当前数字</param>
        /// <param name="_digitIndex">第几位数</param>
        public void setShowNum(long _num,int _digitIndex)
        {
            _m_digitIndex = _digitIndex;
            long divisor = _digitIndex == 0 ? 1 : (long)Math.Pow(10, _digitIndex);
            //当前位数上的数字
            _m_lCurShowNum = _num / divisor % 10;
            _m_lTargetNum = _m_lCurShowNum;
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                float target = _m_lCurShowNum * 0.1f;
                moveToVerticalRate(target);
            });
        }

        /// <summary>
        /// 设置增加数并开始滚动
        /// </summary>
        /// <param name="_addNum"></param>
        /// <param name="_checkCanContinue"></param>
        /// <param name="_onAddPerNum"></param>
        public void setAddNumAndScroll(long _addNum, Func<bool> _checkCanContinue, Action _onAddPerNum)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            _m_bIsScrolling = true;
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_lTargetNum = _m_lTargetNum + _addNum;
            Action onAddPerNum = _onAddPerNum;
            long serialize = _m_lOpSerialize;
            long realAddNum = _m_lTargetNum - _m_lCurShowNum;
            float curNormalized = wnd.scrollRect.verticalNormalizedPosition;
            if (curNormalized < 0.01f)
                curNormalized = 1;
            float targetNormalized = 1-(_m_lTargetNum / 10 + _m_lTargetNum % 10 * 0.1f);
            float scrollDuration = getDuration(realAddNum);

            //完成的回调
            Action onDone = () =>
            {
                if (serialize != _m_lOpSerialize)
                    return;
                
                _m_bIsScrolling = false;
                //当前位数上的数字
                _m_lCurShowNum = _m_lCurShowNum % 10;
                _m_lTargetNum = _m_lCurShowNum;
                //再重置下展示的位置
                float target = _m_lCurShowNum * 0.1f;
                moveToVerticalRate(target);

                _m_aOnScrollDone?.Invoke();
            };
            _m_stepCounter.resetAll();
            _m_stepCounter.chgTotalStepCount(2);
            _m_stepCounter.regAllDoneDelegate(onDone);

            //重置动画
            if (_m_tweener != null)
                _m_tweener.Kill();
            if (_m_numTweener != null)
                _m_numTweener.Kill();

            //开始数字滚动
            _m_tweener = DOTween.To(
                    value =>
                    {
                        float normalized = value;
                        if (value < 0)
                            normalized = 1 - (Math.Abs(value) % 1);

                        if (normalized <0.01f)
                            normalized = 1;
                        if (wnd != null && wnd.scrollRect != null)
                            wnd.scrollRect.verticalNormalizedPosition = normalized;
                    },
                    curNormalized,
                    targetNormalized,
                    scrollDuration)
                .OnUpdate(() =>
                {
                    if (_checkCanContinue != null && !_checkCanContinue())
                    {
                        _m_lOpSerialize = ALSerializeOpMgr.next();
                        _m_tweener?.Kill();
                    }
                })
                .SetEase(wnd.digitCurve)
                .OnComplete(_m_stepCounter.addDoneStepCount);

            //开始数字更新
            _m_numTweener = DOTween.To(
                    value =>
                    {
                        if (_checkCanContinue != null && !_checkCanContinue())
                            return;

                        long newValue = (long) Math.Ceiling(value);
                        if (newValue != _m_lCurShowNum)
                        {
                            long addValue = newValue - _m_lCurShowNum;
                            _checkCarryBit(_m_lCurShowNum, newValue, _checkCanContinue);
                            _m_lCurShowNum = newValue;
                            //每增加1执行一次回调
                            for (int i = 0; i < addValue; i++)
                                onAddPerNum?.Invoke();
                        }
                    },
                    _m_lCurShowNum,
                    _m_lTargetNum,
                    scrollDuration)
                .OnUpdate(() =>
                {
                    if (_checkCanContinue != null && !_checkCanContinue())
                    {
                        onAddPerNum = null;
                        _m_lOpSerialize = ALSerializeOpMgr.next();
                        _m_numTweener?.Kill();
                    }
                })
                .SetEase(wnd.digitCurve)
                .OnComplete(_m_stepCounter.addDoneStepCount);
        }

        //获取滚动时间
        private float getDuration(long _addNum)
        {
            if (wnd == null || wnd.digitDurationList == null)
                return 0;

            for (int i = 0; i < wnd.digitDurationList.Count; i++)
            {
                if (WCGLongRange.inRange(_addNum, wnd.digitDurationList[i].minNum, wnd.digitDurationList[i].maxNum))
                    return wnd.digitDurationList[i].duration;
            }

            return 0;
        }

        //检查是否进位了
        private void _checkCarryBit(long oldNum,long newNum, Func<bool> _checkCanContinue)
        {
            long oldNumInIndex = oldNum / 10;
            long newNumInIndex = newNum / 10;

            if(oldNumInIndex != newNumInIndex)
                _m_aOnCarryBit?.Invoke(_m_digitIndex + 1, newNumInIndex - oldNumInIndex, _checkCanContinue);
        }
    }
}