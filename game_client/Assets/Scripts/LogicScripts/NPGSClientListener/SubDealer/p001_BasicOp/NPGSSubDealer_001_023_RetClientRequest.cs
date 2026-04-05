using UnityEngine;
using System.Collections;
using ALBasicProtocolPack;
using ALPackage;

namespace GOE
{
    public class NPGSSubDealer_001_023_RetClientRequest : _AALBasicProtocolSubBasicOrderDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_023_RetClientRequest>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_023_RetClientRequest _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_023_RetClientRequest();
        }

        protected override void _dealProtocol(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_023_RetClientRequest _msg)
        {
            //检测消息是否合法
            int checkMsg = NPGSMsgDealer.instance.checkClientMsgSerialize(_msg.getMsgSerialize());

            //返回0表示重复消息
            if(checkMsg == 0)
                return;

            //返回-1表示消息顺序错乱
            if(checkMsg == -1)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("rec Msg Order Err! - " + _msg.getMsgSerialize() + " curSerialize:");
#endif
                return;
            }

            //根据结果进行消息处理
            bool dealSuc = false;
            if (_msg.getRes())
            {
                dealSuc = NPRequestCallbackMgr.instance.dealCallbackSucDealer(_msg.getClientRequestSerialize(), _msg.getMsgBuffer());
            }
            else
            {
                dealSuc = NPRequestCallbackMgr.instance.dealCallbackFailDealer(_msg.getClientRequestSerialize(), _msg.getErrCode(), _msg.getMsgBuffer());
            }

            //如果都没有处理，则走Dispather
            if(!dealSuc)
                GSProtocolDispather.instance.DealProtocol(NPGSClientListener.instance, _msg.getMsgBuffer());

            //发送对应消息处理统计
            NPGSClientListener.instance.sendMes(NPGSWriter_001_BasicOp.make_021_ReceivedMsg(NPGSMsgDealer.instance.getReceivedMsgCount()));
        }
    }
}
