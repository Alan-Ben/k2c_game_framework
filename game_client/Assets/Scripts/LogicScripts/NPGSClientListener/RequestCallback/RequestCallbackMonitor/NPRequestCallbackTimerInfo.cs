using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的超时监控对象
    /// </summary>
    public struct NPRequestCallbackTimerInfo
    {
        //回调序列号
        private long _m_lCallbackSerialize;
        //超时的时间标记
        private long _m_lTimeoutTimeMS;

        public NPRequestCallbackTimerInfo(long _callbackSerialize, long _timeoutTimeMS)
        {
            _m_lCallbackSerialize = _callbackSerialize;
            _m_lTimeoutTimeMS = _timeoutTimeMS;
        }

        public long getCallbackSerialize() { return _m_lCallbackSerialize; }
        public long getTimeoutTimeMS() { return _m_lTimeoutTimeMS; }
    }
}
