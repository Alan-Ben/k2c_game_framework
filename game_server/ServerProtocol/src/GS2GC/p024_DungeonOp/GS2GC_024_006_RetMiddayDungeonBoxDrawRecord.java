package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_006_RetMiddayDungeonBoxDrawRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 领取记录列表 */
private java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> drawRecordList;


public GS2GC_024_006_RetMiddayDungeonBoxDrawRecord() {
	drawRecordList = new java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord>();
}

public GS2GC_024_006_RetMiddayDungeonBoxDrawRecord(
	 java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> _drawRecordList
) {	drawRecordList = _drawRecordList;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)6; }

/** 领取记录列表 */
public java.util.ArrayList<Common.DungeonObj.MiddayDungeon_DrawRecord> getDrawRecordList() { return drawRecordList; }
/** 领取记录列表 */
public void addDrawRecordList(Common.DungeonObj.MiddayDungeon_DrawRecord _drawRecordList) { drawRecordList.add(_drawRecordList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (drawRecordList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (drawRecordList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _drawRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _drawRecordListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_DrawRecord _drawRecordList = new Common.DungeonObj.MiddayDungeon_DrawRecord();
		if(_buf.remaining() <= 0) return;
	int __drawRecordListCustLen = _buf.getInt();
	int __drawRecordListCurPos = _buf.position();
	_drawRecordList.ReadUnzipBuf(_buf, __drawRecordListCurPos + __drawRecordListCustLen);
	_buf.position(__drawRecordListCurPos + __drawRecordListCustLen);

		drawRecordList.add(_drawRecordList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)drawRecordList.size());
	for(int _i = 0; _i < drawRecordList.size(); _i++) { 
		_buf.putInt(drawRecordList.get(_i).GetBufSize());
	drawRecordList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)6);
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

