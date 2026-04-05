using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-奇物等级变更
/// </summary>
public class GS2GC_036_052_OnTreasureHuntTreasureLevelChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物ID
/// </summary>
private long treasureId;
/// <summary>
/// 奇物等级
/// </summary>
private int level;


public GS2GC_036_052_OnTreasureHuntTreasureLevelChg() {
	treasureId = (long)0;
	level = 0;
}

public GS2GC_036_052_OnTreasureHuntTreasureLevelChg(
	long _treasureId
	, int _level
) {	treasureId = _treasureId;
	level = _level;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 奇物ID
/// </summary>
public long getTreasureId() { return treasureId; }
/// <summary>
/// 奇物ID
/// </summary>
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/// <summary>
/// 奇物等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 奇物等级
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
	treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(treasureId);
	_buf.putInt(level);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)52);
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
	builder.Append("treasureId").Append(":").Append(treasureId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

