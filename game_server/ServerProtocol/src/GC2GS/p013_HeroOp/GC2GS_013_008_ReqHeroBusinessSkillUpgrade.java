package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 升级大臣经营技能
 **/
public class GC2GS_013_008_ReqHeroBusinessSkillUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 经营技能id */
private long businessSkillId;


public GC2GS_013_008_ReqHeroBusinessSkillUpgrade() {
	heroId = (long)0;
	businessSkillId = (long)0;
}

public GC2GS_013_008_ReqHeroBusinessSkillUpgrade(
	 long _heroId
	, long _businessSkillId
) {	heroId = _heroId;
	businessSkillId = _businessSkillId;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)8; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 经营技能id */
public long getBusinessSkillId() { return businessSkillId; }
/** 经营技能id */
public void setBusinessSkillId(long _businessSkillId) { businessSkillId = _businessSkillId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) businessSkillId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(businessSkillId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)8);
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

