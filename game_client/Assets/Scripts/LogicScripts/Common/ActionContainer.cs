using System;
using System.Collections.Generic;

namespace GOE
{
    /***************
     * 回调统计类对象
     **/
    public class ActionContainer
    {
        private Action _m_action;

        public ActionContainer()
        {
            _m_action = default(Action);
        }

        public Action action { get { return _m_action; } set { _m_action = value; } }

        public void dealAction()
        {
            if(null != _m_action)
                _m_action();
        }
    }
}
