using ALBasicProtocolPack;
using GS2GC.p015_ConsortOp;

namespace GOE
{
    /// <summary>
    /// 家人对话奖励领取推送
    /// </summary>
    public class GSSubDealer_015_071_OnConsortChatDialogueRewardDraw : NPSubDealer<GS2GC_015_071_OnConsortChatDialogueRewardDraw>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_015_071_OnConsortChatDialogueRewardDraw _createProtocolObj()
        {
            return new GS2GC_015_071_OnConsortChatDialogueRewardDraw();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_015_071_OnConsortChatDialogueRewardDraw _msg)
        {
            NPPlayer.instance.consortChatComp.onConsortChatDialogueRewardDraw(_msg);

        }
    }
}