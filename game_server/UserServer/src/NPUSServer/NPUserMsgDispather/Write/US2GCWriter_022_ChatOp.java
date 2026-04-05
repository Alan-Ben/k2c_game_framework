package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p022_ChatOp.*;


public class US2GCWriter_022_ChatOp
{
    public static GS2GC_022_001_RetPlayerChatLogin make_001_RetPlayerChatLogin(long _systemId, String _systemTag, String _ip, int _port, String _checkCode)
    {
        GS2GC_022_001_RetPlayerChatLogin proto = new GS2GC_022_001_RetPlayerChatLogin();
        proto.setSystemId(_systemId);
        proto.setSystemTag(_systemTag);
        proto.setIp(_ip);
        proto.setPort(_port);
        proto.setCheckCode(_checkCode);
        
        return proto;
    }
    
    public static GS2GC_022_002_RetPlayerJoinChatRoom make_002_RetPlayerJoinChatRoom()
    {
    	GS2GC_022_002_RetPlayerJoinChatRoom proto = new GS2GC_022_002_RetPlayerJoinChatRoom();
    	
    	return proto;
    }
    
    public static GS2GC_022_003_RetPlayerQuitChatRoom make_003_RetPlayerQuitChatRoom()
    {
    	GS2GC_022_003_RetPlayerQuitChatRoom proto = new GS2GC_022_003_RetPlayerQuitChatRoom();
    	
    	return proto;
    }
    
    public static GS2GC_022_004_RetPlayerSendRoomMsg make_004_RetPlayerSendRoomMsg()
    {
    	GS2GC_022_004_RetPlayerSendRoomMsg proto = new GS2GC_022_004_RetPlayerSendRoomMsg();
    	
    	return proto;
    }
    
    public static GS2GC_022_005_RetPlayerSendPrivateMsg make_005_RetPlayerSendPrivateMsg(long _msgId)
    {
    	GS2GC_022_005_RetPlayerSendPrivateMsg proto = new GS2GC_022_005_RetPlayerSendPrivateMsg();
    	proto.setMsgId(_msgId);
    	
    	return proto;
    }

    public static GS2GC_022_006_RetPlayerSendRoomMsgV2 make_006_RetPlayerSendRoomMsgV2()
    {
        GS2GC_022_006_RetPlayerSendRoomMsgV2 proto = new GS2GC_022_006_RetPlayerSendRoomMsgV2();

        return proto;
    }
    
    public static GS2GC_022_050_OnChatRoomJoin make_050_OnChatRoomJoin(int _roomType, long _roomId, long _extId)
    {
    	GS2GC_022_050_OnChatRoomJoin proto = new GS2GC_022_050_OnChatRoomJoin();
    	proto.setRoomType(_roomType);
    	proto.setRoomId(_roomId);
        proto.setExtId(_extId);
    	
    	return proto;
    }
    
    public static GS2GC_022_051_OnChatRoomDisconnect make_051_OnChatRoomDisconnect(long _roomId)
    {
    	GS2GC_022_051_OnChatRoomDisconnect proto = new GS2GC_022_051_OnChatRoomDisconnect();
    	proto.setRoomId(_roomId);
    	
    	return proto;
    }
}
