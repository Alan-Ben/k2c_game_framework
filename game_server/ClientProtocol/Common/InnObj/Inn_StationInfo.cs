using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_设施信息
/// </summary>
public class Inn_StationInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 设施ID
/// </summary>
private long stationId;
/// <summary>
/// 等级
/// </summary>
private int level;


public Inn_StationInfo() {
	stationId = (long)0;
	level = 0;
}

public Inn_StationInfo(
	long _stationId
	, int _level
) {	stationId = _stationId;
	level = _level;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 设施ID
/// </summary>
public long getStationId() { return stationId; }
/// <summary>
/// 设施ID
/// </summary>
public void setStationId(long _stationId) { stationId = _stationId; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stationId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(stationId);
	_buf.putInt(level);
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
	builder.Append("stationId").Append(":").Append(stationId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

