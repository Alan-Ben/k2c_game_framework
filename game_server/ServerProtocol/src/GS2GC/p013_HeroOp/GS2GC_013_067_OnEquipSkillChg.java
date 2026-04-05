package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品技能信息变更
 **/
public class GS2GC_013_067_OnEquipSkillChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 藏品数据id */
private long dbId;
private Common.HeroObj.Equip_SkillInfo skillInfo;


public GS2GC_013_067_OnEquipSkillChg() {
	dbId = (long)0;
	skillInfo = new Common.HeroObj.Equip_SkillInfo();
}

public GS2GC_013_067_OnEquipSkillChg(
	 long _dbId
	, Common.HeroObj.Equip_SkillInfo _skillInfo
) {	dbId = _dbId;
	skillInfo = _skillInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)67; }

/** 藏品数据id */
public long getDbId() { return dbId; }
/** 藏品数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
public Common.HeroObj.Equip_SkillInfo getSkillInfo() { return skillInfo; }
public void setSkillInfo(Common.HeroObj.Equip_SkillInfo _skillInfo) { skillInfo = _skillInfo; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _skillInfoCustLen = _buf.getInt();
	int _skillInfoCurPos = _buf.position();
	skillInfo.ReadUnzipBuf(_buf, _skillInfoCurPos + _skillInfoCustLen);
	_buf.position(_skillInfoCurPos + _skillInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putInt(skillInfo.GetBufSize());
	skillInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)67);
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

