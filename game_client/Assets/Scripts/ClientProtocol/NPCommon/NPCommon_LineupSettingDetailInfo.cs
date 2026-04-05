using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_LineupSettingDetailInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private string lineupName;
private string lineupDesc;
private NPCommon.NPCommon_LineupSettingInfo lineupSettingInfo;


public NPCommon_LineupSettingDetailInfo() {
	id = (long)0;
	lineupName = "";
	lineupDesc = "";
	lineupSettingInfo = new NPCommon.NPCommon_LineupSettingInfo();
}

public NPCommon_LineupSettingDetailInfo(
	long _id
	, string _lineupName
	, string _lineupDesc
	, NPCommon.NPCommon_LineupSettingInfo _lineupSettingInfo
) {	id = _id;
	lineupName = _lineupName;
	lineupDesc = _lineupDesc;
	lineupSettingInfo = _lineupSettingInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public string getLineupName() { return lineupName; }
public void setLineupName(string _lineupName) { lineupName = _lineupName; }
public string getLineupDesc() { return lineupDesc; }
public void setLineupDesc(string _lineupDesc) { lineupDesc = _lineupDesc; }
public NPCommon.NPCommon_LineupSettingInfo getLineupSettingInfo() { return lineupSettingInfo; }
public void setLineupSettingInfo(NPCommon.NPCommon_LineupSettingInfo _lineupSettingInfo) { lineupSettingInfo = _lineupSettingInfo; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupDesc);
	_size += 4 + lineupSettingInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupDesc);
	_size += 4 + lineupSettingInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lineupName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lineupDesc = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupSettingInfoCustLen = _buf.getInt();
	int _lineupSettingInfoCurPos = _buf.getCurPos();
	lineupSettingInfo.ReadUnzipBuf(_buf, _lineupSettingInfoCurPos + _lineupSettingInfoCustLen);
	_buf.setPosition(_lineupSettingInfoCurPos + _lineupSettingInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putString(lineupName);
	_buf.putString(lineupDesc);
	_buf.putInt(lineupSettingInfo.GetBufSize());
	lineupSettingInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("lineupName").Append(":").Append(lineupName.ToString()).Append(", ");
	builder.Append("lineupDesc").Append(":").Append(lineupDesc.ToString()).Append(", ");
	builder.Append("lineupSettingInfo").Append(":").Append(lineupSettingInfo == null ? "null" : lineupSettingInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

