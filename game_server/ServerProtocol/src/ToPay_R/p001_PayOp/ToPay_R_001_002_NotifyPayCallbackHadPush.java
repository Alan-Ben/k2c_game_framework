package ToPay_R.p001_PayOp;

import java.nio.ByteBuffer;
public class ToPay_R_001_002_NotifyPayCallbackHadPush implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> dbIdList;


public ToPay_R_001_002_NotifyPayCallbackHadPush() {
	dbIdList = new java.util.ArrayList<Long>();
}

public ToPay_R_001_002_NotifyPayCallbackHadPush(
	 java.util.ArrayList<Long> _dbIdList
) {	dbIdList = _dbIdList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public java.util.ArrayList<Long> getDbIdList() { return dbIdList; }
public void addDbIdList(long _dbIdList) { dbIdList.add(_dbIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (dbIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dbIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dbIdListCount = _buf.getShort();
	for(int _i = 0; _i < _dbIdListCount; _i++) { 
		long _dbIdList = (long)0;
		if(_buf.remaining() > 0) _dbIdList = _buf.getLong();
		dbIdList.add(_dbIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dbIdList.size());
	for(int _i = 0; _i < dbIdList.size(); _i++) { 
		_buf.putLong(dbIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

