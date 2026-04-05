package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_040_RetAnnouncementRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 已领取公告id列表 */
private java.util.ArrayList<Long> hadDrawIdList;


public GS2GC_002_040_RetAnnouncementRecord() {
	hadDrawIdList = new java.util.ArrayList<Long>();
}

public GS2GC_002_040_RetAnnouncementRecord(
	 java.util.ArrayList<Long> _hadDrawIdList
) {	hadDrawIdList = _hadDrawIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)40; }

/** 已领取公告id列表 */
public java.util.ArrayList<Long> getHadDrawIdList() { return hadDrawIdList; }
/** 已领取公告id列表 */
public void addHadDrawIdList(long _hadDrawIdList) { hadDrawIdList.add(_hadDrawIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadDrawIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadDrawIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawIdListCount; _i++) { 
		long _hadDrawIdList = (long)0;
		if(_buf.remaining() > 0) _hadDrawIdList = _buf.getLong();
		hadDrawIdList.add(_hadDrawIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)hadDrawIdList.size());
	for(int _i = 0; _i < hadDrawIdList.size(); _i++) { 
		_buf.putLong(hadDrawIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)40);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)40);
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

