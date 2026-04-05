package Common.CollegeObj;

import java.nio.ByteBuffer;
/*********
 * 大学学习结束信息
 **/
public class College_StudySettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 资质经验 */
private long talentExp;
/** 技能点 */
private long skillPoint;


public College_StudySettleInfo() {
	heroId = (long)0;
	talentExp = (long)0;
	skillPoint = (long)0;
}

public College_StudySettleInfo(
	 long _heroId
	, long _talentExp
	, long _skillPoint
) {	heroId = _heroId;
	talentExp = _talentExp;
	skillPoint = _skillPoint;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 资质经验 */
public long getTalentExp() { return talentExp; }
/** 资质经验 */
public void setTalentExp(long _talentExp) { talentExp = _talentExp; }
/** 技能点 */
public long getSkillPoint() { return skillPoint; }
/** 技能点 */
public void setSkillPoint(long _skillPoint) { skillPoint = _skillPoint; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) talentExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillPoint = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(talentExp);
	_buf.putLong(skillPoint);
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

