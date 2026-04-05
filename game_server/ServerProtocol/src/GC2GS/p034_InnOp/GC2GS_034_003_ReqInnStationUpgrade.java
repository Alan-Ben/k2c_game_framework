package GC2GS.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店设施升级
 **/
public class GC2GS_034_003_ReqInnStationUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 设施ID */
private long stationId;


public GC2GS_034_003_ReqInnStationUpgrade() {
	stationId = (long)0;
}

public GC2GS_034_003_ReqInnStationUpgrade(
	 long _stationId
) {	stationId = _stationId;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)3; }

/** 设施ID */
public long getStationId() { return stationId; }
/** 设施ID */
public void setStationId(long _stationId) { stationId = _stationId; }


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
	if(_buf.remaining() > 0) stationId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(stationId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)3);
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

