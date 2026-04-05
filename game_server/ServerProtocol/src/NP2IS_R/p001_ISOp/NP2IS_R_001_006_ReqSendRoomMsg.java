package NP2IS_R.p001_ISOp;

import java.nio.ByteBuffer;
/*********
 * 发送聊天房间消息
 **/
public class NP2IS_R_001_006_ReqSendRoomMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天系统用户唯一标识 */
private String chatUid;
/** 注册到聊天服务器的房间实例ID */
private long sdkRoomId;
/** 消息类型,解释消息内容 */
private int msgType;
/** 发送者信息 */
private byte[] gameUser;
/** 消息内容 */
private byte[] gameContent;


public NP2IS_R_001_006_ReqSendRoomMsg() {
	chatUid = "";
	sdkRoomId = (long)0;
	msgType = 0;
	gameUser = null;
	gameContent = null;
}

public NP2IS_R_001_006_ReqSendRoomMsg(
	 String _chatUid
	, long _sdkRoomId
	, int _msgType
	, byte[] _gameUser
	, byte[] _gameContent
) {	chatUid = _chatUid;
	sdkRoomId = _sdkRoomId;
	msgType = _msgType;
	gameUser = _gameUser;
	gameContent = _gameContent;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)6; }

/** 聊天系统用户唯一标识 */
public String getChatUid() { return chatUid; }
/** 聊天系统用户唯一标识 */
public void setChatUid(String _chatUid) { chatUid = _chatUid; }
/** 注册到聊天服务器的房间实例ID */
public long getSdkRoomId() { return sdkRoomId; }
/** 注册到聊天服务器的房间实例ID */
public void setSdkRoomId(long _sdkRoomId) { sdkRoomId = _sdkRoomId; }
/** 消息类型,解释消息内容 */
public int getMsgType() { return msgType; }
/** 消息类型,解释消息内容 */
public void setMsgType(int _msgType) { msgType = _msgType; }
/** 发送者信息 */
public byte[] getGameUser() { return gameUser; }
public java.nio.ByteBuffer get_buffer_GameUser() { if(null == gameUser)return null; else return ByteBuffer.wrap(gameUser); }

/** 发送者信息 */
public void setGameUser(byte[] _gameUser) { gameUser = _gameUser; }
public void setGameUser(java.nio.ByteBuffer _gameUser) 
{
	if(null == _gameUser){return;}
	int _oldPos = _gameUser.position();
	int _bufLength = _gameUser.remaining();
	gameUser = new byte[_bufLength];
	_gameUser.get(gameUser);
	_gameUser.position(_oldPos);
}

/** 消息内容 */
public byte[] getGameContent() { return gameContent; }
public java.nio.ByteBuffer get_buffer_GameContent() { if(null == gameContent)return null; else return ByteBuffer.wrap(gameContent); }

/** 消息内容 */
public void setGameContent(byte[] _gameContent) { gameContent = _gameContent; }
public void setGameContent(java.nio.ByteBuffer _gameContent) 
{
	if(null == _gameContent){return;}
	int _oldPos = _gameContent.position();
	int _bufLength = _gameContent.remaining();
	gameContent = new byte[_bufLength];
	_gameContent.get(gameContent);
	_gameContent.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + (gameUser == null ? 0 : gameUser.length);
	_size += 4 + (gameContent == null ? 0 : gameContent.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + (gameUser == null ? 0 : gameUser.length);
	_size += 4 + (gameContent == null ? 0 : gameContent.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatUid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkRoomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _gameUserCount = _buf.getInt();
	if(0 < _gameUserCount){
		gameUser = new byte[_gameUserCount];
		_buf.get(gameUser);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _gameContentCount = _buf.getInt();
	if(0 < _gameContentCount){
		gameContent = new byte[_gameContentCount];
		_buf.get(gameContent);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, chatUid);
	_buf.putLong(sdkRoomId);
	_buf.putInt(msgType);
	_buf.putInt((gameUser == null ? 0 : gameUser.length));
	if(null != gameUser){_buf.put(gameUser);}

	_buf.putInt((gameContent == null ? 0 : gameContent.length));
	if(null != gameContent){_buf.put(gameContent);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)6);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

