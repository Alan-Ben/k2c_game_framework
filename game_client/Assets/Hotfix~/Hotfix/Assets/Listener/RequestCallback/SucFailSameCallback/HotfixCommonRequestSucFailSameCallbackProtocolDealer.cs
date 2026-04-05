using System;
using ALBasicProtocolPack;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 通用的callBack方式回调，c#好像不能写匿名类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HotfixCommonRequestSucFailSameCallbackProtocolDealer<T> : _AHotfixRequestCallbackProtocolFailMsgDealer<T, T>
        where T : _IALProtocolStructure, new()
    {
        private Action<bool, T> _m_reqCallBack;//请求结果回调, 第一个参数代表是否成功, 第二个参数是返回协议内容
        private Action<int> _m_dealErrorCode;//对失败时的错误码进行操作的回调
        private bool _m_bShowErrorCodeTip;//是否需要提示错误码
        private bool _m_needDispatch;//是否需要分发消息

        protected override bool needDispath { get { return _m_needDispatch; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_reqCallBack">请求结果回调, 第一个参数代表是否成功, 第二个参数是返回协议内容</param>
        /// <param name="_dealFailErrorCode">对失败时的错误码进行操作的回调</param>
        /// <param name="_showErrorCodeTip">是否需要提示错误码</param>
        /// <param name="_needDispatch">是否需要分发消息</param>
        public HotfixCommonRequestSucFailSameCallbackProtocolDealer(Action<bool, T> _reqCallBack, Action<int> _dealFailErrorCode = null, bool _showErrorCodeTip = true, bool _needDispatch = false)
        {
            _m_reqCallBack = _reqCallBack;
            _m_dealErrorCode = _dealFailErrorCode;
            _m_bShowErrorCodeTip = _showErrorCodeTip;
            _m_needDispatch = _needDispatch;
        }

        protected override T _createSucProtocolObj()
        {
            return new T();
        }

        protected override void _dealSuc(T _sucMsg)
        {
            if (_m_reqCallBack != null)
                _m_reqCallBack(true, _sucMsg);
            _m_reqCallBack = null;
        }

        protected override T _createFailProtocolObj(int _errCode)
        {
            return new T();
        }

        protected override void _dealFail(int _errCode, T _sucMsg)
        {
            if(_m_bShowErrorCodeTip)//显示上浮提示
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
            
            if (_m_reqCallBack != null)
                _m_reqCallBack(false, _sucMsg);
            _m_reqCallBack = null;

            if (_m_dealErrorCode != null)
                _m_dealErrorCode(_errCode);
            _m_dealErrorCode = null;

        }
    }
}