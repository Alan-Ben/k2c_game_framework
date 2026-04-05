using System;

namespace Hotfix
{
    public abstract class HotfixBaseComponent
    {
        //是否初始化了
        private bool _m_bIsInited;

        //是否初始化过程完结了
        private bool _m_bIsInitDone;

        //初始化结果
        private bool _m_bIsInitSuc;

        //回调数据
        private Action<bool> _m_aInitDelegate = default(Action<bool>);

        public bool isInitDone { get { return _m_bIsInitDone; } }
        public bool isInitSuc { get { return _m_bIsInitSuc; } }

        /// <summary>
        /// Component从Model里移除时调用
        /// </summary>
        public void discard()
        {
            _onDiscard();

            //清除数据
            _m_bIsInited = false;
            _m_bIsInitDone = false;
            _m_bIsInitSuc = false;

            _m_aInitDelegate = default(Action<bool>);
        }

        /// <summary>
        /// 开始初始化Component
        /// </summary>
        public void init(Action<bool> _doneDelegate)
        {
            if (_m_bIsInited)
            {
                if(_m_bIsInitDone)
                {
                    if (null != _doneDelegate)
                        _doneDelegate(_m_bIsInitSuc);
                }
                else
                {
                    _m_aInitDelegate += _doneDelegate;
                }

                return;
            }

            _m_bIsInited = true;
            //注册回调
            _m_aInitDelegate += _doneDelegate;

            //处理初始化操作
            _dealInit();
        }

        protected abstract void _dealInit();
        protected abstract void _onDiscard();

        protected abstract void _onInitDone();
        protected abstract void _onInitFail();

        public void regInitDone()
        {
        }

        public void setInitDone()
        {
            //设置结束
            _m_bIsInitDone = true;
            _m_bIsInitSuc = true;

            //调用事件函数
            _onInitDone();

            //调用回调
            if(null != _m_aInitDelegate)
                _m_aInitDelegate(_m_bIsInitSuc);
            _m_aInitDelegate = default(Action<bool>);
        }

        public void setInitFail()
        {
            //设置结束
            _m_bIsInitDone = true;
            _m_bIsInitSuc = false;

            //调用事件函数
            _onInitFail();

            //调用回调
            if(null != _m_aInitDelegate)
                _m_aInitDelegate(_m_bIsInitSuc);
            _m_aInitDelegate = default(Action<bool>);
        }
    }
}