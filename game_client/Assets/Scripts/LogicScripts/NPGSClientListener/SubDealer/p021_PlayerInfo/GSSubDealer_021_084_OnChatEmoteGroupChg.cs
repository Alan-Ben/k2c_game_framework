using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 聊天表情包变更推送
    /// </summary>
    public class GSSubDealer_021_084_OnChatEmoteGroupChg : NPSubDealer<GS2GC_021_084_OnChatEmoteGroupChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_084_OnChatEmoteGroupChg _createProtocolObj()
        {
            return new GS2GC_021_084_OnChatEmoteGroupChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_084_OnChatEmoteGroupChg _msg)
        {
            NPPlayer.instance.chatComp.onChatEmoteGroupChg(_msg);
        }
    }
}