using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-战斗NPC信息
/// </summary>
public class Mars_ExploreBattleNPCInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 原士兵数
/// </summary>
private long oriSoldierNum;
/// <summary>
/// 受伤士兵数
/// </summary>
private long hurtSoldierNum;
/// <summary>
/// 单个士兵战力
/// </summary>
private long singleSoldierPower;


public Mars_ExploreBattleNPCInfo() {
	oriSoldierNum = (long)0;
	hurtSoldierNum = (long)0;
	singleSoldierPower = (long)0;
}

public Mars_ExploreBattleNPCInfo(
	long _oriSoldierNum
	, long _hurtSoldierNum
	, long _singleSoldierPower
) {	oriSoldierNum = _oriSoldierNum;
	hurtSoldierNum = _hurtSoldierNum;
	singleSoldierPower = _singleSoldierPower;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 原士兵数
/// </summary>
public long getOriSoldierNum() { return oriSoldierNum; }
/// <summary>
/// 原士兵数
/// </summary>
public void setOriSoldierNum(long _oriSoldierNum) { oriSoldierNum = _oriSoldierNum; }
/// <summary>
/// 受伤士兵数
/// </summary>
public long getHurtSoldierNum() { return hurtSoldierNum; }
/// <summary>
/// 受伤士兵数
/// </summary>
public void setHurtSoldierNum(long _hurtSoldierNum) { hurtSoldierNum = _hurtSoldierNum; }
/// <summary>
/// 单个士兵战力
/// </summary>
public long getSingleSoldierPower() { return singleSoldierPower; }
/// <summary>
/// 单个士兵战力
/// </summary>
public void setSingleSoldierPower(long _singleSoldierPower) { singleSoldierPower = _singleSoldierPower; }


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
	oriSoldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hurtSoldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	singleSoldierPower = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oriSoldierNum);
	_buf.putLong(hurtSoldierNum);
	_buf.putLong(singleSoldierPower);
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
	builder.Append("oriSoldierNum").Append(":").Append(oriSoldierNum.ToString()).Append(", ");
	builder.Append("hurtSoldierNum").Append(":").Append(hurtSoldierNum.ToString()).Append(", ");
	builder.Append("singleSoldierPower").Append(":").Append(singleSoldierPower.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

