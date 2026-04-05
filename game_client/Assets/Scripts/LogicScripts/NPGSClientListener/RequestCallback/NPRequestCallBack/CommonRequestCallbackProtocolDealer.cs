using System;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 通用的callBack方式回调，c#好像不能写匿名类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonRequestCallbackProtocolDealer<T> : _ANPRequestCallbackProtocolDealer<T>
        where T : _IALProtocolStructure, new()
    {
        private Action<T> _m_sucAction;
        private Action<int> _m_failAction;
        
        public CommonRequestCallbackProtocolDealer(Action<T> _sucAction, Action<int> _fail, bool _needDispatch = false, bool _needPrintProtocol = true)
            : base(_needDispatch, _needPrintProtocol)
        {
            _m_sucAction = _sucAction;
            _m_failAction = _fail;
        }

        protected override T _createSucProtocolObj()
        {
            return new T();
        }

        protected override void _dealSuc(T _sucMsg)
        {
            if (null != _m_sucAction)
                _m_sucAction(_sucMsg);
            _m_sucAction = null;
        }

        protected override void _dealFail(int _errCode)
        {
            if (null != _m_failAction)
                _m_failAction(_errCode);
            _m_failAction = null;
        }
    }
}