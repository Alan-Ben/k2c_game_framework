package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_HeroCardInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
private long refId;
private int level;
private int starNum;
private short quality;
private java.util.ArrayList<Integer> prop_add;
private java.util.ArrayList<NPCommon.NPCommon_SkillLevel> skillLevels;
private int viewTimeTag;
private int getTimeTagS;


public NPCommon_HeroCardInfo() {
	dbId = (long)0;
	refId = (long)0;
	level = 0;
	starNum = 0;
	quality = (short)0;
	prop_add = new java.util.ArrayList<Integer>();
	skillLevels = new java.util.ArrayList<NPCommon.NPCommon_SkillLevel>();
	viewTimeTag = 0;
	getTimeTagS = 0;
}

public NPCommon_HeroCardInfo(
	 long _dbId
	, long _refId
	, int _level
	, int _starNum
	, short _quality
	, java.util.ArrayList<Integer> _prop_add
	, java.util.ArrayList<NPCommon.NPCommon_SkillLevel> _skillLevels
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

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
public java.util.ArrayList<Integer> getProp_add() { return prop_add; }
public void addProp_add(int _prop_add) { prop_add.add(_prop_add); }
public java.util.ArrayList<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.add(_skillLevels); }
public int getViewTimeTag() { return viewTimeTag; }
public void setViewTimeTag(int _viewTimeTag) { viewTimeTag = _viewTimeTag; }
public int getGetTimeTagS() { return getTimeTagS; }
public void setGetTimeTagS(int _getTimeTagS) { getTimeTagS = _getTimeTagS; }


public final int GetBufSize() {
	int _size = 34;
	_size += 2 + (prop_add.size() * 4);
	_size += 2 + (skillLevels.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 36;
	_size += 2 + (prop_add.size() * 4);
	_size += 2 + (skillLevels.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _prop_addCount = _buf.getShort();
	for(int _i = 0; _i < _prop_addCount; _i++) { 
		int _prop_add = 0;
		if(_buf.remaining() > 0) _prop_add = _buf.getInt();
		prop_add.add(_prop_add);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _skillLevelsCount = _buf.getShort();
	for(int _i = 0; _i < _skillLevelsCount; _i++) { 
		NPCommon.NPCommon_SkillLevel _skillLevels = new NPCommon.NPCommon_SkillLevel();
		if(_buf.remaining() <= 0) return;
	int __skillLevelsCustLen = _buf.getInt();
	int __skillLevelsCurPos = _buf.position();
	_skillLevels.ReadUnzipBuf(_buf, __skillLevelsCurPos + __skillLevelsCustLen);
	_buf.position(__skillLevelsCurPos + __skillLevelsCustLen);

		skillLevels.add(_skillLevels);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) viewTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) getTimeTagS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putInt(level);
	_buf.putInt(starNum);
	_buf.putShort(quality);
	_buf.putShort((short)prop_add.size());
	for(int _i = 0; _i < prop_add.size(); _i++) { 
		_buf.putInt(prop_add.get(_i));
	}
	_buf.putShort((short)skillLevels.size());
	for(int _i = 0; _i < skillLevels.size(); _i++) { 
		_buf.putInt(skillLevels.get(_i).GetBufSize());
	skillLevels.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(viewTimeTag);
	_buf.putInt(getTimeTagS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

