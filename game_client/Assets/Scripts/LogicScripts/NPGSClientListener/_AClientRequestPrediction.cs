using System;
using ALBasicProtocolPack;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器发送请求的预测。
    /// </summary>
    /// <remarks>
    /// 这个类能够发送请求的同时，预测请求的结果，并在真正的服务端结果放回时比对预测结果，如果不一致则修正预测结果。
    /// </remarks>
    public abstract class _AClientRequestPrediction<T_REQUEST, T_RESPONSE>
        where T_REQUEST : _IALProtocolStructure
        where T_RESPONSE : _IALProtocolStructure, new()
    {
        public T_RESPONSE send()
        {
            T_REQUEST request = _createProtocolObj();
            T_RESPONSE predictResponse = _predictResponse();
            _enablePrediction(predictResponse);
            NPGSClientListener.sendRequestByLog(request,
                new CommonRequestSucFailSameCallbackProtocolDealer<T_RESPONSE>((_isSuc, _msg) =>
                {
                    if (!_isPredictionCorrect(predictResponse, _isSuc, _msg))
                    {
#if UNITY_EDITOR
                        ALLog.Error($"{typeof(T_REQUEST).Name} 客户端预测错误。: " + GCommon.GetInfoPropertys(predictResponse));
#endif
                        _fixWrongPrediction(predictResponse, _isSuc, _msg);
                    }
                }));
            return predictResponse;
        }

        protected abstract T_REQUEST _createProtocolObj();
        protected abstract T_RESPONSE _predictResponse();
        protected abstract void _enablePrediction(T_RESPONSE _predictResponse);
        protected abstract bool _isPredictionCorrect(T_RESPONSE _predictResponse, bool _isRequestSuc, T_RESPONSE _serverResponse);
        protected abstract void _fixWrongPrediction(T_RESPONSE _predictResponse, bool _isRequestSuc, T_RESPONSE _serverResponse);
    }
}