package ALLRPC.US.Friend;

import java.nio.ByteBuffer;
public class RemoveFriend_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 发起移除请求玩家CID */
private long sendCid;
/** 目标玩家CID */
private long targetCid;


public RemoveFriend_Req() {
	sendCid = (long)0;
	targetCid = (long)0;
}

public RemoveFriend_Req(
	 long _sendCid
	, long _targetCid
) {	sendCid = _sendCid;
	targetCid = _targetCid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 发起移除请求玩家CID */
public long getSendCid() { return sendCid; }
/** 发起移除请求玩家CID */
public void setSendCid(long _sendCid) { sendCid = _sendCid; }
/** 目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sendCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sendCid);
	_buf.putLong(targetCid);
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

