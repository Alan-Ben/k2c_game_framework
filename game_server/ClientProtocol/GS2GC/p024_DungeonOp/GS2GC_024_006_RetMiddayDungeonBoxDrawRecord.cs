using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_006_RetMiddayDungeonBoxDrawRecord : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 领取记录列表
/// </summary>
private List<Common.DungeonObj.MiddayDungeon_DrawRecord> drawRecordList;


public GS2GC_024_006_RetMiddayDungeonBoxDrawRecord() {
	drawRecordList = new List<Common.DungeonObj.MiddayDungeon_DrawRecord>();
}

public GS2GC_024_006_RetMiddayDungeonBoxDrawRecord(
	List<Common.DungeonObj.MiddayDungeon_DrawRecord> _drawRecordList
) {	drawRecordList = _drawRecordList;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 领取记录列表
/// </summary>
public List<Common.DungeonObj.MiddayDungeon_DrawRecord> getDrawRecordList() { return drawRecordList; }
/// <summary>
/// 领取记录列表
/// </summary>
public void addDrawRecordList(Common.DungeonObj.MiddayDungeon_DrawRecord _drawRecordList) { drawRecordList.Add(_drawRecordList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (drawRecordList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (drawRecordList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _drawRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _drawRecordListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_DrawRecord _drawRecordList = new Common.DungeonObj.MiddayDungeon_DrawRecord();
		int __drawRecordListCustLen = _buf.getInt();
	int __drawRecordListCurPos = _buf.getCurPos();
	_drawRecordList.ReadUnzipBuf(_buf, __drawRecordListCurPos + __drawRecordListCustLen);
	_buf.setPosition(__drawRecordListCurPos + __drawRecordListCustLen);

		drawRecordList.Add(_drawRecordList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)drawRecordList.Count);
	for(int _i = 0; _i < drawRecordList.Count; _i++) { 
		_buf.putInt(drawRecordList[_i].GetBufSize());
	drawRecordList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)6);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("drawRecordList").Append(":").Append(drawRecordList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

