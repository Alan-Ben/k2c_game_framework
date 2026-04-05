using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-太空舱信息
/// </summary>
public class TreasureHunt_StationInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 太空舱等级
/// </summary>
private int stationLevel;
/// <summary>
/// 经验值
/// </summary>
private long exp;


public TreasureHunt_StationInfo() {
	stationLevel = 0;
	exp = (long)0;
}

public TreasureHunt_StationInfo(
	int _stationLevel
	, long _exp
) {	stationLevel = _stationLevel;
	exp = _exp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 太空舱等级
/// </summary>
public int getStationLevel() { return stationLevel; }
/// <summary>
/// 太空舱等级
/// </summary>
public void setStationLevel(int _stationLevel) { stationLevel = _stationLevel; }
/// <summary>
/// 经验值
/// </summary>
public long getExp() { return exp; }
/// <summary>
/// 经验值
/// </summary>
public void setExp(long _exp) { exp = _exp; }


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
	stationLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stationLevel);
	_buf.putLong(exp);
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
	builder.Append("stationLevel").Append(":").Append(stationLevel.ToString()).Append(", ");
	builder.Append("exp").Append(":").Append(exp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

