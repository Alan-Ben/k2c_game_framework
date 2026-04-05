using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人数据
/// </summary>
public class Consort_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 亲密度
/// </summary>
private long intimacy;
/// <summary>
/// 加护力
/// </summary>
private long charm;
/// <summary>
/// 加护点
/// </summary>
private long charmPoint;
/// <summary>
/// 羁绊数据
/// </summary>
private Common.ConsortObj.Consort_Fetters fetters;
/// <summary>
/// 加护技能列表
/// </summary>
private List<Common.ConsortObj.Consort_BlessSkill> blessSkillList;
/// <summary>
/// 已触发的邀约剧情ID列表
/// </summary>
private List<long> triggeredCallStoryIdList;
/// <summary>
/// 家人皮肤列表
/// </summary>
private List<Common.ConsortObj.Consort_SkinInfo> skinList;
/// <summary>
/// 当前穿戴皮肤ID
/// </summary>
private long curSkinId;
/// <summary>
/// 星辉是否解锁 true-已解锁
/// </summary>
private bool isUnlockHalo;
/// <summary>
/// 星辉数据
/// </summary>
private Common.ConsortObj.Consort_Halo halo;
/// <summary>
/// 已操作经营技能数量
/// </summary>
private int opBusinessSkillCount;
/// <summary>
/// 经营技能属性数据汇总
/// </summary>
private List<Common.ConsortObj.Consort_BusinessSkillPropertySum> businessSkillPropertySum;


public Consort_Info() {
	consortId = (long)0;
	intimacy = (long)0;
	charm = (long)0;
	charmPoint = (long)0;
	fetters = new Common.ConsortObj.Consort_Fetters();
	blessSkillList = new List<Common.ConsortObj.Consort_BlessSkill>();
	triggeredCallStoryIdList = new List<long>();
	skinList = new List<Common.ConsortObj.Consort_SkinInfo>();
	curSkinId = (long)0;
	isUnlockHalo = false;
	halo = new Common.ConsortObj.Consort_Halo();
	opBusinessSkillCount = 0;
	businessSkillPropertySum = new List<Common.ConsortObj.Consort_BusinessSkillPropertySum>();
}

public Consort_Info(
	long _consortId
	, long _intimacy
	, long _charm
	, long _charmPoint
	, Common.ConsortObj.Consort_Fetters _fetters
	, List<Common.ConsortObj.Consort_BlessSkill> _blessSkillList
	, List<long> _triggeredCallStoryIdList
	, List<Common.ConsortObj.Consort_SkinInfo> _skinList
	, long _curSkinId
	, bool _isUnlockHalo
	, Common.ConsortObj.Consort_Halo _halo
	, int _opBusinessSkillCount
	, List<Common.ConsortObj.Consort_BusinessSkillPropertySum> _businessSkillPropertySum
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 亲密度
/// </summary>
public long getIntimacy() { return intimacy; }
/// <summary>
/// 亲密度
/// </summary>
public void setIntimacy(long _intimacy) { intimacy = _intimacy; }
/// <summary>
/// 加护力
/// </summary>
public long getCharm() { return charm; }
/// <summary>
/// 加护力
/// </summary>
public void setCharm(long _charm) { charm = _charm; }
/// <summary>
/// 加护点
/// </summary>
public long getCharmPoint() { return charmPoint; }
/// <summary>
/// 加护点
/// </summary>
public void setCharmPoint(long _charmPoint) { charmPoint = _charmPoint; }
/// <summary>
/// 羁绊数据
/// </summary>
public Common.ConsortObj.Consort_Fetters getFetters() { return fetters; }
/// <summary>
/// 羁绊数据
/// </summary>
public void setFetters(Common.ConsortObj.Consort_Fetters _fetters) { fetters = _fetters; }
/// <summary>
/// 加护技能列表
/// </summary>
public List<Common.ConsortObj.Consort_BlessSkill> getBlessSkillList() { return blessSkillList; }
/// <summary>
/// 加护技能列表
/// </summary>
public void addBlessSkillList(Common.ConsortObj.Consort_BlessSkill _blessSkillList) { blessSkillList.Add(_blessSkillList); }
/// <summary>
/// 已触发的邀约剧情ID列表
/// </summary>
public List<long> getTriggeredCallStoryIdList() { return triggeredCallStoryIdList; }
/// <summary>
/// 已触发的邀约剧情ID列表
/// </summary>
public void addTriggeredCallStoryIdList(long _triggeredCallStoryIdList) { triggeredCallStoryIdList.Add(_triggeredCallStoryIdList); }
/// <summary>
/// 家人皮肤列表
/// </summary>
public List<Common.ConsortObj.Consort_SkinInfo> getSkinList() { return skinList; }
/// <summary>
/// 家人皮肤列表
/// </summary>
public void addSkinList(Common.ConsortObj.Consort_SkinInfo _skinList) { skinList.Add(_skinList); }
/// <summary>
/// 当前穿戴皮肤ID
/// </summary>
public long getCurSkinId() { return curSkinId; }
/// <summary>
/// 当前穿戴皮肤ID
/// </summary>
public void setCurSkinId(long _curSkinId) { curSkinId = _curSkinId; }
/// <summary>
/// 星辉是否解锁 true-已解锁
/// </summary>
public bool getIsUnlockHalo() { return isUnlockHalo; }
/// <summary>
/// 星辉是否解锁 true-已解锁
/// </summary>
public void setIsUnlockHalo(bool _isUnlockHalo) { isUnlockHalo = _isUnlockHalo; }
/// <summary>
/// 星辉数据
/// </summary>
public Common.ConsortObj.Consort_Halo getHalo() { return halo; }
/// <summary>
/// 星辉数据
/// </summary>
public void setHalo(Common.ConsortObj.Consort_Halo _halo) { halo = _halo; }
/// <summary>
/// 已操作经营技能数量
/// </summary>
public int getOpBusinessSkillCount() { return opBusinessSkillCount; }
/// <summary>
/// 已操作经营技能数量
/// </summary>
public void setOpBusinessSkillCount(int _opBusinessSkillCount) { opBusinessSkillCount = _opBusinessSkillCount; }
/// <summary>
/// 经营技能属性数据汇总
/// </summary>
public List<Common.ConsortObj.Consort_BusinessSkillPropertySum> getBusinessSkillPropertySum() { return businessSkillPropertySum; }
/// <summary>
/// 经营技能属性数据汇总
/// </summary>
public void addBusinessSkillPropertySum(Common.ConsortObj.Consort_BusinessSkillPropertySum _businessSkillPropertySum) { businessSkillPropertySum.Add(_businessSkillPropertySum); }


public int GetBufSize() {
	int _size = 61;
	_size += 2 + (blessSkillList.Count * 16);
	_size += 2 + (triggeredCallStoryIdList.Count * 8);
	_size += 2 + (skinList.Count * 16);
	_size += 2 + (businessSkillPropertySum.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 63;
	_size += 2 + (blessSkillList.Count * 16);
	_size += 2 + (triggeredCallStoryIdList.Count * 8);
	_size += 2 + (skinList.Count * 16);
	_size += 2 + (businessSkillPropertySum.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	intimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	charm = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	charmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _fettersCustLen = _buf.getInt();
	int _fettersCurPos = _buf.getCurPos();
	fetters.ReadUnzipBuf(_buf, _fettersCurPos + _fettersCustLen);
	_buf.setPosition(_fettersCurPos + _fettersCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _blessSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _blessSkillListCount; _i++) { 
		Common.ConsortObj.Consort_BlessSkill _blessSkillList = new Common.ConsortObj.Consort_BlessSkill();
		int __blessSkillListCustLen = _buf.getInt();
	int __blessSkillListCurPos = _buf.getCurPos();
	_blessSkillList.ReadUnzipBuf(_buf, __blessSkillListCurPos + __blessSkillListCustLen);
	_buf.setPosition(__blessSkillListCurPos + __blessSkillListCustLen);

		blessSkillList.Add(_blessSkillList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _triggeredCallStoryIdListCount = _buf.getShort();
	for(int _i = 0; _i < _triggeredCallStoryIdListCount; _i++) { 
		long _triggeredCallStoryIdList = (long)0;
		_triggeredCallStoryIdList = _buf.getLong();
		triggeredCallStoryIdList.Add(_triggeredCallStoryIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.ConsortObj.Consort_SkinInfo _skinList = new Common.ConsortObj.Consort_SkinInfo();
		int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.getCurPos();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.setPosition(__skinListCurPos + __skinListCustLen);

		skinList.Add(_skinList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curSkinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isUnlockHalo = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _haloCustLen = _buf.getInt();
	int _haloCurPos = _buf.getCurPos();
	halo.ReadUnzipBuf(_buf, _haloCurPos + _haloCustLen);
	_buf.setPosition(_haloCurPos + _haloCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opBusinessSkillCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _businessSkillPropertySumCount = _buf.getShort();
	for(int _i = 0; _i < _businessSkillPropertySumCount; _i++) { 
		Common.ConsortObj.Consort_BusinessSkillPropertySum _businessSkillPropertySum = new Common.ConsortObj.Consort_BusinessSkillPropertySum();
		int __businessSkillPropertySumCustLen = _buf.getInt();
	int __businessSkillPropertySumCurPos = _buf.getCurPos();
	_businessSkillPropertySum.ReadUnzipBuf(_buf, __businessSkillPropertySumCurPos + __businessSkillPropertySumCustLen);
	_buf.setPosition(__businessSkillPropertySumCurPos + __businessSkillPropertySumCustLen);

		businessSkillPropertySum.Add(_businessSkillPropertySum);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(intimacy);
	_buf.putLong(charm);
	_buf.putLong(charmPoint);
	_buf.putInt(fetters.GetBufSize());
	fetters.PutUnzipBuf(_buf);
	_buf.putShort((short)blessSkillList.Count);
	for(int _i = 0; _i < blessSkillList.Count; _i++) { 
		_buf.putInt(blessSkillList[_i].GetBufSize());
	blessSkillList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)triggeredCallStoryIdList.Count);
	for(int _i = 0; _i < triggeredCallStoryIdList.Count; _i++) { 
		_buf.putLong(triggeredCallStoryIdList[_i]);
	}
	_buf.putShort((short)skinList.Count);
	for(int _i = 0; _i < skinList.Count; _i++) { 
		_buf.putInt(skinList[_i].GetBufSize());
	skinList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(curSkinId);
	_buf.put(isUnlockHalo?(byte)1:(byte)0);
	_buf.putInt(halo.GetBufSize());
	halo.PutUnzipBuf(_buf);
	_buf.putInt(opBusinessSkillCount);
	_buf.putShort((short)businessSkillPropertySum.Count);
	for(int _i = 0; _i < businessSkillPropertySum.Count; _i++) { 
		_buf.putInt(businessSkillPropertySum[_i].GetBufSize());
	businessSkillPropertySum[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("intimacy").Append(":").Append(intimacy.ToString()).Append(", ");
	builder.Append("charm").Append(":").Append(charm.ToString()).Append(", ");
	builder.Append("charmPoint").Append(":").Append(charmPoint.ToString()).Append(", ");
	builder.Append("fetters").Append(":").Append(fetters == null ? "null" : fetters.ToString()).Append(", ");
	builder.Append("blessSkillList").Append(":").Append(blessSkillList.ToString()).Append(", ");
	builder.Append("triggeredCallStoryIdList").Append(":").Append(triggeredCallStoryIdList.ToString()).Append(", ");
	builder.Append("skinList").Append(":").Append(skinList.ToString()).Append(", ");
	builder.Append("curSkinId").Append(":").Append(curSkinId.ToString()).Append(", ");
	builder.Append("isUnlockHalo").Append(":").Append(isUnlockHalo.ToString()).Append(", ");
	builder.Append("halo").Append(":").Append(halo == null ? "null" : halo.ToString()).Append(", ");
	builder.Append("opBusinessSkillCount").Append(":").Append(opBusinessSkillCount.ToString()).Append(", ");
	builder.Append("businessSkillPropertySum").Append(":").Append(businessSkillPropertySum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

