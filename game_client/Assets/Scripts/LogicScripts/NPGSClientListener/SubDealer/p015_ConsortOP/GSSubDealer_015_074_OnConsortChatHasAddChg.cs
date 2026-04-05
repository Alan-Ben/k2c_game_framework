using ALBasicProtocolPack;
using GS2GC.p015_ConsortOp;

namespace GOE
{
    /// <summary>
    /// 家人对话是否添加好友标志位变更
    /// </summary>
    public class GSSubDealer_015_074_OnConsortChatHasAddChg : NPSubDealer<GS2GC_015_074_OnConsortChatHasAddChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_015_074_OnConsortChatHasAddChg _createProtocolObj()
        {
            return new GS2GC_015_074_OnConsortChatHasAddChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_015_074_OnConsortChatHasAddChg _msg)
        {
            NPPlayer.instance.consortChatComp.onConsortChatHasAddChg(_msg);
        }
    }
}