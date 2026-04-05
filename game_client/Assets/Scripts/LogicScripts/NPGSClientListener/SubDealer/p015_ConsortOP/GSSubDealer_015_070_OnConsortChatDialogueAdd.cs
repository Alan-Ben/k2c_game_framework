using ALBasicProtocolPack;
using GS2GC.p015_ConsortOp;

namespace GOE
{
    /// <summary>
    /// 新增家人对话推送
    /// </summary>
    public class GSSubDealer_015_070_OnConsortChatDialogueAdd : NPSubDealer<GS2GC_015_070_OnConsortChatDialogueAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_015_070_OnConsortChatDialogueAdd _createProtocolObj()
        {
            return new GS2GC_015_070_OnConsortChatDialogueAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_015_070_OnConsortChatDialogueAdd _msg)
        {
            NPPlayer.instance.consortChatComp.onConsortChatDialogueAdd(_msg);
        }
    }
}