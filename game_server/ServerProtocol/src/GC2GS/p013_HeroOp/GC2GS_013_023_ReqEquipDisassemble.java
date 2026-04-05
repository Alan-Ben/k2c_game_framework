package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品分解
 **/
public class GC2GS_013_023_ReqEquipDisassemble implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id列表 */
private java.util.ArrayList<Long> dbList;


public GC2GS_013_023_ReqEquipDisassemble() {
	dbList = new java.util.ArrayList<Long>();
}

public GC2GS_013_023_ReqEquipDisassemble(
	 java.util.ArrayList<Long> _dbList
) {	dbList = _dbList;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)23; }

/** 数据id列表 */
public java.util.ArrayList<Long> getDbList() { return dbList; }
/** 数据id列表 */
public void addDbList(long _dbList) { dbList.add(_dbList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (dbList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dbList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dbListCount = _buf.getShort();
	for(int _i = 0; _i < _dbListCount; _i++) { 
		long _dbList = (long)0;
		if(_buf.remaining() > 0) _dbList = _buf.getLong();
		dbList.add(_dbList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dbList.size());
	for(int _i = 0; _i < dbList.size(); _i++) { 
		_buf.putLong(dbList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)23);
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

