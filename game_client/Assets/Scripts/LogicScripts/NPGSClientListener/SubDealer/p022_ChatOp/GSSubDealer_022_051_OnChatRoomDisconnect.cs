
using ALBasicProtocolPack;
using GS2GC.p022_ChatOp;

namespace GOE
{
    public class GSSubDealer_022_051_OnChatRoomDisconnect : NPSubDealer<GS2GC_022_051_OnChatRoomDisconnect>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_022_051_OnChatRoomDisconnect _createProtocolObj()
        {
            return new GS2GC_022_051_OnChatRoomDisconnect();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_022_051_OnChatRoomDisconnect _msg)
        {
            if (null == _msg)
                return;
            
            if (NPPlayer.instance.chatComp != null)
                NPPlayer.instance.chatComp.onChatRoomDisconnect(_msg);

        }
    }
}
