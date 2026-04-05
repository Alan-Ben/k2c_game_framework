package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣资质技能变更
 **/
public class GS2GC_013_052_OnHeroTalentSkillChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 资质技能信息 */
private Common.HeroObj.Hero_TalentSkillInfo talentSkillInfo;


public GS2GC_013_052_OnHeroTalentSkillChg() {
	heroId = (long)0;
	talentSkillInfo = new Common.HeroObj.Hero_TalentSkillInfo();
}

public GS2GC_013_052_OnHeroTalentSkillChg(
	 long _heroId
	, Common.HeroObj.Hero_TalentSkillInfo _talentSkillInfo
) {	heroId = _heroId;
	talentSkillInfo = _talentSkillInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)52; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 资质技能信息 */
public Common.HeroObj.Hero_TalentSkillInfo getTalentSkillInfo() { return talentSkillInfo; }
/** 资质技能信息 */
public void setTalentSkillInfo(Common.HeroObj.Hero_TalentSkillInfo _talentSkillInfo) { talentSkillInfo = _talentSkillInfo; }


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
	int _talentSkillInfoCustLen = _buf.getInt();
	int _talentSkillInfoCurPos = _buf.position();
	talentSkillInfo.ReadUnzipBuf(_buf, _talentSkillInfoCurPos + _talentSkillInfoCustLen);
	_buf.position(_talentSkillInfoCurPos + _talentSkillInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(talentSkillInfo.GetBufSize());
	talentSkillInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)52);
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

