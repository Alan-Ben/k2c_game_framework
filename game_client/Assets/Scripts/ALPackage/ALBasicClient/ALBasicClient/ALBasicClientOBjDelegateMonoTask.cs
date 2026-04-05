using System;
using System.Collections.Generic;

namespace ALPackage
{
    /**********************
     * 用于在接收线程调用的错误函数任务
     **/
    public class ALBasicClientOBjDelegateMonoTask<T> : _IALBaseMonoTask
    {
        private Action<T> _m_aDelegate;
        private T _m_obj;

        public ALBasicClientOBjDelegateMonoTask(Action<T> _delegate, T _obj)
        {
            _m_aDelegate = _delegate;
            _m_obj = _obj;
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            if (null == _m_aDelegate)
                return;

            _m_aDelegate(_m_obj);
        }
    }
}
