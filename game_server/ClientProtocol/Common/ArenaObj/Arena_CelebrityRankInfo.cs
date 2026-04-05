using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场名人榜数据
/// </summary>
public class Arena_CelebrityRankInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 攻击者CID
/// </summary>
private long attackerCid;
/// <summary>
/// 攻击者名字
/// </summary>
private string attackerName;
/// <summary>
/// 防守者名字
/// </summary>
private string defenderName;
/// <summary>
/// 击败大臣数量
/// </summary>
private int defeatHeroNum;
/// <summary>
/// 是否指定攻击
/// </summary>
private bool isSelectAttack;
/// <summary>
/// 发生时间戳
/// </summary>
private long timeMs;


public Arena_CelebrityRankInfo() {
	dbId = (long)0;
	attackerCid = (long)0;
	attackerName = "";
	defenderName = "";
	defeatHeroNum = 0;
	isSelectAttack = false;
	timeMs = (long)0;
}

public Arena_CelebrityRankInfo(
	long _dbId
	, long _attackerCid
	, string _attackerName
	, string _defenderName
	, int _defeatHeroNum
	, bool _isSelectAttack
	, long _timeMs
) {	dbId = _dbId;
	attackerCid = _attackerCid;
	attackerName = _attackerName;
	defenderName = _defenderName;
	defeatHeroNum = _defeatHeroNum;
	isSelectAttack = _isSelectAttack;
	timeMs = _timeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 攻击者CID
/// </summary>
public long getAttackerCid() { return attackerCid; }
/// <summary>
/// 攻击者CID
/// </summary>
public void setAttackerCid(long _attackerCid) { attackerCid = _attackerCid; }
/// <summary>
/// 攻击者名字
/// </summary>
public string getAttackerName() { return attackerName; }
/// <summary>
/// 攻击者名字
/// </summary>
public void setAttackerName(string _attackerName) { attackerName = _attackerName; }
/// <summary>
/// 防守者名字
/// </summary>
public string getDefenderName() { return defenderName; }
/// <summary>
/// 防守者名字
/// </summary>
public void setDefenderName(string _defenderName) { defenderName = _defenderName; }
/// <summary>
/// 击败大臣数量
/// </summary>
public int getDefeatHeroNum() { return defeatHeroNum; }
/// <summary>
/// 击败大臣数量
/// </summary>
public void setDefeatHeroNum(int _defeatHeroNum) { defeatHeroNum = _defeatHeroNum; }
/// <summary>
/// 是否指定攻击
/// </summary>
public bool getIsSelectAttack() { return isSelectAttack; }
/// <summary>
/// 是否指定攻击
/// </summary>
public void setIsSelectAttack(bool _isSelectAttack) { isSelectAttack = _isSelectAttack; }
/// <summary>
/// 发生时间戳
/// </summary>
public long getTimeMs() { return timeMs; }
/// <summary>
/// 发生时间戳
/// </summary>
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }


public int GetBufSize() {
	int _size = 29;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defenderName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defenderName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	defenderName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	defeatHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSelectAttack = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(attackerCid);
	_buf.putString(attackerName);
	_buf.putString(defenderName);
	_buf.putInt(defeatHeroNum);
	_buf.put(isSelectAttack?(byte)1:(byte)0);
	_buf.putLong(timeMs);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("attackerCid").Append(":").Append(attackerCid.ToString()).Append(", ");
	builder.Append("attackerName").Append(":").Append(attackerName.ToString()).Append(", ");
	builder.Append("defenderName").Append(":").Append(defenderName.ToString()).Append(", ");
	builder.Append("defeatHeroNum").Append(":").Append(defeatHeroNum.ToString()).Append(", ");
	builder.Append("isSelectAttack").Append(":").Append(isSelectAttack.ToString()).Append(", ");
	builder.Append("timeMs").Append(":").Append(timeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

