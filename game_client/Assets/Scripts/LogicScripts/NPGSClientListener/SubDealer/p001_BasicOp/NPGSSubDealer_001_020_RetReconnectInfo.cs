using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_020_RetReconnectInfo : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_020_RetReconnectInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_020_RetReconnectInfo _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_020_RetReconnectInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_020_RetReconnectInfo _msg)
        {
            //返回服务端收到的消息，此时客户端需要获取需要再次发送的消息并发送
            // 强制转化类型对象
            //验证消息,并获取需要返回的消息数量
            //UnityEngine.Debug.LogError("chk: " + WCGGSMsgDealer.instance.getListFirstSendedMsgSerialize() + " - " + _msg.getReceivedMesCount()
            //                    + " - " + WCGGSMsgDealer.instance.sendBackMsgCount);
            List<NPGSMsgItem> needSendBackMsg = NPGSMsgDealer.instance.getNeedSendBackMsg(_msg.getReceivedMesCount());
            //逐个消息返回
            if(null != needSendBackMsg)
            {
                for(int i = 0; i < needSendBackMsg.Count; i++)
                {
                    //UnityEngine.Debug.LogError("resend: " + needSendBackMsg[i].getMainOrder() + " - " + needSendBackMsg[i].getSubOrder());
                    //UnityEngine.Debug.LogError($"send {_msg.getReceivedMesCount() + i + 1} - {needSendBackMsg[i].clientSerialize}");
                    //返回消息，由于需要从接受下标往后。所以需要累加1
                    NPGSClientListener.directSendMsg(
                        NPGSWriter_001_BasicOp.make_022_SendSerializeMsg(_msg.getReceivedMesCount() + i + 1, needSendBackMsg[i].clientSerialize, needSendBackMsg[i].protocolObj.makeFullPackage()));
                }
            }
        }
    }
}

