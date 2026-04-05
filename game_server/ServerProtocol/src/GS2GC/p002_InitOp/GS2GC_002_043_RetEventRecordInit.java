package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_043_RetEventRecordInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据列表 */
private java.util.ArrayList<Common.PlayerObj.Player_EventRecordInfo> recordList;


public GS2GC_002_043_RetEventRecordInit() {
	recordList = new java.util.ArrayList<Common.PlayerObj.Player_EventRecordInfo>();
}

public GS2GC_002_043_RetEventRecordInit(
	 java.util.ArrayList<Common.PlayerObj.Player_EventRecordInfo> _recordList
) {	recordList = _recordList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)43; }

/** 数据列表 */
public java.util.ArrayList<Common.PlayerObj.Player_EventRecordInfo> getRecordList() { return recordList; }
/** 数据列表 */
public void addRecordList(Common.PlayerObj.Player_EventRecordInfo _recordList) { recordList.add(_recordList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (recordList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (recordList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _recordListCount = _buf.getShort();
	for(int _i = 0; _i < _recordListCount; _i++) { 
		Common.PlayerObj.Player_EventRecordInfo _recordList = new Common.PlayerObj.Player_EventRecordInfo();
		if(_buf.remaining() <= 0) return;
	int __recordListCustLen = _buf.getInt();
	int __recordListCurPos = _buf.position();
	_recordList.ReadUnzipBuf(_buf, __recordListCurPos + __recordListCustLen);
	_buf.position(__recordListCurPos + __recordListCustLen);

		recordList.add(_recordList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)recordList.size());
	for(int _i = 0; _i < recordList.size(); _i++) { 
		_buf.putInt(recordList.get(_i).GetBufSize());
	recordList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)43);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)43);
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

