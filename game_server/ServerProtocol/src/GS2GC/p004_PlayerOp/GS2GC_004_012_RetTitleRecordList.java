package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_012_RetTitleRecordList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> titleIdList;


public GS2GC_004_012_RetTitleRecordList() {
	titleIdList = new java.util.ArrayList<Long>();
}

public GS2GC_004_012_RetTitleRecordList(
	 java.util.ArrayList<Long> _titleIdList
) {	titleIdList = _titleIdList;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)12; }

public java.util.ArrayList<Long> getTitleIdList() { return titleIdList; }
public void addTitleIdList(long _titleIdList) { titleIdList.add(_titleIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (titleIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (titleIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _titleIdListCount = _buf.getShort();
	for(int _i = 0; _i < _titleIdListCount; _i++) { 
		long _titleIdList = (long)0;
		if(_buf.remaining() > 0) _titleIdList = _buf.getLong();
		titleIdList.add(_titleIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)titleIdList.size());
	for(int _i = 0; _i < titleIdList.size(); _i++) { 
		_buf.putLong(titleIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)12);
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

