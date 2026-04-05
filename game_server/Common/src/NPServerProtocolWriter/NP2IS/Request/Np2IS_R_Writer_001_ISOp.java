package NPServerProtocolWriter.NP2IS.Request;

import NP2IS_R.p001_ISOp.*;

import java.nio.ByteBuffer;


public class Np2IS_R_Writer_001_ISOp
{
    public static NP2IS_R_001_001_ReqRegRoom make_001_ReqRegRoom(int _roomType, long _roomTypeId)
    {
        NP2IS_R_001_001_ReqRegRoom proto = new NP2IS_R_001_001_ReqRegRoom();
        proto.setRoomType(_roomType);
        proto.setRoomTypeId(_roomTypeId);
        
        return proto;
    }
    
    public static NP2IS_R_001_002_ReqUnRegRoom make_002_ReqUnRegRoom(int _roomType, long _roomTypeId)
    {
        NP2IS_R_001_002_ReqUnRegRoom proto = new NP2IS_R_001_002_ReqUnRegRoom();
        proto.setRoomType(_roomType);
        proto.setRoomTypeId(_roomTypeId);
        
        return proto;
    }
    
    public static NP2IS_R_001_003_ReqRegChatUser make_003_ReqRegChatUser(long _cid, String _areaTag)
    {
    	NP2IS_R_001_003_ReqRegChatUser proto = new NP2IS_R_001_003_ReqRegChatUser();
        proto.setCid(_cid);
        proto.setAreaTag(_areaTag);
        
        return proto;
    }
    
    public static NP2IS_R_001_004_ReqJoinRoom make_004_ReqJoinRoom(String _chatUid, long _sdkRoomId)
    {
    	NP2IS_R_001_004_ReqJoinRoom proto = new NP2IS_R_001_004_ReqJoinRoom();
        proto.setChatUid(_chatUid);
        proto.setSdkRoomId(_sdkRoomId);
        
        return proto;
    }
    
    public static NP2IS_R_001_005_ReqQuitRoom make_005_ReqQuitRoom(String _chatUid, long _sdkRoomId)
    {
    	NP2IS_R_001_005_ReqQuitRoom proto = new NP2IS_R_001_005_ReqQuitRoom();
        proto.setChatUid(_chatUid);
        proto.setSdkRoomId(_sdkRoomId);
        
        return proto;
    }
	
    public static NP2IS_R_001_006_ReqSendRoomMsg make_006_ReqSendRoomSysMsg(long _sdkRoomId, 
    		int _msgType, ByteBuffer _gameUser, ByteBuffer _gameContent)
    {
    	NP2IS_R_001_006_ReqSendRoomMsg proto = new NP2IS_R_001_006_ReqSendRoomMsg();
    	proto.setChatUid("0");
        proto.setSdkRoomId(_sdkRoomId);
        proto.setMsgType(_msgType);
        if(null != _gameUser)
        	proto.setGameUser(_gameUser);
        proto.setGameContent(_gameContent);
        
        return proto;
    }
    public static NP2IS_R_001_006_ReqSendRoomMsg make_006_ReqSendRoomMsg(String _chatUid, long _sdkRoomId, 
    		int _msgType, ByteBuffer _gameUser, ByteBuffer _gameContent)
    {
    	NP2IS_R_001_006_ReqSendRoomMsg proto = new NP2IS_R_001_006_ReqSendRoomMsg();
    	proto.setChatUid(_chatUid);
        proto.setSdkRoomId(_sdkRoomId);
        proto.setMsgType(_msgType);
        if(null != _gameUser)
        	proto.setGameUser(_gameUser);
        proto.setGameContent(_gameContent);
        
        return proto;
    }
	
    public static NP2IS_R_001_007_ReqSendPrivateMsg make_007_ReqSendPrivateMsg(String _senderChatUid, long _receiverCid, 
    		int _msgType, ByteBuffer _gameUser, ByteBuffer _gameContent)
    {
    	NP2IS_R_001_007_ReqSendPrivateMsg proto = new NP2IS_R_001_007_ReqSendPrivateMsg();
        proto.setSenderChatUid(_senderChatUid);
        proto.setReceiverCid(_receiverCid);
        proto.setMsgType(_msgType);
        proto.setGameUser(_gameUser);
        proto.setGameContent(_gameContent);
        
        return proto;
    }
}
