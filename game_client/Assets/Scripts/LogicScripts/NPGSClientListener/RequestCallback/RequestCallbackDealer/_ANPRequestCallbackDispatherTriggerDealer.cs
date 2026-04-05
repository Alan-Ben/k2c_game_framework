using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的时候会调用消息dispaterh
    /// 本对象只做触发处理
    /// </summary>
    public abstract class _ANPRequestCallbackDispatherTriggerDealer : _INPRequestCallbackDealer
    {
        public _ANPRequestCallbackDispatherTriggerDealer()
        {
        }

        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        public void dealSuc(byte[] _sucMsg)
        {
            //如果需要调用协议，则优先调用协议处理
            GSProtocolDispather.instance.DealProtocol(NPGSClientListener.instance, _sucMsg);
            
            //进行处理
            _dealSuc();
        }
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        protected abstract void _dealSuc();
        
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        public void dealFail(int _errCode, byte[] _sucMsg)
        {
            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                GCommon.NetRecv(string.Format("<color=red>S -> C: </color> errorCode:{0} msg:{1}", _errCode, ProtocolErrorCodeResult.instance.getResultMsg(_errCode)));
            }
            
            //进行实际处理
            _dealFail(_errCode);
        }
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        protected abstract void _dealFail(int _errCode);
    }
}
