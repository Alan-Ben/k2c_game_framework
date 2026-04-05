using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 藏品基础信息
/// </summary>
public class Equip_BaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 藏品id
/// </summary>
private long equipId;
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 觉醒等级
/// </summary>
private int awakenLevel;
/// <summary>
/// 穿戴大臣id
/// </summary>
private long wearHeroId;
/// <summary>
/// 总加成值
/// </summary>
private int skillAddValue;
/// <summary>
/// 是否锁定
/// </summary>
private bool isLock;


public Equip_BaseInfo() {
	dbId = (long)0;
	equipId = (long)0;
	level = 0;
	awakenLevel = 0;
	wearHeroId = (long)0;
	skillAddValue = 0;
	isLock = false;
}

public Equip_BaseInfo(
	long _dbId
	, long _equipId
	, int _level
	, int _awakenLevel
	, long _wearHeroId
	, int _skillAddValue
	, bool _isLock
) {	dbId = _dbId;
	equipId = _equipId;
	level = _level;
	awakenLevel = _awakenLevel;
	wearHeroId = _wearHeroId;
	skillAddValue = _skillAddValue;
	isLock = _isLock;
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
/// 藏品id
/// </summary>
public long getEquipId() { return equipId; }
/// <summary>
/// 藏品id
/// </summary>
public void setEquipId(long _equipId) { equipId = _equipId; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 觉醒等级
/// </summary>
public int getAwakenLevel() { return awakenLevel; }
/// <summary>
/// 觉醒等级
/// </summary>
public void setAwakenLevel(int _awakenLevel) { awakenLevel = _awakenLevel; }
/// <summary>
/// 穿戴大臣id
/// </summary>
public long getWearHeroId() { return wearHeroId; }
/// <summary>
/// 穿戴大臣id
/// </summary>
public void setWearHeroId(long _wearHeroId) { wearHeroId = _wearHeroId; }
/// <summary>
/// 总加成值
/// </summary>
public int getSkillAddValue() { return skillAddValue; }
/// <summary>
/// 总加成值
/// </summary>
public void setSkillAddValue(int _skillAddValue) { skillAddValue = _skillAddValue; }
/// <summary>
/// 是否锁定
/// </summary>
public bool getIsLock() { return isLock; }
/// <summary>
/// 是否锁定
/// </summary>
public void setIsLock(bool _isLock) { isLock = _isLock; }


public int GetBufSize() {
	int _size = 37;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	equipId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	awakenLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	wearHeroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillAddValue = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLock = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(equipId);
	_buf.putInt(level);
	_buf.putInt(awakenLevel);
	_buf.putLong(wearHeroId);
	_buf.putInt(skillAddValue);
	_buf.put(isLock?(byte)1:(byte)0);
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
	builder.Append("equipId").Append(":").Append(equipId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("awakenLevel").Append(":").Append(awakenLevel.ToString()).Append(", ");
	builder.Append("wearHeroId").Append(":").Append(wearHeroId.ToString()).Append(", ");
	builder.Append("skillAddValue").Append(":").Append(skillAddValue.ToString()).Append(", ");
	builder.Append("isLock").Append(":").Append(isLock.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

