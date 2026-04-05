using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 大臣信息
/// </summary>
public class Hero_Info : ALBasicProtocolPack._IALProtocolStructure {
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
/// 皮肤列表
/// </summary>
private List<Common.HeroObj.Hero_SkinInfo> skinList;
/// <summary>
/// 资质技能列表
/// </summary>
private List<Common.HeroObj.Hero_TalentSkillInfo> talentSkillList;
/// <summary>
/// 经营技能列表
/// </summary>
private List<Common.HeroObj.Hero_BusinessSkillInfo> businessSkillList;
/// <summary>
/// 光环信息
/// </summary>
private Common.HeroObj.Hero_HaloInfo haloInfo;
/// <summary>
/// 放置信息
/// </summary>
private Common.HeroObj.Hero_PlaceInfo placeInfo;
/// <summary>
/// 道具额外加成实力
/// </summary>
private long itemAddPower;
/// <summary>
/// 竞技场额外加成实力
/// </summary>
private long arenaAddPower;
/// <summary>
/// 游历额外加成实力
/// </summary>
private long travelAddPower;


public Hero_Info() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	step = 0;
	star = 0;
	skinList = new List<Common.HeroObj.Hero_SkinInfo>();
	talentSkillList = new List<Common.HeroObj.Hero_TalentSkillInfo>();
	businessSkillList = new List<Common.HeroObj.Hero_BusinessSkillInfo>();
	haloInfo = new Common.HeroObj.Hero_HaloInfo();
	placeInfo = new Common.HeroObj.Hero_PlaceInfo();
	itemAddPower = (long)0;
	arenaAddPower = (long)0;
	travelAddPower = (long)0;
}

public Hero_Info(
	long _heroId
	, long _skinId
	, int _level
	, int _step
	, int _star
	, List<Common.HeroObj.Hero_SkinInfo> _skinList
	, List<Common.HeroObj.Hero_TalentSkillInfo> _talentSkillList
	, List<Common.HeroObj.Hero_BusinessSkillInfo> _businessSkillList
	, Common.HeroObj.Hero_HaloInfo _haloInfo
	, Common.HeroObj.Hero_PlaceInfo _placeInfo
	, long _itemAddPower
	, long _arenaAddPower
	, long _travelAddPower
) {	heroId = _heroId;
	skinId = _skinId;
	level = _level;
	step = _step;
	star = _star;
	skinList = _skinList;
	talentSkillList = _talentSkillList;
	businessSkillList = _businessSkillList;
	haloInfo = _haloInfo;
	placeInfo = _placeInfo;
	itemAddPower = _itemAddPower;
	arenaAddPower = _arenaAddPower;
	travelAddPower = _travelAddPower;
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
/// 皮肤列表
/// </summary>
public List<Common.HeroObj.Hero_SkinInfo> getSkinList() { return skinList; }
/// <summary>
/// 皮肤列表
/// </summary>
public void addSkinList(Common.HeroObj.Hero_SkinInfo _skinList) { skinList.Add(_skinList); }
/// <summary>
/// 资质技能列表
/// </summary>
public List<Common.HeroObj.Hero_TalentSkillInfo> getTalentSkillList() { return talentSkillList; }
/// <summary>
/// 资质技能列表
/// </summary>
public void addTalentSkillList(Common.HeroObj.Hero_TalentSkillInfo _talentSkillList) { talentSkillList.Add(_talentSkillList); }
/// <summary>
/// 经营技能列表
/// </summary>
public List<Common.HeroObj.Hero_BusinessSkillInfo> getBusinessSkillList() { return businessSkillList; }
/// <summary>
/// 经营技能列表
/// </summary>
public void addBusinessSkillList(Common.HeroObj.Hero_BusinessSkillInfo _businessSkillList) { businessSkillList.Add(_businessSkillList); }
/// <summary>
/// 光环信息
/// </summary>
public Common.HeroObj.Hero_HaloInfo getHaloInfo() { return haloInfo; }
/// <summary>
/// 光环信息
/// </summary>
public void setHaloInfo(Common.HeroObj.Hero_HaloInfo _haloInfo) { haloInfo = _haloInfo; }
/// <summary>
/// 放置信息
/// </summary>
public Common.HeroObj.Hero_PlaceInfo getPlaceInfo() { return placeInfo; }
/// <summary>
/// 放置信息
/// </summary>
public void setPlaceInfo(Common.HeroObj.Hero_PlaceInfo _placeInfo) { placeInfo = _placeInfo; }
/// <summary>
/// 道具额外加成实力
/// </summary>
public long getItemAddPower() { return itemAddPower; }
/// <summary>
/// 道具额外加成实力
/// </summary>
public void setItemAddPower(long _itemAddPower) { itemAddPower = _itemAddPower; }
/// <summary>
/// 竞技场额外加成实力
/// </summary>
public long getArenaAddPower() { return arenaAddPower; }
/// <summary>
/// 竞技场额外加成实力
/// </summary>
public void setArenaAddPower(long _arenaAddPower) { arenaAddPower = _arenaAddPower; }
/// <summary>
/// 游历额外加成实力
/// </summary>
public long getTravelAddPower() { return travelAddPower; }
/// <summary>
/// 游历额外加成实力
/// </summary>
public void setTravelAddPower(long _travelAddPower) { travelAddPower = _travelAddPower; }


public int GetBufSize() {
	int _size = 89;
	_size += 2 + (skinList.Count * 16);
	_size += 2 + (talentSkillList.Count * 16);
	_size += 2 + (businessSkillList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 91;
	_size += 2 + (skinList.Count * 16);
	_size += 2 + (talentSkillList.Count * 16);
	_size += 2 + (businessSkillList.Count * 16);

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
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.HeroObj.Hero_SkinInfo _skinList = new Common.HeroObj.Hero_SkinInfo();
		int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.getCurPos();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.setPosition(__skinListCurPos + __skinListCustLen);

		skinList.Add(_skinList);
	}
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
	short _businessSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _businessSkillListCount; _i++) { 
		Common.HeroObj.Hero_BusinessSkillInfo _businessSkillList = new Common.HeroObj.Hero_BusinessSkillInfo();
		int __businessSkillListCustLen = _buf.getInt();
	int __businessSkillListCurPos = _buf.getCurPos();
	_businessSkillList.ReadUnzipBuf(_buf, __businessSkillListCurPos + __businessSkillListCustLen);
	_buf.setPosition(__businessSkillListCurPos + __businessSkillListCustLen);

		businessSkillList.Add(_businessSkillList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _haloInfoCustLen = _buf.getInt();
	int _haloInfoCurPos = _buf.getCurPos();
	haloInfo.ReadUnzipBuf(_buf, _haloInfoCurPos + _haloInfoCustLen);
	_buf.setPosition(_haloInfoCurPos + _haloInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _placeInfoCustLen = _buf.getInt();
	int _placeInfoCurPos = _buf.getCurPos();
	placeInfo.ReadUnzipBuf(_buf, _placeInfoCurPos + _placeInfoCustLen);
	_buf.setPosition(_placeInfoCurPos + _placeInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemAddPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	arenaAddPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	travelAddPower = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putInt(step);
	_buf.putInt(star);
	_buf.putShort((short)skinList.Count);
	for(int _i = 0; _i < skinList.Count; _i++) { 
		_buf.putInt(skinList[_i].GetBufSize());
	skinList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)talentSkillList.Count);
	for(int _i = 0; _i < talentSkillList.Count; _i++) { 
		_buf.putInt(talentSkillList[_i].GetBufSize());
	talentSkillList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)businessSkillList.Count);
	for(int _i = 0; _i < businessSkillList.Count; _i++) { 
		_buf.putInt(businessSkillList[_i].GetBufSize());
	businessSkillList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(haloInfo.GetBufSize());
	haloInfo.PutUnzipBuf(_buf);
	_buf.putInt(placeInfo.GetBufSize());
	placeInfo.PutUnzipBuf(_buf);
	_buf.putLong(itemAddPower);
	_buf.putLong(arenaAddPower);
	_buf.putLong(travelAddPower);
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
	builder.Append("skinList").Append(":").Append(skinList.ToString()).Append(", ");
	builder.Append("talentSkillList").Append(":").Append(talentSkillList.ToString()).Append(", ");
	builder.Append("businessSkillList").Append(":").Append(businessSkillList.ToString()).Append(", ");
	builder.Append("haloInfo").Append(":").Append(haloInfo == null ? "null" : haloInfo.ToString()).Append(", ");
	builder.Append("placeInfo").Append(":").Append(placeInfo == null ? "null" : placeInfo.ToString()).Append(", ");
	builder.Append("itemAddPower").Append(":").Append(itemAddPower.ToString()).Append(", ");
	builder.Append("arenaAddPower").Append(":").Append(arenaAddPower.ToString()).Append(", ");
	builder.Append("travelAddPower").Append(":").Append(travelAddPower.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

