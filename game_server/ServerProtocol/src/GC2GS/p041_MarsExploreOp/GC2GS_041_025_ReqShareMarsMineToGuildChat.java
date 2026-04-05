package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-分享矿到联盟聊天
 **/
public class GC2GS_041_025_ReqShareMarsMineToGuildChat implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例ID */
private long mineInstanceId;


public GC2GS_041_025_ReqShareMarsMineToGuildChat() {
	mineInstanceId = (long)0;
}

public GC2GS_041_025_ReqShareMarsMineToGuildChat(
	 long _mineInstanceId
) {	mineInstanceId = _mineInstanceId;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)25; }

/** 矿实例ID */
public long getMineInstanceId() { return mineInstanceId; }
/** 矿实例ID */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }


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
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mineInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)25);
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

