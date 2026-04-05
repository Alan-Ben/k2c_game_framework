package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石技能变更
 **/
public class GS2GC_036_055_OnTreasureHuntOreSkillChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** true普通技能 false高级技能 */
private boolean isNormal;
/** 技能信息 */
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo skillInfo;


public GS2GC_036_055_OnTreasureHuntOreSkillChg() {
	oreId = (long)0;
	isNormal = false;
	skillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
}

public GS2GC_036_055_OnTreasureHuntOreSkillChg(
	 long _oreId
	, boolean _isNormal
	, Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _skillInfo
) {	oreId = _oreId;
	isNormal = _isNormal;
	skillInfo = _skillInfo;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)55; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** true普通技能 false高级技能 */
public boolean getIsNormal() { return isNormal; }
/** true普通技能 false高级技能 */
public void setIsNormal(boolean _isNormal) { isNormal = _isNormal; }
/** 技能信息 */
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getSkillInfo() { return skillInfo; }
/** 技能信息 */
public void setSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _skillInfo) { skillInfo = _skillInfo; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNormal = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _skillInfoCustLen = _buf.getInt();
	int _skillInfoCurPos = _buf.position();
	skillInfo.ReadUnzipBuf(_buf, _skillInfoCurPos + _skillInfoCustLen);
	_buf.position(_skillInfoCurPos + _skillInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.put(isNormal?(byte)1:(byte)0);
	_buf.putInt(skillInfo.GetBufSize());
	skillInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)55);
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

