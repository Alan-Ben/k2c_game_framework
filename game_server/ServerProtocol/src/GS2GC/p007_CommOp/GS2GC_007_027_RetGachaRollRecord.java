package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_027_RetGachaRollRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 记录列表 */
private java.util.ArrayList<Common.GachaObj.Gacha_RecordInfo> recordList;


public GS2GC_007_027_RetGachaRollRecord() {
	recordList = new java.util.ArrayList<Common.GachaObj.Gacha_RecordInfo>();
}

public GS2GC_007_027_RetGachaRollRecord(
	 java.util.ArrayList<Common.GachaObj.Gacha_RecordInfo> _recordList
) {	recordList = _recordList;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)27; }

/** 记录列表 */
public java.util.ArrayList<Common.GachaObj.Gacha_RecordInfo> getRecordList() { return recordList; }
/** 记录列表 */
public void addRecordList(Common.GachaObj.Gacha_RecordInfo _recordList) { recordList.add(_recordList); }


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
		Common.GachaObj.Gacha_RecordInfo _recordList = new Common.GachaObj.Gacha_RecordInfo();
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
	_buf.put((byte)7);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)27);
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

