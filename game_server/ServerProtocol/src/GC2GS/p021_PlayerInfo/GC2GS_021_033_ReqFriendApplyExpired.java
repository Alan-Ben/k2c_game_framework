package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_033_ReqFriendApplyExpired implements ALBasicProtocolPack._IALProtocolStructure {
private long applyCid;


public GC2GS_021_033_ReqFriendApplyExpired() {
	applyCid = (long)0;
}

public GC2GS_021_033_ReqFriendApplyExpired(
	 long _applyCid
) {	applyCid = _applyCid;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)33; }

public long getApplyCid() { return applyCid; }
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }


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
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)33);
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

