using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_065_OnEventRecordChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.PlayerObj.Player_EventRecordInfo record;


public GS2GC_004_065_OnEventRecordChg() {
	record = new Common.PlayerObj.Player_EventRecordInfo();
}

public GS2GC_004_065_OnEventRecordChg(
	Common.PlayerObj.Player_EventRecordInfo _record
) {	record = _record;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)65; }

public Common.PlayerObj.Player_EventRecordInfo getRecord() { return record; }
public void setRecord(Common.PlayerObj.Player_EventRecordInfo _record) { record = _record; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _recordCustLen = _buf.getInt();
	int _recordCurPos = _buf.getCurPos();
	record.ReadUnzipBuf(_buf, _recordCurPos + _recordCustLen);
	_buf.setPosition(_recordCurPos + _recordCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(record.GetBufSize());
	record.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)65);
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
	builder.Append("record").Append(":").Append(record == null ? "null" : record.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

