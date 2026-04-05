using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_LineupSettingInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阵型配置id
/// </summary>
private long layoutRefId;
private List<NPCommon.NPCommon_LineupInfo> lineupList;


public NPCommon_LineupSettingInfo() {
	layoutRefId = (long)0;
	lineupList = new List<NPCommon.NPCommon_LineupInfo>();
}

public NPCommon_LineupSettingInfo(
	long _layoutRefId
	, List<NPCommon.NPCommon_LineupInfo> _lineupList
) {	layoutRefId = _layoutRefId;
	lineupList = _lineupList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阵型配置id
/// </summary>
public long getLayoutRefId() { return layoutRefId; }
/// <summary>
/// 阵型配置id
/// </summary>
public void setLayoutRefId(long _layoutRefId) { layoutRefId = _layoutRefId; }
public List<NPCommon.NPCommon_LineupInfo> getLineupList() { return lineupList; }
public void addLineupList(NPCommon.NPCommon_LineupInfo _lineupList) { lineupList.Add(_lineupList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (lineupList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (lineupList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	layoutRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _lineupListCount = _buf.getShort();
	for(int _i = 0; _i < _lineupListCount; _i++) { 
		NPCommon.NPCommon_LineupInfo _lineupList = new NPCommon.NPCommon_LineupInfo();
		int __lineupListCustLen = _buf.getInt();
	int __lineupListCurPos = _buf.getCurPos();
	_lineupList.ReadUnzipBuf(_buf, __lineupListCurPos + __lineupListCustLen);
	_buf.setPosition(__lineupListCurPos + __lineupListCustLen);

		lineupList.Add(_lineupList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(layoutRefId);
	_buf.putShort((short)lineupList.Count);
	for(int _i = 0; _i < lineupList.Count; _i++) { 
		_buf.putInt(lineupList[_i].GetBufSize());
	lineupList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("layoutRefId").Append(":").Append(layoutRefId.ToString()).Append(", ");
	builder.Append("lineupList").Append(":").Append(lineupList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

