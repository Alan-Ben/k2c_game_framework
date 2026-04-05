package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 升级大臣资质技能
 **/
public class GC2GS_013_007_ReqHeroTalentSkillUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 资质技能id */
private long talentSkillId;
/** 是否十连升级 */
private boolean isTen;


public GC2GS_013_007_ReqHeroTalentSkillUpgrade() {
	heroId = (long)0;
	talentSkillId = (long)0;
	isTen = false;
}

public GC2GS_013_007_ReqHeroTalentSkillUpgrade(
	 long _heroId
	, long _talentSkillId
	, boolean _isTen
) {	heroId = _heroId;
	talentSkillId = _talentSkillId;
	isTen = _isTen;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)7; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 资质技能id */
public long getTalentSkillId() { return talentSkillId; }
/** 资质技能id */
public void setTalentSkillId(long _talentSkillId) { talentSkillId = _talentSkillId; }
/** 是否十连升级 */
public boolean getIsTen() { return isTen; }
/** 是否十连升级 */
public void setIsTen(boolean _isTen) { isTen = _isTen; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) talentSkillId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(talentSkillId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)7);
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

