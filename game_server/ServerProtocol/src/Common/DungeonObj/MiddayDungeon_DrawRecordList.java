package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本_宝箱领取记录列表
 **/
public class MiddayDungeon_DrawRecordList implements ALBasicProtocolPack._IALProtocolStructure {
/** 记录列表 */
private java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> recordList;


public MiddayDungeon_DrawRecordList() {
	recordList = new java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord>();
}

public MiddayDungeon_DrawRecordList(
	 java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> _recordList
) {	recordList = _recordList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 记录列表 */
public java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> getRecordList() { return recordList; }
/** 记录列表 */
public void addRecordList(Common.DungeonObj.MiddayDungeon_DrawRecord _recordList) { recordList.add(_recordList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (recordList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (recordList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _recordListCount = _buf.getShort();
	for(int _i = 0; _i < _recordListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_DrawRecord _recordList = new Common.DungeonObj.MiddayDungeon_DrawRecord();
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
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

