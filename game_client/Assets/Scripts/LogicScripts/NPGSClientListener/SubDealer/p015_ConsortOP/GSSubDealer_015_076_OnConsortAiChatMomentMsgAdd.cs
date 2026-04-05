using ALBasicProtocolPack;
using GS2GC.p015_ConsortOp;

namespace GOE
{
    /// <summary>
    /// 家人朋友圈AI对话新增消息
    /// </summary>
    public class GSSubDealer_015_076_OnConsortAiChatMomentMsgAdd : NPSubDealer<GS2GC_015_076_OnConsortAiChatMomentMsgAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_015_076_OnConsortAiChatMomentMsgAdd _createProtocolObj()
        {
            return new GS2GC_015_076_OnConsortAiChatMomentMsgAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_015_076_OnConsortAiChatMomentMsgAdd _msg)
        {
            NPPlayer.instance.consortChatComp.onConsortAiChatMomentMsgAdd(_msg);
        }
    }
}