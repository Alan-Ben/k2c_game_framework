using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 带入战斗的宠物信息
/// </summary>
public class NPCommon_BattleCardInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宠物实例id
/// </summary>
private long petInstanceId;
/// <summary>
/// 宠物配表id
/// </summary>
private long petRefId;
/// <summary>
/// 宠物等级
/// </summary>
private int level;
/// <summary>
/// 宠物星级配置
/// </summary>
private long starRefId;
/// <summary>
/// 宠物品质
/// </summary>
private short quality;
private long skinId;
private List<NPCommon.NPCommon_SkillLevel> skillLevels;
private List<long> propertyBonus;
/// <summary>
/// 宠物资质品质
/// </summary>
private NPEnum.ENPAttrGrade attrGrade;
/// <summary>
/// 资质列表
/// </summary>
private List<int> attrList;


public NPCommon_BattleCardInfo() {
	petInstanceId = (long)0;
	petRefId = (long)0;
	level = 0;
	starRefId = (long)0;
	quality = (short)0;
	skinId = (long)0;
	skillLevels = new List<NPCommon.NPCommon_SkillLevel>();
	propertyBonus = new List<long>();
	attrGrade = 0;
	attrList = new List<int>();
}

public NPCommon_BattleCardInfo(
	long _petInstanceId
	, long _petRefId
	, int _level
	, long _starRefId
	, short _quality
	, long _skinId
	, List<NPCommon.NPCommon_SkillLevel> _skillLevels
	, List<long> _propertyBonus
	, NPEnum.ENPAttrGrade _attrGrade
	, List<int> _attrList
) {	petInstanceId = _petInstanceId;
	petRefId = _petRefId;
	level = _level;
	starRefId = _starRefId;
	quality = _quality;
	skinId = _skinId;
	skillLevels = _skillLevels;
	propertyBonus = _propertyBonus;
	attrGrade = _attrGrade;
	attrList = _attrList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宠物实例id
/// </summary>
public long getPetInstanceId() { return petInstanceId; }
/// <summary>
/// 宠物实例id
/// </summary>
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/// <summary>
/// 宠物配表id
/// </summary>
public long getPetRefId() { return petRefId; }
/// <summary>
/// 宠物配表id
/// </summary>
public void setPetRefId(long _petRefId) { petRefId = _petRefId; }
/// <summary>
/// 宠物等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 宠物等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 宠物星级配置
/// </summary>
public long getStarRefId() { return starRefId; }
/// <summary>
/// 宠物星级配置
/// </summary>
public void setStarRefId(long _starRefId) { starRefId = _starRefId; }
/// <summary>
/// 宠物品质
/// </summary>
public short getQuality() { return quality; }
/// <summary>
/// 宠物品质
/// </summary>
public void setQuality(short _quality) { quality = _quality; }
public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }
public List<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.Add(_skillLevels); }
public List<long> getPropertyBonus() { return propertyBonus; }
public void addPropertyBonus(long _propertyBonus) { propertyBonus.Add(_propertyBonus); }
/// <summary>
/// 宠物资质品质
/// </summary>
public NPEnum.ENPAttrGrade getAttrGrade() { return attrGrade; }
/// <summary>
/// 宠物资质品质
/// </summary>
public void setAttrGrade(NPEnum.ENPAttrGrade _attrGrade) { attrGrade = _attrGrade; }
/// <summary>
/// 资质列表
/// </summary>
public List<int> getAttrList() { return attrList; }
/// <summary>
/// 资质列表
/// </summary>
public void addAttrList(int _attrList) { attrList.Add(_attrList); }


public int GetBufSize() {
	int _size = 42;
	_size += 2 + (skillLevels.Count * 16);
	_size += 2 + (propertyBonus.Count * 8);
	_size += 2 + (attrList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 44;
	_size += 2 + (skillLevels.Count * 16);
	_size += 2 + (propertyBonus.Count * 8);
	_size += 2 + (attrList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _skillLevelsCount = _buf.getShort();
	for(int _i = 0; _i < _skillLevelsCount; _i++) { 
		NPCommon.NPCommon_SkillLevel _skillLevels = new NPCommon.NPCommon_SkillLevel();
		int __skillLevelsCustLen = _buf.getInt();
	int __skillLevelsCurPos = _buf.getCurPos();
	_skillLevels.ReadUnzipBuf(_buf, __skillLevelsCurPos + __skillLevelsCustLen);
	_buf.setPosition(__skillLevelsCurPos + __skillLevelsCustLen);

		skillLevels.Add(_skillLevels);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _propertyBonusCount = _buf.getShort();
	for(int _i = 0; _i < _propertyBonusCount; _i++) { 
		long _propertyBonus = (long)0;
		_propertyBonus = _buf.getLong();
		propertyBonus.Add(_propertyBonus);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attrGrade = (NPEnum.ENPAttrGrade)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _attrListCount = _buf.getShort();
	for(int _i = 0; _i < _attrListCount; _i++) { 
		int _attrList = 0;
		_attrList = _buf.getInt();
		attrList.Add(_attrList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(petRefId);
	_buf.putInt(level);
	_buf.putLong(starRefId);
	_buf.putShort(quality);
	_buf.putLong(skinId);
	_buf.putShort((short)skillLevels.Count);
	for(int _i = 0; _i < skillLevels.Count; _i++) { 
		_buf.putInt(skillLevels[_i].GetBufSize());
	skillLevels[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)propertyBonus.Count);
	for(int _i = 0; _i < propertyBonus.Count; _i++) { 
		_buf.putLong(propertyBonus[_i]);
	}
	_buf.putInt((int)attrGrade);

	_buf.putShort((short)attrList.Count);
	for(int _i = 0; _i < attrList.Count; _i++) { 
		_buf.putInt(attrList[_i]);
	}
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
	builder.Append("petInstanceId").Append(":").Append(petInstanceId.ToString()).Append(", ");
	builder.Append("petRefId").Append(":").Append(petRefId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("starRefId").Append(":").Append(starRefId.ToString()).Append(", ");
	builder.Append("quality").Append(":").Append(quality.ToString()).Append(", ");
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("skillLevels").Append(":").Append(skillLevels.ToString()).Append(", ");
	builder.Append("propertyBonus").Append(":").Append(propertyBonus.ToString()).Append(", ");
	builder.Append("attrGrade").Append(":").Append(attrGrade.ToString()).Append(", ");
	builder.Append("attrList").Append(":").Append(attrList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

