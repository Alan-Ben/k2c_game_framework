package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_BattleLordInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int level;
private java.util.ArrayList<NPCommon.NPCommon_SkillLevel> skillLevels;
private java.util.ArrayList<Long> propertyBonus;


public NPCommon_BattleLordInfo() {
	level = 0;
	skillLevels = new java.util.ArrayList<NPCommon.NPCommon_SkillLevel>();
	propertyBonus = new java.util.ArrayList<Long>();
}

public NPCommon_BattleLordInfo(
	 int _level
	, java.util.ArrayList<NPCommon.NPCommon_SkillLevel> _skillLevels
	, java.util.ArrayList<Long> _propertyBonus
) {	level = _level;
	skillLevels = _skillLevels;
	propertyBonus = _propertyBonus;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public java.util.ArrayList<NPCommon.NPCommon_SkillLevel> getSkillLevels() { return skillLevels; }
public void addSkillLevels(NPCommon.NPCommon_SkillLevel _skillLevels) { skillLevels.add(_skillLevels); }
public java.util.ArrayList<Long> getPropertyBonus() { return propertyBonus; }
public void addPropertyBonus(long _propertyBonus) { propertyBonus.add(_propertyBonus); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (skillLevels.size() * 16);
	_size += 2 + (propertyBonus.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (skillLevels.size() * 16);
	_size += 2 + (propertyBonus.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
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
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putShort((short)skillLevels.size());
	for(int _i = 0; _i < skillLevels.size(); _i++) { 
		_buf.putInt(skillLevels.get(_i).GetBufSize());
	skillLevels.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)propertyBonus.size());
	for(int _i = 0; _i < propertyBonus.size(); _i++) { 
		_buf.putLong(propertyBonus.get(_i));
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

