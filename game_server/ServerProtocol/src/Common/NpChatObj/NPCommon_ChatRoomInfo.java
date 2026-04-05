package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * NP聊天房间信息
 **/
public class NPCommon_ChatRoomInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天服务器房间的唯一id */
private long chatSdkRoomId;
/** 房间类型,ENPChatRoomType */
private int chatRoomType;
/** 在指定类型下的房间唯一id */
private long chatRoomSubTypeId;


public NPCommon_ChatRoomInfo() {
	chatSdkRoomId = (long)0;
	chatRoomType = 0;
	chatRoomSubTypeId = (long)0;
}

public NPCommon_ChatRoomInfo(
	 long _chatSdkRoomId
	, int _chatRoomType
	, long _chatRoomSubTypeId
) {	chatSdkRoomId = _chatSdkRoomId;
	chatRoomType = _chatRoomType;
	chatRoomSubTypeId = _chatRoomSubTypeId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 聊天服务器房间的唯一id */
public long getChatSdkRoomId() { return chatSdkRoomId; }
/** 聊天服务器房间的唯一id */
public void setChatSdkRoomId(long _chatSdkRoomId) { chatSdkRoomId = _chatSdkRoomId; }
/** 房间类型,ENPChatRoomType */
public int getChatRoomType() { return chatRoomType; }
/** 房间类型,ENPChatRoomType */
public void setChatRoomType(int _chatRoomType) { chatRoomType = _chatRoomType; }
/** 在指定类型下的房间唯一id */
public long getChatRoomSubTypeId() { return chatRoomSubTypeId; }
/** 在指定类型下的房间唯一id */
public void setChatRoomSubTypeId(long _chatRoomSubTypeId) { chatRoomSubTypeId = _chatRoomSubTypeId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatSdkRoomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatRoomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatRoomSubTypeId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chatSdkRoomId);
	_buf.putInt(chatRoomType);
	_buf.putLong(chatRoomSubTypeId);
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

