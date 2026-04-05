package GS2GC.p022_ChatOp;

import java.nio.ByteBuffer;
public class GS2GC_022_005_RetPlayerSendPrivateMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 私聊消息ID */
private long msgId;


public GS2GC_022_005_RetPlayerSendPrivateMsg() {
	msgId = (long)0;
}

public GS2GC_022_005_RetPlayerSendPrivateMsg(
	 long _msgId
) {	msgId = _msgId;
}

public final byte getMainOrder() { return (byte)22; }

public final byte getSubOrder() { return (byte)5; }

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
	_buf.put((byte)22);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)5);
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

