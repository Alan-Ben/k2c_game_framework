package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 开启宴会（普通模式）
 **/
public class GC2GS_019_001_ReqStartDinner implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会配置ID */
private long dinnerId;


public GC2GS_019_001_ReqStartDinner() {
	dinnerId = (long)0;
}

public GC2GS_019_001_ReqStartDinner(
	 long _dinnerId
) {	dinnerId = _dinnerId;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)1; }

/** 宴会配置ID */
public long getDinnerId() { return dinnerId; }
/** 宴会配置ID */
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }


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
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dinnerId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)1);
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

