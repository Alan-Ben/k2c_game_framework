package GC2GS.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 经营建筑解锁产品
 **/
public class GC2GS_010_007_ReqBusinessUnlockProduct implements ALBasicProtocolPack._IALProtocolStructure {
/** 产品ID */
private long refId;


public GC2GS_010_007_ReqBusinessUnlockProduct() {
	refId = (long)0;
}

public GC2GS_010_007_ReqBusinessUnlockProduct(
	 long _refId
) {	refId = _refId;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)7; }

/** 产品ID */
public long getRefId() { return refId; }
/** 产品ID */
public void setRefId(long _refId) { refId = _refId; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
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

