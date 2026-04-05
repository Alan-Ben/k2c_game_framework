using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p037_GuildDungeonOp
{

/// <summary>
/// 设置副本自动开启
/// </summary>
public class GC2GS_037_002_ReqSetAutoStartDungeon : ALBasicProtocolPack._IALProtocolStructure {
private List<long> autoStartDungeonIdList;
private int hour;
private int min;


public GC2GS_037_002_ReqSetAutoStartDungeon() {
	autoStartDungeonIdList = new List<long>();
	hour = 0;
	min = 0;
}

public GC2GS_037_002_ReqSetAutoStartDungeon(
	List<long> _autoStartDungeonIdList
	, int _hour
	, int _min
) {	autoStartDungeonIdList = _autoStartDungeonIdList;
	hour = _hour;
	min = _min;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)2; }

public List<long> getAutoStartDungeonIdList() { return autoStartDungeonIdList; }
public void addAutoStartDungeonIdList(long _autoStartDungeonIdList) { autoStartDungeonIdList.Add(_autoStartDungeonIdList); }
public int getHour() { return hour; }
public void setHour(int _hour) { hour = _hour; }
public int getMin() { return min; }
public void setMin(int _min) { min = _min; }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (autoStartDungeonIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (autoStartDungeonIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _autoStartDungeonIdListCount = _buf.getShort();
	for(int _i = 0; _i < _autoStartDungeonIdListCount; _i++) { 
		long _autoStartDungeonIdList = (long)0;
		_autoStartDungeonIdList = _buf.getLong();
		autoStartDungeonIdList.Add(_autoStartDungeonIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hour = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	min = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)autoStartDungeonIdList.Count);
	for(int _i = 0; _i < autoStartDungeonIdList.Count; _i++) { 
		_buf.putLong(autoStartDungeonIdList[_i]);
	}
	_buf.putInt(hour);
	_buf.putInt(min);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)2);
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
	builder.Append("autoStartDungeonIdList").Append(":").Append(autoStartDungeonIdList.ToString()).Append(", ");
	builder.Append("hour").Append(":").Append(hour.ToString()).Append(", ");
	builder.Append("min").Append(":").Append(min.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

