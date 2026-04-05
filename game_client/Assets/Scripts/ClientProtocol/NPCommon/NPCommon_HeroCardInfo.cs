using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_HeroCardInfo : ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
private long refId;
private int level;
private int starNum;
private short quality;
private List<int> prop_add;
private List<NPCommon.NPCommon_SkillLevel> skillLevels;
private int viewTimeTag;
private int getTimeTagS;


public NPCommon_HeroCardInfo() {
	dbId = (long)0;
	refId = (long)0;
	level = 0;
	starNum = 0;
	quality = (short)0;
	prop_add = new List<int>();
	skillLevels = new List<NPCommon.NPCommon_SkillLevel>();
	viewTimeTag = 0;
	getTimeTagS = 0;
}

public NPCommon_HeroCardInfo(
	long _dbId
	, long _refId
	, int _level
	, int _starNum
	, short _quality
	, List<int> _prop_add
	, List<NPCommon.NPCommon_SkillLevel> _skillLevels
	, int _viewTimeTag
	, int _getTimeTagS
) {	dbId = _dbId;
	refId = _refId;
	level = _level;
	starNum = _starNum;
	quality = _quality;
	prop_add = _prop_add;
	skillLevels = _skillLevels;
	viewTimeTag = _viewTimeTag;
	getTimeTagS = _getTimeTagS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getDbId() { return dbId; }
public void setDbId(long _dbId) { dbId = _dbId; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public int getStarNum() { return starNum; }
public void setStarNum(int _starNum) { starNum = _starNum; }
public short getQuality() { return quality; }
public void setQuality(short _quality) { quality = _quality; }
public List<int> getProp_add() { return prop_add; }
public void addProp_add(int _prop_add) { prop_add.Add(_prop_add); }
public List<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.Add(_skillLevels); }
public int getViewTimeTag() { return viewTimeTag; }
public void setViewTimeTag(int _viewTimeTag) { viewTimeTag = _viewTimeTag; }
public int getGetTimeTagS() { return getTimeTagS; }
public void setGetTimeTagS(int _getTimeTagS) { getTimeTagS = _getTimeTagS; }


public int GetBufSize() {
	int _size = 34;
	_size += 2 + (prop_add.Count * 4);
	_size += 2 + (skillLevels.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 36;
	_size += 2 + (prop_add.Count * 4);
	_size += 2 + (skillLevels.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	starNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _prop_addCount = _buf.getShort();
	for(int _i = 0; _i < _prop_addCount; _i++) { 
		int _prop_add = 0;
		_prop_add = _buf.getInt();
		prop_add.Add(_prop_add);
	}
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
	viewTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	getTimeTagS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putInt(level);
	_buf.putInt(starNum);
	_buf.putShort(quality);
	_buf.putShort((short)prop_add.Count);
	for(int _i = 0; _i < prop_add.Count; _i++) { 
		_buf.putInt(prop_add[_i]);
	}
	_buf.putShort((short)skillLevels.Count);
	for(int _i = 0; _i < skillLevels.Count; _i++) { 
		_buf.putInt(skillLevels[_i].GetBufSize());
	skillLevels[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(viewTimeTag);
	_buf.putInt(getTimeTagS);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("starNum").Append(":").Append(starNum.ToString()).Append(", ");
	builder.Append("quality").Append(":").Append(quality.ToString()).Append(", ");
	builder.Append("prop_add").Append(":").Append(prop_add.ToString()).Append(", ");
	builder.Append("skillLevels").Append(":").Append(skillLevels.ToString()).Append(", ");
	builder.Append("viewTimeTag").Append(":").Append(viewTimeTag.ToString()).Append(", ");
	builder.Append("getTimeTagS").Append(":").Append(getTimeTagS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

