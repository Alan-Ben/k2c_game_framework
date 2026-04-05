package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 开启宴会（许可证模式）
 **/
public class GC2GS_019_002_ReqStartDinnerByPermit implements ALBasicProtocolPack._IALProtocolStructure {
/** 许可证实例ID */
private long permitInstanceId;


public GC2GS_019_002_ReqStartDinnerByPermit() {
	permitInstanceId = (long)0;
}

public GC2GS_019_002_ReqStartDinnerByPermit(
	 long _permitInstanceId
) {	permitInstanceId = _permitInstanceId;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)2; }

/** 许可证实例ID */
public long getPermitInstanceId() { return permitInstanceId; }
/** 许可证实例ID */
public void setPermitInstanceId(long _permitInstanceId) { permitInstanceId = _permitInstanceId; }


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
	if(_buf.remaining() > 0) permitInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(permitInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)2);
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

