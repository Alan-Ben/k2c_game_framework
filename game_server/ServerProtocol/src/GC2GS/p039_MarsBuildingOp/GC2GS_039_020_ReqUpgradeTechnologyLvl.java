package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星科技-科技升级
 **/
public class GC2GS_039_020_ReqUpgradeTechnologyLvl implements ALBasicProtocolPack._IALProtocolStructure {
/** 科技ID */
private long technologyId;


public GC2GS_039_020_ReqUpgradeTechnologyLvl() {
	technologyId = (long)0;
}

public GC2GS_039_020_ReqUpgradeTechnologyLvl(
	 long _technologyId
) {	technologyId = _technologyId;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)20; }

/** 科技ID */
public long getTechnologyId() { return technologyId; }
/** 科技ID */
public void setTechnologyId(long _technologyId) { technologyId = _technologyId; }


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
	if(_buf.remaining() > 0) technologyId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(technologyId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)20);
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

