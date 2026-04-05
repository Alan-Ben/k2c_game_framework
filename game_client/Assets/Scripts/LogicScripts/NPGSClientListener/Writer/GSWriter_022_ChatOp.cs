
using ChatPackage;
using GC2GS.p022_ChatOp;
using NPEnum;

namespace GOE
{
    public static class GSWriter_022_ChatOp
    {
        public static GC2GS_022_001_ReqPlayerChatLogin make_001_ReqPlayerChatLogin()
        {
            return new GC2GS_022_001_ReqPlayerChatLogin();
        }

        public static GC2GS_022_002_ReqPlayerJoinChatRoom make_002_ReqPlayerJoinChatRoom(int _roomType, long _extId)
        {
            GC2GS_022_002_ReqPlayerJoinChatRoom protocol = new GC2GS_022_002_ReqPlayerJoinChatRoom(_roomType, _extId);
            return protocol;
        }

        public static GC2GS_022_003_ReqPlayerQuitChatRoom make_003_ReqPlayerQuitChatRoom(int _roomType, long _extId)
        {
            GC2GS_022_003_ReqPlayerQuitChatRoom protocol = new GC2GS_022_003_ReqPlayerQuitChatRoom(_roomType, _extId);
            return protocol;
        }

        public static GC2GS_022_004_ReqPlayerSendRoomMsg make_004_ReqPlayerSendRoomMsg(long _roomId, _AMsgDetailInfo _msgInfo)
        {
            if (_msgInfo == null)
                return null;
            GC2GS_022_004_ReqPlayerSendRoomMsg protocol = new GC2GS_022_004_ReqPlayerSendRoomMsg(_roomId,_msgInfo.msgType,_msgInfo.getSenderBytesData(), _msgInfo.getContentBytesData());
            return protocol;
        }
        
        public static GC2GS_022_006_ReqPlayerSendRoomMsgV2 make_006_ReqPlayerSendRoomMsgV2(ENPChatRoomType _roomType, long _extId, _AMsgDetailInfo _msgInfo)
        {
            if (_msgInfo == null)
                return null;
            GC2GS_022_006_ReqPlayerSendRoomMsgV2 protocol = new GC2GS_022_006_ReqPlayerSendRoomMsgV2((int)_roomType, _extId, _msgInfo.msgType,_msgInfo.getSenderBytesData(), _msgInfo.getContentBytesData());
            return protocol;
        }
        
        public static GC2GS_022_005_ReqPlayerSendPrivateMsg make_005_ReqPlayerSendPrivateMsg(long _receiverCid, _AMsgDetailInfo _msgInfo)
        {
            if (_msgInfo == null)
                return null;
            GC2GS_022_005_ReqPlayerSendPrivateMsg protocol = new GC2GS_022_005_ReqPlayerSendPrivateMsg(_receiverCid,_msgInfo.msgType,_msgInfo.getSenderBytesData(), _msgInfo.getContentBytesData());
            return protocol;
        }
    }
}