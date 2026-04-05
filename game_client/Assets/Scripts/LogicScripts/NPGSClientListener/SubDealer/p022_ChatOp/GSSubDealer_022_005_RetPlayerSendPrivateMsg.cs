
using ALBasicProtocolPack;
using GS2GC.p022_ChatOp;

namespace GOE
{
    public class GSSubDealer_022_005_RetPlayerSendPrivateMsg : NPSubDealer<GS2GC_022_005_RetPlayerSendPrivateMsg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_022_005_RetPlayerSendPrivateMsg _createProtocolObj()
        {
            return new GS2GC_022_005_RetPlayerSendPrivateMsg();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_022_005_RetPlayerSendPrivateMsg _msg)
        {
            if (_msg == null)
                return;
            
            // if (NPPlayer.instance.chatComp != null)
            //     NPPlayer.instance.chatComp.(_msg);
        }
    }
}
