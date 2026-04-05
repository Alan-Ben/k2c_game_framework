package NP2IS_RB.p001_ISOp;

import java.nio.ByteBuffer;
public class NP2IS_RB_001_007_RetSendPrivateMsg implements ALBasicProtocolPack._IALProtocolStructure {
private long msgId;


public NP2IS_RB_001_007_RetSendPrivateMsg() {
	msgId = (long)0;
}

public NP2IS_RB_001_007_RetSendPrivateMsg(
	 long _msgId
) {	msgId = _msgId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)7; }

public long getMsgId() { return msgId; }
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
	_buf.put((byte)1);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)7);
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

