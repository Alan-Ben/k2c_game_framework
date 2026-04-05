package ALLRPC.Common.Chat;

import java.nio.ByteBuffer;
public class JoinChatRoom_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long roomId;
/** 聊天房间用户 */
private Common.ServerObj.ServerObj_ChatUser chatUser;


public JoinChatRoom_Req() {
	roomId = (long)0;
	chatUser = new Common.ServerObj.ServerObj_ChatUser();
}

public JoinChatRoom_Req(
	 long _roomId
	, Common.ServerObj.ServerObj_ChatUser _chatUser
) {	roomId = _roomId;
	chatUser = _chatUser;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getRoomId() { return roomId; }
public void setRoomId(long _roomId) { roomId = _roomId; }
/** 聊天房间用户 */
public Common.ServerObj.ServerObj_ChatUser getChatUser() { return chatUser; }
/** 聊天房间用户 */
public void setChatUser(Common.ServerObj.ServerObj_ChatUser _chatUser) { chatUser = _chatUser; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + chatUser.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + chatUser.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _chatUserCustLen = _buf.getInt();
	int _chatUserCurPos = _buf.position();
	chatUser.ReadUnzipBuf(_buf, _chatUserCurPos + _chatUserCustLen);
	_buf.position(_chatUserCurPos + _chatUserCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(roomId);
	_buf.putInt(chatUser.GetBufSize());
	chatUser.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

