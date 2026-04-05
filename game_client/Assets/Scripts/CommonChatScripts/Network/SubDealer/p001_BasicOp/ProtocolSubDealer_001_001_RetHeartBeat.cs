using GS2GC.p001_BasicOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 心跳包回包的处理
    /// </summary>
    internal class ProtocolSubDealer_001_001_RetHeartBeat : _AProtocolBaseSubDealer<GS2GC_001_001_RetHeartBeat>
    {
        protected override void _dealProtocolByLog(ChatClient _dealer, GS2GC_001_001_RetHeartBeat _msg)
        {
            _dealer?.retHeart();
        }
    }
}
