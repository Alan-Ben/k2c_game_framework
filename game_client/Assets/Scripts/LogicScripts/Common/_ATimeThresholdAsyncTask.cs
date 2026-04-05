using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个带有时间阈值的异步任务
    /// </summary>
    /// <remarks>
    /// 你可以设定一个时间阈值，超过这个时间阈值，内部才会真正重新执行异步任务，否则只会认为还不需要再次执行异步任务，会把回调并入上次的任务中。
    /// </remarks>
    public abstract class _ATimeThresholdAsyncTask
    {
        // 超过这个时间阈值，就需要重新执行 realDeal ，否则不会执行 realDeal ，回调也将并入上次的 deal 中
        private float _m_timeThresholdSeconds;

        // 上次执行 deal 的时间和完成 action
        private float _m_lastDealTime;
        private bool _m_lastDealDone;
        private Action _m_lastDealDoneAction;
        private int _m_dealSerialize;

        protected _ATimeThresholdAsyncTask()
        {
            _m_timeThresholdSeconds = -1;
            
            _m_lastDealTime = -1;
            _m_lastDealDone = false;
            _m_lastDealDoneAction = null;
        }
        protected _ATimeThresholdAsyncTask(float _mTimeThresholdSeconds)
        {
            _m_timeThresholdSeconds = _mTimeThresholdSeconds;
            
            _m_lastDealTime = -1;
            _m_lastDealDone = false;
            _m_lastDealDoneAction = null;
        }
        
        /// <summary>
        /// 时间阈值
        /// </summary>
        /// <remarks>
        /// 超过这个时间阈值，就需要重新执行 realDeal ，否则不会执行 realDeal ，回调也将并入上次的 deal 中
        /// </remarks>
        public float timeThresholdSeconds { get { return _m_timeThresholdSeconds; } set { _m_timeThresholdSeconds = value; } }
        
        /// <summary>
        /// 执行这个异步任务
        /// </summary>
        public void deal(Action _done)
        {
            float now = Time.realtimeSinceStartup;
            if (_m_lastDealTime <= 0 || now - _m_lastDealTime > _m_timeThresholdSeconds)
            {
                // 如果上次执行的时间不存在，或是相差大于设置的时间阈值，就执行异步任务
                
                // 设置准备参数
                _m_lastDealTime = now;
                _m_lastDealDone = false;
                _m_lastDealDoneAction += _done;
                int serialize = _m_dealSerialize = ALSerializeOpMgr.next();
                
                // 执行任务
                _realDeal(() =>
                {
                    // 如果序列号不同，就废弃掉这次执行结果，已最后一次执行的结果为准
                    if (serialize != _m_dealSerialize)
                        return;
                    
                    // 设置任务完成
                    _m_lastDealDone = true;
                    
                    Action doneActions = _m_lastDealDoneAction;
                    _m_lastDealDoneAction = null;
                    doneActions?.Invoke();
                });
            }
            else
            {
                // 如果还在时间阈值内，就认为上次执行结果还有效，根据 realDeal 是否完成来处理
                if (_m_lastDealDone)
                    _done?.Invoke();
                else
                    _m_lastDealDoneAction += _done;
            }
        }

        protected abstract void _realDeal(Action _complete);
    }
}