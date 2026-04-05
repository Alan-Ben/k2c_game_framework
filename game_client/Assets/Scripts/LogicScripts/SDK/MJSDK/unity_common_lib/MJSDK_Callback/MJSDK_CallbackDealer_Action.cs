using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace MJSDK_Package
{
    /// <summary>
    /// 默认构造的一个带入Action可以快速定义处理函数的处理类
    /// 方便各组件快速调用
    /// </summary>
    public class MJSDK_CallbackDealer_Action : _IMJSDK_CallbackDealer
    {
        //成功回调
        private Action<string> _m_dSucDelegate;
        //失败回调
        private Action<int, string> _m_dFailDelegate;

        public MJSDK_CallbackDealer_Action(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            _m_dSucDelegate = _sucDelegate;
            _m_dFailDelegate = _failDelegate;
        }

        /// <summary>
        /// 在SDK消息处理成功之后的结果反馈
        /// </summary>
        /// <param name="_content"></param>
        void _IMJSDK_CallbackDealer.onSDKDealDone(string _content)
        {
            if (null != _m_dSucDelegate)
                _m_dSucDelegate(_content);
            else
                UnityEngine.Debug.LogError($"Msg DealDone: {_content}");
        }

        /// <summary>
        /// 当消息处理失败的时候，调用的本失败处理
        /// </summary>
        /// <param name="_errCode"></param>
        /// <param name="_content"></param>
        void _IMJSDK_CallbackDealer.onSDKDealFail(int _errCode, string _content)
        {
            if (null != _m_dFailDelegate)
                _m_dFailDelegate(_errCode, _content);
            else
                UnityEngine.Debug.LogError($"Msg DealFail: Err[{_errCode}] - Content[{_content}]");
        }
    }
}
