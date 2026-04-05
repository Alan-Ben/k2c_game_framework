using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 战斗怪物血量信息
/// </summary>
public class NPCommon_BattleMobHp : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// mission_lineup 配表id
/// </summary>
private int lineupMobId;
/// <summary>
/// 血量万分比
/// </summary>
private short hp;
/// <summary>
/// 最大血量
/// </summary>
private long hpMax;


public NPCommon_BattleMobHp() {
	lineupMobId = 0;
	hp = (short)0;
	hpMax = (long)0;
}

public NPCommon_BattleMobHp(
	int _lineupMobId
	, short _hp
	, long _hpMax
) {	lineupMobId = _lineupMobId;
	hp = _hp;
	hpMax = _hpMax;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// mission_lineup 配表id
/// </summary>
public int getLineupMobId() { return lineupMobId; }
/// <summary>
/// mission_lineup 配表id
/// </summary>
public void setLineupMobId(int _lineupMobId) { lineupMobId = _lineupMobId; }
/// <summary>
/// 血量万分比
/// </summary>
public short getHp() { return hp; }
/// <summary>
/// 血量万分比
/// </summary>
public void setHp(short _hp) { hp = _hp; }
/// <summary>
/// 最大血量
/// </summary>
public long getHpMax() { return hpMax; }
/// <summary>
/// 最大血量
/// </summary>
public void setHpMax(long _hpMax) { hpMax = _hpMax; }


public int GetBufSize() {
	int _size = 14;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lineupMobId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hp = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hpMax = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lineupMobId);
	_buf.putShort(hp);
	_buf.putLong(hpMax);
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
	builder.Append("lineupMobId").Append(":").Append(lineupMobId.ToString()).Append(", ");
	builder.Append("hp").Append(":").Append(hp.ToString()).Append(", ");
	builder.Append("hpMax").Append(":").Append(hpMax.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

