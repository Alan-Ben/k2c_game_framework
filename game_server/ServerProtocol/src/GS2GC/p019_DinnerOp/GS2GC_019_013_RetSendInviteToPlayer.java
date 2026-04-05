package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 对指定玩家发起邀请
 **/
public class GS2GC_019_013_RetSendInviteToPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 私聊消息ID */
private long msgId;


public GS2GC_019_013_RetSendInviteToPlayer() {
	msgId = (long)0;
}

public GS2GC_019_013_RetSendInviteToPlayer(
	 long _msgId
) {	msgId = _msgId;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)13; }

/** 私聊消息ID */
public long getMsgId() { return msgId; }
/** 私聊消息ID */
public void setMsgId(long _msgId) { msgId = _msgId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msgId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(msgId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)13);
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

