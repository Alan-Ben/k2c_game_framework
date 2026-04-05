using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-奇物信息
/// </summary>
public class TreasureHunt_TreasureInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物ID
/// </summary>
private long treasureId;
/// <summary>
/// 技能等级
/// </summary>
private int skillLevel;
/// <summary>
/// 获得时间 ms
/// </summary>
private long gainTimeMs;


public TreasureHunt_TreasureInfo() {
	treasureId = (long)0;
	skillLevel = 0;
	gainTimeMs = (long)0;
}

public TreasureHunt_TreasureInfo(
	long _treasureId
	, int _skillLevel
	, long _gainTimeMs
) {	treasureId = _treasureId;
	skillLevel = _skillLevel;
	gainTimeMs = _gainTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 奇物ID
/// </summary>
public long getTreasureId() { return treasureId; }
/// <summary>
/// 奇物ID
/// </summary>
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/// <summary>
/// 技能等级
/// </summary>
public int getSkillLevel() { return skillLevel; }
/// <summary>
/// 技能等级
/// </summary>
public void setSkillLevel(int _skillLevel) { skillLevel = _skillLevel; }
/// <summary>
/// 获得时间 ms
/// </summary>
public long getGainTimeMs() { return gainTimeMs; }
/// <summary>
/// 获得时间 ms
/// </summary>
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(treasureId);
	_buf.putInt(skillLevel);
	_buf.putLong(gainTimeMs);
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
	builder.Append("treasureId").Append(":").Append(treasureId.ToString()).Append(", ");
	builder.Append("skillLevel").Append(":").Append(skillLevel.ToString()).Append(", ");
	builder.Append("gainTimeMs").Append(":").Append(gainTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

