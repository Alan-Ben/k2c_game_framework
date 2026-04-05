package GC2GS.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 火星居民-处理奖励帮助
 **/
public class GC2GS_040_004_ReqDealRewardHelp implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;


public GC2GS_040_004_ReqDealRewardHelp() {
	id = (long)0;
}

public GC2GS_040_004_ReqDealRewardHelp(
	 long _id
) {	id = _id;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)4; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)4);
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

