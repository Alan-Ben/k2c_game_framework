using System;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 通用的处理了错误码的回调callback，只需要传成功回调，错误默认上浮提示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonErrCodeRequestCallbackProtocolDealer<T> : _ANPRequestCallbackProtocolDealer<T>
        where T : _IALProtocolStructure, new()
    {
        private Action<T> _m_sucAction;

        public CommonErrCodeRequestCallbackProtocolDealer(Action<T> _sucAction, bool _preDealGCProtocol = false, bool _needPrintProtocol = true)
            : base(_preDealGCProtocol, _needPrintProtocol)
        {
            _m_sucAction = _sucAction;
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
            //上浮提示
            NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
        }
    }
}