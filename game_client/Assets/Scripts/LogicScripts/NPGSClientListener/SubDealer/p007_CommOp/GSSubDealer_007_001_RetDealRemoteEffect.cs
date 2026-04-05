using ALPackage;
using ALBasicProtocolPack;
using NPCommon;
using System.Collections.Generic;

namespace GOE
{
    public class GSSubDealer_007_001_RetDealRemoteEffect : NPSubDealer<GS2GC.p007_CommOp.GS2GC_007_001_RetDealRemoteEffect>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p007_CommOp.GS2GC_007_001_RetDealRemoteEffect _createProtocolObj()
        {
            return new GS2GC.p007_CommOp.GS2GC_007_001_RetDealRemoteEffect();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p007_CommOp.GS2GC_007_001_RetDealRemoteEffect _msg)
        {
            if (_msg == null)
                return;

            //获取对应数据
            NPRemoteEffectRefObj remoteEffectRef = GRefdataCoreMgr.instance.remoteEffectMap.getRef(_msg.getRefId());
            if(null == remoteEffectRef)
            {
                ALLog.Error($"can not find remote effect: {_msg.getRefId()}");
                return;
            }
            
            //处理效果
            if(null != remoteEffectRef.client_effect && (null == remoteEffectRef.client_condition || remoteEffectRef.client_condition.IsEnable(null)))
            {
                remoteEffectRef.client_effect.dealEffect();
            }
            
            //发送远程效果处理完成的消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.REMOTE_EFFECT_DEAL_DONE);
        }
    }
}
