package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 请求查询单个集结信息
 **/
public class GC2GS_042_062_ReqQueryRallyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 集结ID */
private long rallyId;


public GC2GS_042_062_ReqQueryRallyInfo() {
	rallyId = (long)0;
}

public GC2GS_042_062_ReqQueryRallyInfo(
	 long _rallyId
) {	rallyId = _rallyId;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)62; }

/** 集结ID */
public long getRallyId() { return rallyId; }
/** 集结ID */
public void setRallyId(long _rallyId) { rallyId = _rallyId; }


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
	if(_buf.remaining() > 0) rallyId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rallyId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)62);
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

