using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作攻击日志
/// </summary>
public class GuildCooperate_AttackLog : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据ID
/// </summary>
private long dbId;
/// <summary>
/// 攻击时间 ms
/// </summary>
private long timeMs;
/// <summary>
/// 攻击者名称
/// </summary>
private string name;
/// <summary>
/// 奖励据点ID
/// </summary>
private long posId;
/// <summary>
/// 属性
/// </summary>
private CommonEnum.ESpecAttrType attr;
/// <summary>
/// 攻击血量
/// </summary>
private long attackHp;


public GuildCooperate_AttackLog() {
	dbId = (long)0;
	timeMs = (long)0;
	name = "";
	posId = (long)0;
	attr = 0;
	attackHp = (long)0;
}

public GuildCooperate_AttackLog(
	long _dbId
	, long _timeMs
	, string _name
	, long _posId
	, CommonEnum.ESpecAttrType _attr
	, long _attackHp
) {	dbId = _dbId;
	timeMs = _timeMs;
	name = _name;
	posId = _posId;
	attr = _attr;
	attackHp = _attackHp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据ID
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据ID
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 攻击时间 ms
/// </summary>
public long getTimeMs() { return timeMs; }
/// <summary>
/// 攻击时间 ms
/// </summary>
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }
/// <summary>
/// 攻击者名称
/// </summary>
public string getName() { return name; }
/// <summary>
/// 攻击者名称
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 奖励据点ID
/// </summary>
public long getPosId() { return posId; }
/// <summary>
/// 奖励据点ID
/// </summary>
public void setPosId(long _posId) { posId = _posId; }
/// <summary>
/// 属性
/// </summary>
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/// <summary>
/// 属性
/// </summary>
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/// <summary>
/// 攻击血量
/// </summary>
public long getAttackHp() { return attackHp; }
/// <summary>
/// 攻击血量
/// </summary>
public void setAttackHp(long _attackHp) { attackHp = _attackHp; }


public int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attr = (CommonEnum.ESpecAttrType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackHp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(timeMs);
	_buf.putString(name);
	_buf.putLong(posId);
	_buf.putInt((int)attr);

	_buf.putLong(attackHp);
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
	builder.Append("timeMs").Append(":").Append(timeMs.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("posId").Append(":").Append(posId.ToString()).Append(", ");
	builder.Append("attr").Append(":").Append(attr.ToString()).Append(", ");
	builder.Append("attackHp").Append(":").Append(attackHp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

