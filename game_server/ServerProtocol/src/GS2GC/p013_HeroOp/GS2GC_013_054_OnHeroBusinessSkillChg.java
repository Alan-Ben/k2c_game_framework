package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣经营技能变更
 **/
public class GS2GC_013_054_OnHeroBusinessSkillChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 经营技能信息 */
private Common.HeroObj.Hero_BusinessSkillInfo businessSkillInfo;


public GS2GC_013_054_OnHeroBusinessSkillChg() {
	heroId = (long)0;
	businessSkillInfo = new Common.HeroObj.Hero_BusinessSkillInfo();
}

public GS2GC_013_054_OnHeroBusinessSkillChg(
	 long _heroId
	, Common.HeroObj.Hero_BusinessSkillInfo _businessSkillInfo
) {	heroId = _heroId;
	businessSkillInfo = _businessSkillInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)54; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 经营技能信息 */
public Common.HeroObj.Hero_BusinessSkillInfo getBusinessSkillInfo() { return businessSkillInfo; }
/** 经营技能信息 */
public void setBusinessSkillInfo(Common.HeroObj.Hero_BusinessSkillInfo _businessSkillInfo) { businessSkillInfo = _businessSkillInfo; }


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
	if(_buf.remaining() <= 0) return;
	int _businessSkillInfoCustLen = _buf.getInt();
	int _businessSkillInfoCurPos = _buf.position();
	businessSkillInfo.ReadUnzipBuf(_buf, _businessSkillInfoCurPos + _businessSkillInfoCustLen);
	_buf.position(_businessSkillInfoCurPos + _businessSkillInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(businessSkillInfo.GetBufSize());
	businessSkillInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)54);
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

