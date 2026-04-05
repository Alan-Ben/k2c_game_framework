package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 带入战斗的宠物信息
 **/
public class NPCommon_BattleCardInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宠物实例id */
private long petInstanceId;
/** 宠物配表id */
private long petRefId;
/** 宠物等级 */
private int level;
/** 宠物星级配置 */
private long starRefId;
/** 宠物品质 */
private short quality;
private long skinId;
private java.util.ArrayList<NPCommon.NPCommon_SkillLevel> skillLevels;
private java.util.ArrayList<Long> propertyBonus;
/** 宠物资质品质 */
private NPEnum.ENPAttrGrade attrGrade;
/** 资质列表 */
private java.util.ArrayList<Integer> attrList;


public NPCommon_BattleCardInfo() {
	petInstanceId = (long)0;
	petRefId = (long)0;
	level = 0;
	starRefId = (long)0;
	quality = (short)0;
	skinId = (long)0;
	skillLevels = new java.util.ArrayList<NPCommon.NPCommon_SkillLevel>();
	propertyBonus = new java.util.ArrayList<Long>();
	attrGrade = NPEnum.ENPAttrGrade.values()[0];
	attrList = new java.util.ArrayList<Integer>();
}

public NPCommon_BattleCardInfo(
	 long _petInstanceId
	, long _petRefId
	, int _level
	, long _starRefId
	, short _quality
	, long _skinId
	, java.util.ArrayList<NPCommon.NPCommon_SkillLevel> _skillLevels
	, java.util.ArrayList<Long> _propertyBonus
	, NPEnum.ENPAttrGrade _attrGrade
	, java.util.ArrayList<Integer> _attrList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宠物实例id */
public long getPetInstanceId() { return petInstanceId; }
/** 宠物实例id */
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/** 宠物配表id */
public long getPetRefId() { return petRefId; }
/** 宠物配表id */
public void setPetRefId(long _petRefId) { petRefId = _petRefId; }
/** 宠物等级 */
public int getLevel() { return level; }
/** 宠物等级 */
public void setLevel(int _level) { level = _level; }
/** 宠物星级配置 */
public long getStarRefId() { return starRefId; }
/** 宠物星级配置 */
public void setStarRefId(long _starRefId) { starRefId = _starRefId; }
/** 宠物品质 */
public short getQuality() { return quality; }
/** 宠物品质 */
public void setQuality(short _quality) { quality = _quality; }
public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }
public java.util.ArrayList<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.add(_skillLevels); }
public java.util.ArrayList<Long> getPropertyBonus() { return propertyBonus; }
public void addPropertyBonus(long _propertyBonus) { propertyBonus.add(_propertyBonus); }
/** 宠物资质品质 */
public NPEnum.ENPAttrGrade getAttrGrade() { return attrGrade; }
/** 宠物资质品质 */
public void setAttrGrade(NPEnum.ENPAttrGrade _attrGrade) { attrGrade = _attrGrade; }
/** 资质列表 */
public java.util.ArrayList<Integer> getAttrList() { return attrList; }
/** 资质列表 */
public void addAttrList(int _attrList) { attrList.add(_attrList); }


public final int GetBufSize() {
	int _size = 42;
	_size += 2 + (skillLevels.size() * 16);
	_size += 2 + (propertyBonus.size() * 8);
	_size += 2 + (attrList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 44;
	_size += 2 + (skillLevels.size() * 16);
	_size += 2 + (propertyBonus.size() * 8);
	_size += 2 + (attrList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) starRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
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
	if(_buf.remaining() <= 0) return;
	short _propertyBonusCount = _buf.getShort();
	for(int _i = 0; _i < _propertyBonusCount; _i++) { 
		long _propertyBonus = (long)0;
		if(_buf.remaining() > 0) _propertyBonus = _buf.getLong();
		propertyBonus.add(_propertyBonus);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attrGrade = NPEnum.ENPAttrGrade.ENPAttrGrade_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _attrListCount = _buf.getShort();
	for(int _i = 0; _i < _attrListCount; _i++) { 
		int _attrList = 0;
		if(_buf.remaining() > 0) _attrList = _buf.getInt();
		attrList.add(_attrList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(petRefId);
	_buf.putInt(level);
	_buf.putLong(starRefId);
	_buf.putShort(quality);
	_buf.putLong(skinId);
	_buf.putShort((short)skillLevels.size());
	for(int _i = 0; _i < skillLevels.size(); _i++) { 
		_buf.putInt(skillLevels.get(_i).GetBufSize());
	skillLevels.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)propertyBonus.size());
	for(int _i = 0; _i < propertyBonus.size(); _i++) { 
		_buf.putLong(propertyBonus.get(_i));
	}
	_buf.putInt(attrGrade.ordinal());

	_buf.putShort((short)attrList.size());
	for(int _i = 0; _i < attrList.size(); _i++) { 
		_buf.putInt(attrList.get(_i));
	}
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

