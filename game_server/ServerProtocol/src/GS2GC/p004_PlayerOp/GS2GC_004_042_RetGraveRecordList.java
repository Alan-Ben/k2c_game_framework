package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_042_RetGraveRecordList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.GraveObj.GraveObj_Record> recordList;
private int count;


public GS2GC_004_042_RetGraveRecordList() {
	recordList = new java.util.ArrayList<Common.GraveObj.GraveObj_Record>();
	count = 0;
}

public GS2GC_004_042_RetGraveRecordList(
	 java.util.ArrayList<Common.GraveObj.GraveObj_Record> _recordList
	, int _count
) {	recordList = _recordList;
	count = _count;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)42; }

public java.util.ArrayList<Common.GraveObj.GraveObj_Record> getRecordList() { return recordList; }
public void addRecordList(Common.GraveObj.GraveObj_Record _recordList) { recordList.add(_recordList); }
public int getCount() { return count; }
public void setCount(int _count) { count = _count; }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (recordList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (recordList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _recordListCount = _buf.getShort();
	for(int _i = 0; _i < _recordListCount; _i++) { 
		Common.GraveObj.GraveObj_Record _recordList = new Common.GraveObj.GraveObj_Record();
		if(_buf.remaining() <= 0) return;
	int __recordListCustLen = _buf.getInt();
	int __recordListCurPos = _buf.position();
	_recordList.ReadUnzipBuf(_buf, __recordListCurPos + __recordListCustLen);
	_buf.position(__recordListCurPos + __recordListCustLen);

		recordList.add(_recordList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)recordList.size());
	for(int _i = 0; _i < recordList.size(); _i++) { 
		_buf.putInt(recordList.get(_i).GetBufSize());
	recordList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)42);
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

