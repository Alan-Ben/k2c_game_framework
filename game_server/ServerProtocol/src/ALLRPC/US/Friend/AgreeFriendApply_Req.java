package ALLRPC.US.Friend;

import java.nio.ByteBuffer;
public class AgreeFriendApply_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 发起请求玩家CID */
private long applyCid;
/** 同意请求玩家CID */
private long agreeCid;


public AgreeFriendApply_Req() {
	applyCid = (long)0;
	agreeCid = (long)0;
}

public AgreeFriendApply_Req(
	 long _applyCid
	, long _agreeCid
) {	applyCid = _applyCid;
	agreeCid = _agreeCid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 发起请求玩家CID */
public long getApplyCid() { return applyCid; }
/** 发起请求玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 同意请求玩家CID */
public long getAgreeCid() { return agreeCid; }
/** 同意请求玩家CID */
public void setAgreeCid(long _agreeCid) { agreeCid = _agreeCid; }


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
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) agreeCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(agreeCid);
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

