using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_BattleLordInfo : ALBasicProtocolPack._IALProtocolStructure {
private int level;
private List<NPCommon.NPCommon_SkillLevel> skillLevels;
private List<long> propertyBonus;


public NPCommon_BattleLordInfo() {
	level = 0;
	skillLevels = new List<NPCommon.NPCommon_SkillLevel>();
	propertyBonus = new List<long>();
}

public NPCommon_BattleLordInfo(
	int _level
	, List<NPCommon.NPCommon_SkillLevel> _skillLevels
	, List<long> _propertyBonus
) {	level = _level;
	skillLevels = _skillLevels;
	propertyBonus = _propertyBonus;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public List<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.Add(_skillLevels); }
public List<long> getPropertyBonus() { return propertyBonus; }
public void addPropertyBonus(long _propertyBonus) { propertyBonus.Add(_propertyBonus); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (skillLevels.Count * 16);
	_size += 2 + (propertyBonus.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (skillLevels.Count * 16);
	_size += 2 + (propertyBonus.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
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
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(level);
	_buf.putShort((short)skillLevels.Count);
	for(int _i = 0; _i < skillLevels.Count; _i++) { 
		_buf.putInt(skillLevels[_i].GetBufSize());
	skillLevels[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)propertyBonus.Count);
	for(int _i = 0; _i < propertyBonus.Count; _i++) { 
		_buf.putLong(propertyBonus[_i]);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("skillLevels").Append(":").Append(skillLevels.ToString()).Append(", ");
	builder.Append("propertyBonus").Append(":").Append(propertyBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

