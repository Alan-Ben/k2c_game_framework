package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品升级
 **/
public class GC2GS_013_021_ReqEquipUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
private boolean isTen;


public GC2GS_013_021_ReqEquipUpgrade() {
	dbId = (long)0;
	isTen = false;
}

public GC2GS_013_021_ReqEquipUpgrade(
	 long _dbId
	, boolean _isTen
) {	dbId = _dbId;
	isTen = _isTen;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)21; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
public boolean getIsTen() { return isTen; }
public void setIsTen(boolean _isTen) { isTen = _isTen; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)21);
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

