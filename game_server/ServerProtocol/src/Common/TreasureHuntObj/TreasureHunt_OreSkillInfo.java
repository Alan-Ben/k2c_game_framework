package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石技能信息
 **/
public class TreasureHunt_OreSkillInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 技能等级 */
private int skillLevel;
/** 技能点数 */
private int skillPoint;


public TreasureHunt_OreSkillInfo() {
	skillLevel = 0;
	skillPoint = 0;
}

public TreasureHunt_OreSkillInfo(
	 int _skillLevel
	, int _skillPoint
) {	skillLevel = _skillLevel;
	skillPoint = _skillPoint;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 技能等级 */
public int getSkillLevel() { return skillLevel; }
/** 技能等级 */
public void setSkillLevel(int _skillLevel) { skillLevel = _skillLevel; }
/** 技能点数 */
public int getSkillPoint() { return skillPoint; }
/** 技能点数 */
public void setSkillPoint(int _skillPoint) { skillPoint = _skillPoint; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillPoint = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(skillLevel);
	_buf.putInt(skillPoint);
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

