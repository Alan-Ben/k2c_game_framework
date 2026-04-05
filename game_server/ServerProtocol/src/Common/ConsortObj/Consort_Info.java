package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人数据
 **/
public class Consort_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 亲密度 */
private long intimacy;
/** 加护力 */
private long charm;
/** 加护点 */
private long charmPoint;
/** 羁绊数据 */
private Common.ConsortObj.Consort_Fetters fetters;
/** 加护技能列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_BlessSkill> blessSkillList;
/** 已触发的邀约剧情ID列表 */
private java.util.ArrayList<Long> triggeredCallStoryIdList;
/** 家人皮肤列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_SkinInfo> skinList;
/** 当前穿戴皮肤ID */
private long curSkinId;
/** 星辉是否解锁 true-已解锁 */
private boolean isUnlockHalo;
/** 星辉数据 */
private Common.ConsortObj.Consort_Halo halo;
/** 已操作经营技能数量 */
private int opBusinessSkillCount;
/** 经营技能属性数据汇总 */
private java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkillPropertySum> businessSkillPropertySum;


public Consort_Info() {
	consortId = (long)0;
	intimacy = (long)0;
	charm = (long)0;
	charmPoint = (long)0;
	fetters = new Common.ConsortObj.Consort_Fetters();
	blessSkillList = new java.util.ArrayList<Common.ConsortObj.Consort_BlessSkill>();
	triggeredCallStoryIdList = new java.util.ArrayList<Long>();
	skinList = new java.util.ArrayList<Common.ConsortObj.Consort_SkinInfo>();
	curSkinId = (long)0;
	isUnlockHalo = false;
	halo = new Common.ConsortObj.Consort_Halo();
	opBusinessSkillCount = 0;
	businessSkillPropertySum = new java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkillPropertySum>();
}

public Consort_Info(
	 long _consortId
	, long _intimacy
	, long _charm
	, long _charmPoint
	, Common.ConsortObj.Consort_Fetters _fetters
	, java.util.ArrayList<Common.ConsortObj.Consort_BlessSkill> _blessSkillList
	, java.util.ArrayList<Long> _triggeredCallStoryIdList
	, java.util.ArrayList<Common.ConsortObj.Consort_SkinInfo> _skinList
	, long _curSkinId
	, boolean _isUnlockHalo
	, Common.ConsortObj.Consort_Halo _halo
	, int _opBusinessSkillCount
	, java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkillPropertySum> _businessSkillPropertySum
) {	consortId = _consortId;
	intimacy = _intimacy;
	charm = _charm;
	charmPoint = _charmPoint;
	fetters = _fetters;
	blessSkillList = _blessSkillList;
	triggeredCallStoryIdList = _triggeredCallStoryIdList;
	skinList = _skinList;
	curSkinId = _curSkinId;
	isUnlockHalo = _isUnlockHalo;
	halo = _halo;
	opBusinessSkillCount = _opBusinessSkillCount;
	businessSkillPropertySum = _businessSkillPropertySum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 亲密度 */
public long getIntimacy() { return intimacy; }
/** 亲密度 */
public void setIntimacy(long _intimacy) { intimacy = _intimacy; }
/** 加护力 */
public long getCharm() { return charm; }
/** 加护力 */
public void setCharm(long _charm) { charm = _charm; }
/** 加护点 */
public long getCharmPoint() { return charmPoint; }
/** 加护点 */
public void setCharmPoint(long _charmPoint) { charmPoint = _charmPoint; }
/** 羁绊数据 */
public Common.ConsortObj.Consort_Fetters getFetters() { return fetters; }
/** 羁绊数据 */
public void setFetters(Common.ConsortObj.Consort_Fetters _fetters) { fetters = _fetters; }
/** 加护技能列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_BlessSkill> getBlessSkillList() { return blessSkillList; }
/** 加护技能列表 */
public void addBlessSkillList(Common.ConsortObj.Consort_BlessSkill _blessSkillList) { blessSkillList.add(_blessSkillList); }
/** 已触发的邀约剧情ID列表 */
public java.util.ArrayList<Long> getTriggeredCallStoryIdList() { return triggeredCallStoryIdList; }
/** 已触发的邀约剧情ID列表 */
public void addTriggeredCallStoryIdList(long _triggeredCallStoryIdList) { triggeredCallStoryIdList.add(_triggeredCallStoryIdList); }
/** 家人皮肤列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_SkinInfo> getSkinList() { return skinList; }
/** 家人皮肤列表 */
public void addSkinList(Common.ConsortObj.Consort_SkinInfo _skinList) { skinList.add(_skinList); }
/** 当前穿戴皮肤ID */
public long getCurSkinId() { return curSkinId; }
/** 当前穿戴皮肤ID */
public void setCurSkinId(long _curSkinId) { curSkinId = _curSkinId; }
/** 星辉是否解锁 true-已解锁 */
public boolean getIsUnlockHalo() { return isUnlockHalo; }
/** 星辉是否解锁 true-已解锁 */
public void setIsUnlockHalo(boolean _isUnlockHalo) { isUnlockHalo = _isUnlockHalo; }
/** 星辉数据 */
public Common.ConsortObj.Consort_Halo getHalo() { return halo; }
/** 星辉数据 */
public void setHalo(Common.ConsortObj.Consort_Halo _halo) { halo = _halo; }
/** 已操作经营技能数量 */
public int getOpBusinessSkillCount() { return opBusinessSkillCount; }
/** 已操作经营技能数量 */
public void setOpBusinessSkillCount(int _opBusinessSkillCount) { opBusinessSkillCount = _opBusinessSkillCount; }
/** 经营技能属性数据汇总 */
public java.util.ArrayList<Common.ConsortObj.Consort_BusinessSkillPropertySum> getBusinessSkillPropertySum() { return businessSkillPropertySum; }
/** 经营技能属性数据汇总 */
public void addBusinessSkillPropertySum(Common.ConsortObj.Consort_BusinessSkillPropertySum _businessSkillPropertySum) { businessSkillPropertySum.add(_businessSkillPropertySum); }


public final int GetBufSize() {
	int _size = 61;
	_size += 2 + (blessSkillList.size() * 16);
	_size += 2 + (triggeredCallStoryIdList.size() * 8);
	_size += 2 + (skinList.size() * 16);
	_size += 2 + (businessSkillPropertySum.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 63;
	_size += 2 + (blessSkillList.size() * 16);
	_size += 2 + (triggeredCallStoryIdList.size() * 8);
	_size += 2 + (skinList.size() * 16);
	_size += 2 + (businessSkillPropertySum.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) intimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) charm = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) charmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _fettersCustLen = _buf.getInt();
	int _fettersCurPos = _buf.position();
	fetters.ReadUnzipBuf(_buf, _fettersCurPos + _fettersCustLen);
	_buf.position(_fettersCurPos + _fettersCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _blessSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _blessSkillListCount; _i++) { 
		Common.ConsortObj.Consort_BlessSkill _blessSkillList = new Common.ConsortObj.Consort_BlessSkill();
		if(_buf.remaining() <= 0) return;
	int __blessSkillListCustLen = _buf.getInt();
	int __blessSkillListCurPos = _buf.position();
	_blessSkillList.ReadUnzipBuf(_buf, __blessSkillListCurPos + __blessSkillListCustLen);
	_buf.position(__blessSkillListCurPos + __blessSkillListCustLen);

		blessSkillList.add(_blessSkillList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _triggeredCallStoryIdListCount = _buf.getShort();
	for(int _i = 0; _i < _triggeredCallStoryIdListCount; _i++) { 
		long _triggeredCallStoryIdList = (long)0;
		if(_buf.remaining() > 0) _triggeredCallStoryIdList = _buf.getLong();
		triggeredCallStoryIdList.add(_triggeredCallStoryIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.ConsortObj.Consort_SkinInfo _skinList = new Common.ConsortObj.Consort_SkinInfo();
		if(_buf.remaining() <= 0) return;
	int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.position();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.position(__skinListCurPos + __skinListCustLen);

		skinList.add(_skinList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curSkinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUnlockHalo = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _haloCustLen = _buf.getInt();
	int _haloCurPos = _buf.position();
	halo.ReadUnzipBuf(_buf, _haloCurPos + _haloCustLen);
	_buf.position(_haloCurPos + _haloCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opBusinessSkillCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _businessSkillPropertySumCount = _buf.getShort();
	for(int _i = 0; _i < _businessSkillPropertySumCount; _i++) { 
		Common.ConsortObj.Consort_BusinessSkillPropertySum _businessSkillPropertySum = new Common.ConsortObj.Consort_BusinessSkillPropertySum();
		if(_buf.remaining() <= 0) return;
	int __businessSkillPropertySumCustLen = _buf.getInt();
	int __businessSkillPropertySumCurPos = _buf.position();
	_businessSkillPropertySum.ReadUnzipBuf(_buf, __businessSkillPropertySumCurPos + __businessSkillPropertySumCustLen);
	_buf.position(__businessSkillPropertySumCurPos + __businessSkillPropertySumCustLen);

		businessSkillPropertySum.add(_businessSkillPropertySum);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(intimacy);
	_buf.putLong(charm);
	_buf.putLong(charmPoint);
	_buf.putInt(fetters.GetBufSize());
	fetters.PutUnzipBuf(_buf);
	_buf.putShort((short)blessSkillList.size());
	for(int _i = 0; _i < blessSkillList.size(); _i++) { 
		_buf.putInt(blessSkillList.get(_i).GetBufSize());
	blessSkillList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)triggeredCallStoryIdList.size());
	for(int _i = 0; _i < triggeredCallStoryIdList.size(); _i++) { 
		_buf.putLong(triggeredCallStoryIdList.get(_i));
	}
	_buf.putShort((short)skinList.size());
	for(int _i = 0; _i < skinList.size(); _i++) { 
		_buf.putInt(skinList.get(_i).GetBufSize());
	skinList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(curSkinId);
	_buf.put(isUnlockHalo?(byte)1:(byte)0);
	_buf.putInt(halo.GetBufSize());
	halo.PutUnzipBuf(_buf);
	_buf.putInt(opBusinessSkillCount);
	_buf.putShort((short)businessSkillPropertySum.size());
	for(int _i = 0; _i < businessSkillPropertySum.size(); _i++) { 
		_buf.putInt(businessSkillPropertySum.get(_i).GetBufSize());
	businessSkillPropertySum.get(_i).PutUnzipBuf(_buf);
	}
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

