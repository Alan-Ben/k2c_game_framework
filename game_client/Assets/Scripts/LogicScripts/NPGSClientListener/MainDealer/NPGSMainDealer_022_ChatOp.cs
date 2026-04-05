
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSMainDealer_022_ChatOp : ALBasicProtocolMainOrderDealer
    {
        public NPGSMainDealer_022_ChatOp() :
            base(22, 60)
        {
            regDealer(new GSSubDealer_022_002_RetPlayerJoinChatRoom());
            regDealer(new GSSubDealer_022_003_RetPlayerQuitChatRoom());
            regDealer(new GSSubDealer_022_004_RetPlayerSendRoomMsg());
            regDealer(new GSSubDealer_022_005_RetPlayerSendPrivateMsg());
            regDealer(new GSSubDealer_022_050_OnChatRoomJoin());
            regDealer(new GSSubDealer_022_051_OnChatRoomDisconnect());
        }
    }
}
