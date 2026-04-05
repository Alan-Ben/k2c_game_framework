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
    public abstract class _ANPRequestCallbackProtocolDealer<T> : _INPRequestCallbackDealer where T : _IALProtocolStructure
    {
        //是否在处理回调前默认调用协议处理
        private bool _m_bPreDealGCProtocol;

        //是否需要打印协议
        private bool _m_bNeedprintProtocol;

        public _ANPRequestCallbackProtocolDealer()
        {
            _m_bPreDealGCProtocol = false;
            _m_bNeedprintProtocol = true;
        }
        public _ANPRequestCallbackProtocolDealer(bool _preDealGCProtocol, bool _needPrintProtocol)
        {
            _m_bPreDealGCProtocol = _preDealGCProtocol;
            _m_bNeedprintProtocol = _needPrintProtocol;
        }

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
            //如果需要调用协议，则优先调用协议处理
            if (_m_bPreDealGCProtocol)
                GSProtocolDispather.instance.DealProtocol(NPGSClientListener.instance, _sucMsg);

            //后续调用实际逻辑处理
            T protocol = _createSucProtocolObj();

            ALProtocolBuf alBuf = new ALProtocolBuf(_sucMsg);
            //先将前2个字节的信息读取掉
            alBuf.get();
            alBuf.get();

            //读取协议内容
            if (null != protocol)
                protocol.readPackage(alBuf);

            if(Game.instance.mainCamera.gameSetting.printProtocol && _m_bNeedprintProtocol)
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
        
        /**************
         * 回调的处理函数
         * @param _retProtocol
         */
        public void dealFail(int _errCode, byte[] _sucMsg)
        {
            T protocol = _createSucProtocolObj();
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

            if (protocol.GetFullPackBufSize() > Game.instance.mainCamera.gameSetting.protocolErrorMinSize)
            {
                Debug.LogError($"协议大小超过20k: {protocol.GetType().Name}，大小：{protocol.GetFullPackBufSize()}");
            }
#endif
            
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
