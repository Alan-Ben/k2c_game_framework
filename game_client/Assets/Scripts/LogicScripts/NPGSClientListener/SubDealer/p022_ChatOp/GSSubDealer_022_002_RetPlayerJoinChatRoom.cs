
using ALBasicProtocolPack;
using GS2GC.p022_ChatOp;

namespace GOE
{
    public class GSSubDealer_022_002_RetPlayerJoinChatRoom : NPSubDealer<GS2GC_022_002_RetPlayerJoinChatRoom>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_022_002_RetPlayerJoinChatRoom _createProtocolObj()
        {
            return new GS2GC_022_002_RetPlayerJoinChatRoom();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_022_002_RetPlayerJoinChatRoom _msg)
        {
            if (_msg == null)
                return;
            
            // if (NPPlayer.instance.chatComp != null)
            //     NPPlayer.instance.chatComp.(_msg);
        }
    }
}
