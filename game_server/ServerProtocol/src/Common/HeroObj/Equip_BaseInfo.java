package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 藏品基础信息
 **/
public class Equip_BaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 藏品id */
private long equipId;
/** 等级 */
private int level;
/** 觉醒等级 */
private int awakenLevel;
/** 穿戴大臣id */
private long wearHeroId;
/** 总加成值 */
private int skillAddValue;
/** 是否锁定 */
private boolean isLock;


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
	, boolean _isLock
) {	dbId = _dbId;
	equipId = _equipId;
	level = _level;
	awakenLevel = _awakenLevel;
	wearHeroId = _wearHeroId;
	skillAddValue = _skillAddValue;
	isLock = _isLock;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 藏品id */
public long getEquipId() { return equipId; }
/** 藏品id */
public void setEquipId(long _equipId) { equipId = _equipId; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 觉醒等级 */
public int getAwakenLevel() { return awakenLevel; }
/** 觉醒等级 */
public void setAwakenLevel(int _awakenLevel) { awakenLevel = _awakenLevel; }
/** 穿戴大臣id */
public long getWearHeroId() { return wearHeroId; }
/** 穿戴大臣id */
public void setWearHeroId(long _wearHeroId) { wearHeroId = _wearHeroId; }
/** 总加成值 */
public int getSkillAddValue() { return skillAddValue; }
/** 总加成值 */
public void setSkillAddValue(int _skillAddValue) { skillAddValue = _skillAddValue; }
/** 是否锁定 */
public boolean getIsLock() { return isLock; }
/** 是否锁定 */
public void setIsLock(boolean _isLock) { isLock = _isLock; }


public final int GetBufSize() {
	int _size = 37;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) equipId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) awakenLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) wearHeroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillAddValue = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLock = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(equipId);
	_buf.putInt(level);
	_buf.putInt(awakenLevel);
	_buf.putLong(wearHeroId);
	_buf.putInt(skillAddValue);
	_buf.put(isLock?(byte)1:(byte)0);
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

