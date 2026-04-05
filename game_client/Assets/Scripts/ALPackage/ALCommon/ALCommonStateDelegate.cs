using ALBasicProtocolPack;
using System;

namespace ALPackage
{
    /// <summary>
    /// 通用的状态完成回调对象，当状态完成的时候，通过注册会直接调用回调
    /// 当未完成时会记录等待设置状态时进行回调调用
    /// </summary>
    public class ALCommonStateDelegate
    {
        //是否初始化完成
        private bool _m_bIsInited;
        //回调对象
        private Action _m_dDelegate;

        public ALCommonStateDelegate()
        {
            _m_bIsInited = false;
            _m_dDelegate = null;
        }

        public bool isInited { get { return _m_bIsInited; } }

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            //判断是否已经完成，已完成直接设置回调
            if(_m_bIsInited)
            {
                if (null != _delegate)
                    _delegate();
                return;
            }

            if (null == _m_dDelegate)
                _m_dDelegate = _delegate;
            else
                _m_dDelegate += _delegate;
        }

        /// <summary>
        /// 设置初始化完成，此时将调用回调
        /// </summary>
        public void setInitDone()
        {
            _m_bIsInited = true;

            Action doneDelegate = _m_dDelegate;
            _m_dDelegate = null;
            if (null != doneDelegate)
                doneDelegate();
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public void reset()
        {
            _m_dDelegate = null;

            //先调用回调再设置未完成，避免在回调内设置了完成状态
            _m_bIsInited = false;
        }
    }
}
