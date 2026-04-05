package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣信息
 **/
public class Hero_Info implements ALBasicProtocolPack._IALProtocolStructure {
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
/** 皮肤列表 */
private java.util.ArrayList<Common.HeroObj.Hero_SkinInfo> skinList;
/** 资质技能列表 */
private java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> talentSkillList;
/** 经营技能列表 */
private java.util.ArrayList<Common.HeroObj.Hero_BusinessSkillInfo> businessSkillList;
/** 光环信息 */
private Common.HeroObj.Hero_HaloInfo haloInfo;
/** 放置信息 */
private Common.HeroObj.Hero_PlaceInfo placeInfo;
/** 道具额外加成实力 */
private long itemAddPower;
/** 竞技场额外加成实力 */
private long arenaAddPower;
/** 游历额外加成实力 */
private long travelAddPower;


public Hero_Info() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	step = 0;
	star = 0;
	skinList = new java.util.ArrayList<Common.HeroObj.Hero_SkinInfo>();
	talentSkillList = new java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo>();
	businessSkillList = new java.util.ArrayList<Common.HeroObj.Hero_BusinessSkillInfo>();
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
	, java.util.ArrayList<Common.HeroObj.Hero_SkinInfo> _skinList
	, java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> _talentSkillList
	, java.util.ArrayList<Common.HeroObj.Hero_BusinessSkillInfo> _businessSkillList
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
/** 皮肤列表 */
public java.util.ArrayList<Common.HeroObj.Hero_SkinInfo> getSkinList() { return skinList; }
/** 皮肤列表 */
public void addSkinList(Common.HeroObj.Hero_SkinInfo _skinList) { skinList.add(_skinList); }
/** 资质技能列表 */
public java.util.ArrayList<Common.HeroObj.Hero_TalentSkillInfo> getTalentSkillList() { return talentSkillList; }
/** 资质技能列表 */
public void addTalentSkillList(Common.HeroObj.Hero_TalentSkillInfo _talentSkillList) { talentSkillList.add(_talentSkillList); }
/** 经营技能列表 */
public java.util.ArrayList<Common.HeroObj.Hero_BusinessSkillInfo> getBusinessSkillList() { return businessSkillList; }
/** 经营技能列表 */
public void addBusinessSkillList(Common.HeroObj.Hero_BusinessSkillInfo _businessSkillList) { businessSkillList.add(_businessSkillList); }
/** 光环信息 */
public Common.HeroObj.Hero_HaloInfo getHaloInfo() { return haloInfo; }
/** 光环信息 */
public void setHaloInfo(Common.HeroObj.Hero_HaloInfo _haloInfo) { haloInfo = _haloInfo; }
/** 放置信息 */
public Common.HeroObj.Hero_PlaceInfo getPlaceInfo() { return placeInfo; }
/** 放置信息 */
public void setPlaceInfo(Common.HeroObj.Hero_PlaceInfo _placeInfo) { placeInfo = _placeInfo; }
/** 道具额外加成实力 */
public long getItemAddPower() { return itemAddPower; }
/** 道具额外加成实力 */
public void setItemAddPower(long _itemAddPower) { itemAddPower = _itemAddPower; }
/** 竞技场额外加成实力 */
public long getArenaAddPower() { return arenaAddPower; }
/** 竞技场额外加成实力 */
public void setArenaAddPower(long _arenaAddPower) { arenaAddPower = _arenaAddPower; }
/** 游历额外加成实力 */
public long getTravelAddPower() { return travelAddPower; }
/** 游历额外加成实力 */
public void setTravelAddPower(long _travelAddPower) { travelAddPower = _travelAddPower; }


public final int GetBufSize() {
	int _size = 89;
	_size += 2 + (skinList.size() * 16);
	_size += 2 + (talentSkillList.size() * 16);
	_size += 2 + (businessSkillList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 91;
	_size += 2 + (skinList.size() * 16);
	_size += 2 + (talentSkillList.size() * 16);
	_size += 2 + (businessSkillList.size() * 16);

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
	if(_buf.remaining() <= 0) return;
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.HeroObj.Hero_SkinInfo _skinList = new Common.HeroObj.Hero_SkinInfo();
		if(_buf.remaining() <= 0) return;
	int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.position();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.position(__skinListCurPos + __skinListCustLen);

		skinList.add(_skinList);
	}
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
	if(_buf.remaining() <= 0) return;
	short _businessSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _businessSkillListCount; _i++) { 
		Common.HeroObj.Hero_BusinessSkillInfo _businessSkillList = new Common.HeroObj.Hero_BusinessSkillInfo();
		if(_buf.remaining() <= 0) return;
	int __businessSkillListCustLen = _buf.getInt();
	int __businessSkillListCurPos = _buf.position();
	_businessSkillList.ReadUnzipBuf(_buf, __businessSkillListCurPos + __businessSkillListCustLen);
	_buf.position(__businessSkillListCurPos + __businessSkillListCustLen);

		businessSkillList.add(_businessSkillList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _haloInfoCustLen = _buf.getInt();
	int _haloInfoCurPos = _buf.position();
	haloInfo.ReadUnzipBuf(_buf, _haloInfoCurPos + _haloInfoCustLen);
	_buf.position(_haloInfoCurPos + _haloInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _placeInfoCustLen = _buf.getInt();
	int _placeInfoCurPos = _buf.position();
	placeInfo.ReadUnzipBuf(_buf, _placeInfoCurPos + _placeInfoCustLen);
	_buf.position(_placeInfoCurPos + _placeInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemAddPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) arenaAddPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) travelAddPower = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putInt(step);
	_buf.putInt(star);
	_buf.putShort((short)skinList.size());
	for(int _i = 0; _i < skinList.size(); _i++) { 
		_buf.putInt(skinList.get(_i).GetBufSize());
	skinList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)talentSkillList.size());
	for(int _i = 0; _i < talentSkillList.size(); _i++) { 
		_buf.putInt(talentSkillList.get(_i).GetBufSize());
	talentSkillList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)businessSkillList.size());
	for(int _i = 0; _i < businessSkillList.size(); _i++) { 
		_buf.putInt(businessSkillList.get(_i).GetBufSize());
	businessSkillList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(haloInfo.GetBufSize());
	haloInfo.PutUnzipBuf(_buf);
	_buf.putInt(placeInfo.GetBufSize());
	placeInfo.PutUnzipBuf(_buf);
	_buf.putLong(itemAddPower);
	_buf.putLong(arenaAddPower);
	_buf.putLong(travelAddPower);
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

