using System;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 通用的处理了错误码的回调callback，只需要传成功回调，错误默认上浮提示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonErrCodeRequestCallbackDispatherTriggerDealer : _ANPRequestCallbackDispatherTriggerDealer
    {
        private Action _m_sucAction;

        public CommonErrCodeRequestCallbackDispatherTriggerDealer(Action _sucAction)
            : base()
        {
            _m_sucAction = _sucAction;
        }

        protected override void _dealSuc()
        {
            if (null != _m_sucAction)
                _m_sucAction();
            _m_sucAction = null;
        }

        protected override void _dealFail(int _errCode)
        {
            //上浮提示
            NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
        }
    }
}