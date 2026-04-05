using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的处理任务
    /// </summary>
    public class NPRequestCallbackDealFailTask : _IALBaseMonoTask
    {
        private _INPRequestCallbackDealer _m_iDealer;
        //错误码
        private int _m_iErrCode;
        //错误对应的返回消息
        private byte[] _m_arrFailMsg;

        public NPRequestCallbackDealFailTask(_INPRequestCallbackDealer _dealer, int _errCode, byte[] _failMsg)
        {
            _m_iDealer = _dealer;
            _m_iErrCode = _errCode;
            _m_arrFailMsg = _failMsg;
        }

        /***********
         * 处理函数
         */
        public void deal()
        {
            if (null == _m_iDealer)
                return;

            _m_iDealer.dealFail(_m_iErrCode, _m_arrFailMsg);
        }
    }
}
