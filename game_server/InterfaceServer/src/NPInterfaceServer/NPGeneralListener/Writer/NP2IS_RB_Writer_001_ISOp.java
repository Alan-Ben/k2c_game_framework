package NPInterfaceServer.NPGeneralListener.Writer;


import Common.ServerObj.GSLoginCheckObj;
import NP2IS_RB.p001_ISOp.*;
import NPInterfaceServer.ChatSDKConf;

public class NP2IS_RB_Writer_001_ISOp
{
    public static NP2IS_RB_001_001_RetRegRoom make_001_RetRegRoom(long _sdkRoomId)
    {
        NP2IS_RB_001_001_RetRegRoom proto = new NP2IS_RB_001_001_RetRegRoom();
        proto.setSdkRoomId(_sdkRoomId);
        
        return proto;
    }
    
    public static NP2IS_RB_001_002_RetUnRegRoom make_002_RetUnRegRoom()
    {
    	NP2IS_RB_001_002_RetUnRegRoom proto = new NP2IS_RB_001_002_RetUnRegRoom();
        
        return proto;
    }

    public static NP2IS_RB_001_003_RetRegChatUser make_003_RetRegChatUser(String _chatUid, GSLoginCheckObj _checkObj)
    {
    	NP2IS_RB_001_003_RetRegChatUser proto = new NP2IS_RB_001_003_RetRegChatUser();
    	proto.setChatUid(_chatUid);
    	proto.setSystemId(ChatSDKConf.getInstance().getSystemId());
    	proto.setSystemTag(ChatSDKConf.getInstance().getSystemTag());
    	proto.setIp(_checkObj.getIp());
    	proto.setPort(_checkObj.getPort());
    	proto.setCheckCode(_checkObj.getCheckCode());
    	
        return proto;
    }
    
    public static NP2IS_RB_001_004_RetJoinRoom make_004_RetJoinRoom()
    {
    	NP2IS_RB_001_004_RetJoinRoom proto = new NP2IS_RB_001_004_RetJoinRoom();
        
        return proto;
    }
    
    public static NP2IS_RB_001_005_RetQuitRoom make_005_RetQuitRoom()
    {
    	NP2IS_RB_001_005_RetQuitRoom proto = new NP2IS_RB_001_005_RetQuitRoom();
        
        return proto;
    }

    public static NP2IS_RB_001_006_RetSendRoomMsg make_006_RetSendRoomMsg()
    {
    	NP2IS_RB_001_006_RetSendRoomMsg proto = new NP2IS_RB_001_006_RetSendRoomMsg();
        
        return proto;
    }

    public static NP2IS_RB_001_007_RetSendPrivateMsg make_007_RetSendPrivateMsg(long _msgId)
    {
    	NP2IS_RB_001_007_RetSendPrivateMsg proto = new NP2IS_RB_001_007_RetSendPrivateMsg();
    	proto.setMsgId(_msgId);
        
        return proto;
    }
}
