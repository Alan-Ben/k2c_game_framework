
using System;
using UnityEngine;
using UnityEngine.UI;

using ALPackage;

namespace GOE
{
    public interface _IScrollerSmoothMovable
    {
        ScrollRect scrollRect { get; }
        long serialize { get; }
    }

    public class ScrollerSmoothMoveTaskHorizontal : _IALBaseMonoTask
    {
        // 相对缓冲
        private float _m_fSmoothVelocity;

        // 缓冲时间，越大越慢，越小越快
        private float _m_fSmoothTime;

        // 目标的值
        private float _m_fTargetValue;

        // 当前变化的值
        private float _m_fCurrentValue;

        // 操作数
        private long _m_lSerialize;

        private _IScrollerSmoothMovable _m_wndGridCtrl;

        private NPScrollRect _m_monoMGScrollRect;

        // 判断移动结束的距离值，会用这个值去对比当前点和目标点。
        private const float _m_fOverCheckDistance = 0.0001f;

        private Action _m_complete;

        public ScrollerSmoothMoveTaskHorizontal(_IScrollerSmoothMovable _gridCtrl, float _targetHorizontalRate, float _smoothTime = 0.25f, Action _complete = null)
        {
            // 防止组件移动导致滑动失效
            _gridCtrl.scrollRect.StopMovement();
            //_gridCtrl.scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(_gridCtrl.scrollRect.horizontalNormalizedPosition);

            _m_fSmoothVelocity = 0;
            _m_fSmoothTime = _smoothTime;
            _m_fTargetValue = _targetHorizontalRate;
            _m_complete = _complete;
            _m_fCurrentValue = _gridCtrl.scrollRect.horizontalNormalizedPosition;
            _m_lSerialize = _gridCtrl.serialize;
            _m_wndGridCtrl = _gridCtrl;
            _m_monoMGScrollRect = _gridCtrl.scrollRect as NPScrollRect;
        }

        public void deal()
        {
            if (null == _m_wndGridCtrl
                || null == _m_wndGridCtrl.scrollRect
                || _m_lSerialize != _m_wndGridCtrl.serialize
                || !_approximately(_m_fCurrentValue, _m_wndGridCtrl.scrollRect.horizontalNormalizedPosition)
                || (_m_monoMGScrollRect != null && _m_monoMGScrollRect.isDraging))
            {
                _m_complete?.Invoke();
                return;
            }

            if ((_m_fTargetValue == 0 && _m_fCurrentValue < 0) ||
                (_m_fTargetValue == 1 && _m_fCurrentValue > 1))
            {
                _m_complete?.Invoke();
                return;
            }

            if (!_isOver())
            {
                _m_fCurrentValue = Mathf.SmoothDamp(_m_fCurrentValue, _m_fTargetValue, ref _m_fSmoothVelocity, _m_fSmoothTime);
                _m_wndGridCtrl.scrollRect.horizontalNormalizedPosition = _m_fCurrentValue;

                // 没结束的话接着执行
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
            else
            {
                _m_wndGridCtrl.scrollRect.horizontalNormalizedPosition = _m_fTargetValue;
                _m_complete?.Invoke();
            }
        }

        private bool _isOver()
        {
            float delta = Mathf.Abs(_m_fCurrentValue - _m_fTargetValue);
            if (delta > _m_fOverCheckDistance) return false;

            return true;
        }

        private bool _approximately(float _value1, float _value2)
        {
            return Mathf.Abs(_value1 - _value2) < _m_fOverCheckDistance;
        }
    }
    
    public class ScrollerEaseMoveTaskHorizontal : _IALBaseMonoTask
    {
        private float _m_curTime;
        private float _m_totalTime;
        private EaseType _m_easeType;
        private float _m_startValue;
        private float _m_targetValue;
        private Action _m_complete;
        private long _m_lSerialize;
        private _IScrollerSmoothMovable _m_gridCtrl;
        private NPScrollRect _m_scrollRect;

        public ScrollerEaseMoveTaskHorizontal(_IScrollerSmoothMovable _gridCtrl, float _targetHorizontalRate, EaseType _easeType, float _totalTime = 0.3f, Action _complete = null)
        {
            _gridCtrl.scrollRect.StopMovement();
            
            _m_easeType = _easeType;
            _m_curTime = 0;
            _m_totalTime = _totalTime;
            _m_targetValue = _targetHorizontalRate;
            _m_startValue = _gridCtrl.scrollRect.horizontalNormalizedPosition;
            _m_lSerialize = _gridCtrl.serialize;
            _m_gridCtrl = _gridCtrl;
            _m_scrollRect = _gridCtrl.scrollRect as NPScrollRect;
            _m_complete = _complete;
        }

        public void deal()
        {
            if (_m_lSerialize != _m_gridCtrl.serialize || (_m_scrollRect != null && _m_scrollRect.isDraging))
            {
                _m_complete?.Invoke();
                return;
            }

            if (_m_curTime < _m_totalTime)
            {
                float value = Mathf.Lerp(_m_startValue, _m_targetValue, _m_easeType.evaluate(_m_curTime, _m_totalTime));
                _m_gridCtrl.scrollRect.horizontalNormalizedPosition = value;
                _m_curTime += Time.deltaTime;

                // 没结束的话接着执行
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
            else
            {
                _m_gridCtrl.scrollRect.horizontalNormalizedPosition = _m_targetValue;
                _m_complete?.Invoke();
            }
        }
    }
}