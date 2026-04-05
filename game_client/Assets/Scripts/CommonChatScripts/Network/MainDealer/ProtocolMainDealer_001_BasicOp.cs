
using ALBasicProtocolPack;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 基础的系统消息处理
    /// </summary>
    internal class ProtocolMainDealer_001_BasicOp : ALBasicProtocolMainOrderDealer
    {
        internal ProtocolMainDealer_001_BasicOp()
            : base(1, 1)
        {
            // 网络的心跳包回包
            regDealer(new ProtocolSubDealer_001_001_RetHeartBeat());
        }
    }
}
