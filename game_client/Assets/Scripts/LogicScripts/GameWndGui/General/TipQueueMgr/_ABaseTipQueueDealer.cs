using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 提示队列信息
    /// </summary>
    public abstract class _ABaseTipQueueDealer
    {
        //是否已经开始处理
        private bool _m_bIsStarted;
        //当前处理的序列号
        private long _m_lDealSerialize;
        //是否已经完成
        private bool _m_bIsDone;
        //完成回调
        private Action _m_dDoneDelegate;


        public _ABaseTipQueueDealer()
        {
            _m_bIsStarted = false;
            _m_lDealSerialize = 0;
            _m_bIsDone = false;
            _m_dDoneDelegate = null;
        }

        /// <summary>
        /// 是否已经开始
        /// </summary>
        public bool isStarted { get { return _m_bIsStarted; } }
        /// <summary>
        /// 处理序列号
        /// </summary>
        public long dealSerialize { get { return _m_lDealSerialize; } }
        /// <summary>
        /// 是否替换处理
        /// </summary>
        public virtual bool isReplaceNew { get { return false; } }

        /// <summary>
        /// 开始本对象的处理逻辑
        /// </summary>
        public void startDealer(long _dealSerialize)
        {
            //判断是否已经开始，是则不处理，避免多次开启
            if (_m_bIsStarted)
                return;

            _m_bIsStarted = true;

            //设置处理序列号
            _m_lDealSerialize = _dealSerialize;

            //开始处理展示
            _onStart();
        }

        /// <summary>
        /// 开始处理合并展示
        /// </summary>
        /// <param name="_dealSerialize"></param>
        public void startMargeDealer(List<_ABaseTipQueueDealer> _dealerList, long _dealSerialize)
        {
            //判断是否已经开始，是则不处理，避免多次开启
            if (_m_bIsStarted)
                return;

            _m_bIsStarted = true;

            //设置处理序列号
            _m_lDealSerialize = _dealSerialize;

            //处理合并
            _dealMarge(_dealerList);
            //开始处理展示
            _onStart();
        }

        //设置处理完成
        public void setDealDone()
        {
            //调用子类处理逻辑
            _onDealDone();

            //设置完成
            _m_bIsDone = true;

            //处理回调
            if (null != _m_dDoneDelegate)
                _m_dDoneDelegate();
            _m_dDoneDelegate = null;
        }

        /// <summary>
        /// 注册完成的回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regDoneDelegate(Action _delegate)
        {
            //已经完成直接处理
            if (_m_bIsDone)
            {
                if (null != _m_dDoneDelegate)
                    _m_dDoneDelegate();
                _m_dDoneDelegate = null;

                return;
            }

            //注册回调
            if (null == _m_dDoneDelegate)
                _m_dDoneDelegate = _delegate;
            else
                _m_dDoneDelegate += _delegate;
        }

        /// <summary>
        /// 处理合并操作
        /// </summary>
        /// <param name="_dealerList"></param>
        protected virtual void _dealMarge(List<_ABaseTipQueueDealer> _dealerList)
        {
            Debug.LogError("注意！tip使用了合并展示，但未重写_dealMarge");
        }

        /// <summary>
        /// 队列类型
        /// </summary>
        public abstract ETipQueueType tipType { get; }
        /// <summary>
        /// 当队列里有多个相关类型tip时是否需要将展示的值合并，true的话需要重写_dealMarge
        /// </summary>
        public abstract bool needMarge { get; }
        /// <summary>
        /// 开始处理的事件函数，在本处理中需要调用setDealerDone才能继续下一个处理
        /// </summary>
        protected abstract void _onStart();
        /// <summary>
        /// 完成处理的事件处理函数
        /// </summary>
        protected abstract void _onDealDone();
    }
}