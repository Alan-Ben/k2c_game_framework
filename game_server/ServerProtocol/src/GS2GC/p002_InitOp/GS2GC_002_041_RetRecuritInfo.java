package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_041_RetRecuritInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 已兑换id列表 */
private java.util.ArrayList<Long> hadRecuritIdList;


public GS2GC_002_041_RetRecuritInfo() {
	hadRecuritIdList = new java.util.ArrayList<Long>();
}

public GS2GC_002_041_RetRecuritInfo(
	 java.util.ArrayList<Long> _hadRecuritIdList
) {	hadRecuritIdList = _hadRecuritIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)41; }

/** 已兑换id列表 */
public java.util.ArrayList<Long> getHadRecuritIdList() { return hadRecuritIdList; }
/** 已兑换id列表 */
public void addHadRecuritIdList(long _hadRecuritIdList) { hadRecuritIdList.add(_hadRecuritIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadRecuritIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadRecuritIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadRecuritIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hadRecuritIdListCount; _i++) { 
		long _hadRecuritIdList = (long)0;
		if(_buf.remaining() > 0) _hadRecuritIdList = _buf.getLong();
		hadRecuritIdList.add(_hadRecuritIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)hadRecuritIdList.size());
	for(int _i = 0; _i < hadRecuritIdList.size(); _i++) { 
		_buf.putLong(hadRecuritIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)41);
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

