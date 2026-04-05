using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 大臣分享
/// </summary>
public class NPCommon_ChatContent_HeroShare : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 皮肤id
/// </summary>
private long skinId;
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 阶段
/// </summary>
private int step;
/// <summary>
/// 觉醒星级
/// </summary>
private int star;
/// <summary>
/// 战力
/// </summary>
private long power;
/// <summary>
/// 资质技能列表
/// </summary>
private List<Common.HeroObj.Hero_TalentSkillInfo> talentSkillList;
/// <summary>
/// 资质
/// </summary>
private long talent;


public NPCommon_ChatContent_HeroShare() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	step = 0;
	star = 0;
	power = (long)0;
	talentSkillList = new List<Common.HeroObj.Hero_TalentSkillInfo>();
	talent = (long)0;
}

public NPCommon_ChatContent_HeroShare(
	long _heroId
	, long _skinId
	, int _level
	, int _step
	, int _star
	, long _power
	, List<Common.HeroObj.Hero_TalentSkillInfo> _talentSkillList
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 皮肤id
/// </summary>
public long getSkinId() { return skinId; }
/// <summary>
/// 皮肤id
/// </summary>
public void setSkinId(long _skinId) { skinId = _skinId; }
/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 阶段
/// </summary>
public int getStep() { return step; }
/// <summary>
/// 阶段
/// </summary>
public void setStep(int _step) { step = _step; }
/// <summary>
/// 觉醒星级
/// </summary>
public int getStar() { return star; }
/// <summary>
/// 觉醒星级
/// </summary>
public void setStar(int _star) { star = _star; }
/// <summary>
/// 战力
/// </summary>
public long getPower() { return power; }
/// <summary>
/// 战力
/// </summary>
public void setPower(long _power) { power = _power; }
/// <summary>
/// 资质技能列表
/// </summary>
public List<Common.HeroObj.Hero_TalentSkillInfo> getTalentSkillList() { return talentSkillList; }
/// <summary>
/// 资质技能列表
/// </summary>
public void addTalentSkillList(Common.HeroObj.Hero_TalentSkillInfo _talentSkillList) { talentSkillList.Add(_talentSkillList); }
/// <summary>
/// 资质
/// </summary>
public long getTalent() { return talent; }
/// <summary>
/// 资质
/// </summary>
public void setTalent(long _talent) { talent = _talent; }


public int GetBufSize() {
	int _size = 44;
	_size += 2 + (talentSkillList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;
	_size += 2 + (talentSkillList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	star = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	power = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _talentSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _talentSkillListCount; _i++) { 
		Common.HeroObj.Hero_TalentSkillInfo _talentSkillList = new Common.HeroObj.Hero_TalentSkillInfo();
		int __talentSkillListCustLen = _buf.getInt();
	int __talentSkillListCurPos = _buf.getCurPos();
	_talentSkillList.ReadUnzipBuf(_buf, __talentSkillListCurPos + __talentSkillListCustLen);
	_buf.setPosition(__talentSkillListCurPos + __talentSkillListCustLen);

		talentSkillList.Add(_talentSkillList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	talent = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putInt(step);
	_buf.putInt(star);
	_buf.putLong(power);
	_buf.putShort((short)talentSkillList.Count);
	for(int _i = 0; _i < talentSkillList.Count; _i++) { 
		_buf.putInt(talentSkillList[_i].GetBufSize());
	talentSkillList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(talent);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("step").Append(":").Append(step.ToString()).Append(", ");
	builder.Append("star").Append(":").Append(star.ToString()).Append(", ");
	builder.Append("power").Append(":").Append(power.ToString()).Append(", ");
	builder.Append("talentSkillList").Append(":").Append(talentSkillList.ToString()).Append(", ");
	builder.Append("talent").Append(":").Append(talent.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

