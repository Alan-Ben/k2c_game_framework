using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class ScrollRectMoveTask : _IALBaseMonoTask
    {
        private readonly _IScrollRectMoveTask _m_scrollRectOwner;
        private readonly bool _m_vertical;
        private readonly float _m_startNormalizedPosition;
        private readonly float _m_targetNormalizedPosition;
        private readonly float _m_durationTime;
        private readonly EaseType _m_easeType;
        private readonly int _m_serialize;
        private readonly Action _m_complete;

        private float _m_timeCounter;
        
        
        public ScrollRectMoveTask(_IScrollRectMoveTask _scrollRectOwner, bool _vertical, float _targetNormalizedPosition, float _durationTime, EaseType _easeType = EaseType.InOutQuad, Action _complete = null)
        {
            _m_scrollRectOwner = _scrollRectOwner;
            if (_scrollRectOwner != null && _scrollRectOwner.scrollRect != null)
            {
                _m_serialize = _scrollRectOwner.serialize;
                _m_startNormalizedPosition = _vertical ? _scrollRectOwner.scrollRect.verticalNormalizedPosition : _scrollRectOwner.scrollRect.horizontalNormalizedPosition;
            }

            _m_vertical = _vertical;
            _m_targetNormalizedPosition = _targetNormalizedPosition;
            _m_durationTime = _durationTime;
            _m_easeType = _easeType;
            _m_complete = _complete;
            
            _m_timeCounter = 0f;
        }
        
        public void deal()
        {
            if (_m_scrollRectOwner == null ||
                _m_scrollRectOwner.scrollRect == null ||
                _m_scrollRectOwner.serialize != _m_serialize)
            {
                _m_complete?.Invoke();
                return;
            }

            _m_timeCounter += Time.deltaTime;
            if (_m_timeCounter >= _m_durationTime)
            {
                if (_m_vertical)
                    _m_scrollRectOwner.scrollRect.verticalNormalizedPosition = _m_targetNormalizedPosition;
                else
                    _m_scrollRectOwner.scrollRect.horizontalNormalizedPosition = _m_targetNormalizedPosition;
                _m_complete?.Invoke();
                return;
            }
            
            if (_m_vertical)
                _m_scrollRectOwner.scrollRect.verticalNormalizedPosition = Mathf.Lerp(_m_startNormalizedPosition, _m_targetNormalizedPosition, _m_easeType.evaluate(_m_timeCounter, _m_durationTime));
            else
                _m_scrollRectOwner.scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_m_startNormalizedPosition, _m_targetNormalizedPosition, _m_easeType.evaluate(_m_timeCounter, _m_durationTime));
            
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
    
    public interface _IScrollRectMoveTask
    {
        ScrollRect scrollRect { get; }
        int serialize { get; }
    }
}