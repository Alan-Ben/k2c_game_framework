package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 大臣分享
 **/
public class NPCommon_ChatContent_HeroShare implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 皮肤id */
private long skinId;
/** 等级 */
private int level;
/** 阶段 */
private int step;
/** 觉醒星级 */
private int star;
/** 战力 */
private long power;
/** 资质技能列表 */
private java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> talentSkillList;
/** 资质 */
private long talent;


public NPCommon_ChatContent_HeroShare() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	step = 0;
	star = 0;
	power = (long)0;
	talentSkillList = new java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo>();
	talent = (long)0;
}

public NPCommon_ChatContent_HeroShare(
	 long _heroId
	, long _skinId
	, int _level
	, int _step
	, int _star
	, long _power
	, java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> _talentSkillList
	, long _talent
) {	heroId = _heroId;
	skinId = _skinId;
	level = _level;
	step = _step;
	star = _star;
	power = _power;
	talentSkillList = _talentSkillList;
	talent = _talent;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 皮肤id */
public long getSkinId() { return skinId; }
/** 皮肤id */
public void setSkinId(long _skinId) { skinId = _skinId; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 阶段 */
public int getStep() { return step; }
/** 阶段 */
public void setStep(int _step) { step = _step; }
/** 觉醒星级 */
public int getStar() { return star; }
/** 觉醒星级 */
public void setStar(int _star) { star = _star; }
/** 战力 */
public long getPower() { return power; }
/** 战力 */
public void setPower(long _power) { power = _power; }
/** 资质技能列表 */
public java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> getTalentSkillList() { return talentSkillList; }
/** 资质技能列表 */
public void addTalentSkillList(Common.HeroObj.Hero_TalentSkillInfo _talentSkillList) { talentSkillList.add(_talentSkillList); }
/** 资质 */
public long getTalent() { return talent; }
/** 资质 */
public void setTalent(long _talent) { talent = _talent; }


public final int GetBufSize() {
	int _size = 44;
	_size += 2 + (talentSkillList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;
	_size += 2 + (talentSkillList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) star = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) power = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _talentSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _talentSkillListCount; _i++) { 
		Common.HeroObj.Hero_TalentSkillInfo _talentSkillList = new Common.HeroObj.Hero_TalentSkillInfo();
		if(_buf.remaining() <= 0) return;
	int __talentSkillListCustLen = _buf.getInt();
	int __talentSkillListCurPos = _buf.position();
	_talentSkillList.ReadUnzipBuf(_buf, __talentSkillListCurPos + __talentSkillListCustLen);
	_buf.position(__talentSkillListCurPos + __talentSkillListCustLen);

		talentSkillList.add(_talentSkillList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) talent = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putInt(step);
	_buf.putInt(star);
	_buf.putLong(power);
	_buf.putShort((short)talentSkillList.size());
	for(int _i = 0; _i < talentSkillList.size(); _i++) { 
		_buf.putInt(talentSkillList.get(_i).GetBufSize());
	talentSkillList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(talent);
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

