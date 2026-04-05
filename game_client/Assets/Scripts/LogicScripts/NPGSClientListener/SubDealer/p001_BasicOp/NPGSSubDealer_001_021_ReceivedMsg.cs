using ALBasicProtocolPack;
using System.Collections.Generic;
using NPGS2GC.p001_BasicOp;
using UnityEngine;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_021_ReceivedMsg : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_021_ReceivedMsg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_021_ReceivedMsg _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_021_ReceivedMsg();
        }

        protected override bool needPrintProtocol { get { return Game.instance.mainCamera.gameSetting.printProtocol_001_021; } }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_021_ReceivedMsg _msg)
        {
            //返回服务端收到的消息，此时客户端需要获取需要再次发送的消息并发送
            // 强制转化类型对象
            //验证消息,并获取需要返回的消息数量
            //UnityEngine.Debug.LogError("chk: " + WCGGSMsgDealer.instance.getListFirstSendedMsgSerialize() + " - " + _msg.getReceivedMesCount()
            //                   + " - " + WCGGSMsgDealer.instance.sendBackMsgCount);

            if(_msg == null)
                return;

            NPGSMsgDealer.instance.checkSendedMsg(_msg.getReceivedMesCount());

            //UnityEngine.Debug.LogError("aft chk: " + WCGGSMsgDealer.instance.getListFirstSendedMsgSerialize() + " - " + _msg.getReceivedMesCount()
            //+ " - " + WCGGSMsgDealer.instance.sendBackMsgCount);
        }
    }
}

