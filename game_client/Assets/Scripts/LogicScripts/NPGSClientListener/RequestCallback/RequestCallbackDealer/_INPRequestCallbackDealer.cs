using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的处理接口
    /// </summary>
    public interface _INPRequestCallbackDealer
    {
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        void dealSuc(byte[] _sucMsg);
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        void dealFail(int _errCode, byte[] _sucMsg);
    }
}
