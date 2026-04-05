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
    public abstract class _ANPRequestCallbackProtocolFailMsgDealer<T, T_F> : _INPRequestCallbackDealer
        where T : _IALProtocolStructure
        where T_F : _IALProtocolStructure
    {
        public _ANPRequestCallbackProtocolFailMsgDealer()
        {

        }

        /// <summary>
        /// 是否需要在处理之后再分发到正常的协议处理中进行处理，默认不处理
        /// </summary>
        protected virtual bool needDispath { get { return false; } }
        
        /// <summary>
        /// 创建用于解析的协议对象
        /// </summary>
        /// <returns></returns>
        protected abstract T _createSucProtocolObj();
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        public void dealSuc(byte[] _sucMsg)
        {
            //判断是否需要分发
            if (needDispath)
                GSProtocolDispather.instance.DealProtocol(NPGSClientListener.instance, _sucMsg);
            
            T protocol = _createSucProtocolObj();

            ALProtocolBuf alBuf = new ALProtocolBuf(_sucMsg);
            //先将前2个字节的信息读取掉
            alBuf.get();
            alBuf.get();

            //读取协议内容
            if (null != protocol)
                protocol.readPackage(alBuf);

            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if (protocol.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", protocol.GetType().Name, GCommon.GetInfoPropertys(protocol)));
                }
                else
                {
                    GCommon.NetWaring($"协议{protocol.GetType().Name}过大，大小：{protocol.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }
        
#if UNITY_EDITOR
            if (null != protocol && protocol.GetFullPackBufSize() > Game.instance.mainCamera.gameSetting.protocolErrorMinSize)
            {
                Debug.LogError($"协议大小超过20k: {protocol.GetType().Name}，大小：{protocol.GetFullPackBufSize()}");
            }
#endif  
            
            //进行处理
            _dealSuc(protocol);
        }
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        protected abstract void _dealSuc(T _sucMsg);

        /// <summary>
        /// 创建用于解析的协议对象
        /// </summary>
        /// <returns></returns>
        protected abstract T_F _createFailProtocolObj(int _errCode);
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        public void dealFail(int _errCode, byte[] _sucMsg)
        {
            T_F protocol = _createFailProtocolObj(_errCode);

            if (_sucMsg != null && _sucMsg.Length > 0)
            {
                ALProtocolBuf alBuf = new ALProtocolBuf(_sucMsg);
                //先将前2个字节的信息读取掉
                alBuf.get();
                alBuf.get();

                //读取协议内容
                if (null != protocol)
                    protocol.readPackage(alBuf);
            }
            
            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if (protocol.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> errorCode:{1} msg:{2}",
                        protocol.GetType().Name, _errCode, ProtocolErrorCodeResult.instance.getResultMsg(_errCode)));
                }
                else
                {
                    GCommon.NetWaring($"协议{protocol.GetType().Name}过大，大小：{protocol.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }
         
#if UNITY_EDITOR
            if (null != protocol && protocol.GetFullPackBufSize() > Game.instance.mainCamera.gameSetting.protocolErrorMinSize)
            {
                Debug.LogError($"协议大小超过20k: {protocol.GetType().Name}，大小：{protocol.GetFullPackBufSize()}");
            }
#endif  

            //进行实际处理
            _dealFail(_errCode, protocol);
        }
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        protected abstract void _dealFail(int _errCode, T_F _sucMsg);
    }
}
