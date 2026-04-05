using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 子嗣分享
/// </summary>
public class NPCommon_ChatContent_ChildShare : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣名称
/// </summary>
private string childName;
/// <summary>
/// 监护人id
/// </summary>
private long consortId;
/// <summary>
/// 子嗣等级
/// </summary>
private int childLevel;
/// <summary>
/// 子嗣升学阶段
/// </summary>
private int childStep;
/// <summary>
/// 子嗣资源id
/// </summary>
private long childResId;
/// <summary>
/// 子嗣职业id
/// </summary>
private long childCareerId;
/// <summary>
/// 子嗣品质id
/// </summary>
private long childQualityId;
/// <summary>
/// 相性
/// </summary>
private CommonEnum.ESpecAttrType attrType;
/// <summary>
/// 收益
/// </summary>
private long earnings;
/// <summary>
/// 单次教学奖励
/// </summary>
private long educationExpValue;
/// <summary>
/// 基础奖励
/// </summary>
private long educatingBaseAwards;
/// <summary>
/// 天资加成
/// </summary>
private long childQualityBonus;
/// <summary>
/// 情人羁绊加成
/// </summary>
private long consortBonus;
/// <summary>
/// 顾问技能加成
/// </summary>
private long heroBonus;
/// <summary>
/// 是否卷王
/// </summary>
private bool isSuper;
private long cid;
/// <summary>
/// 是否毕业，未毕业的adultId无效
/// </summary>
private bool isGraduated;
private long adultId;


public NPCommon_ChatContent_ChildShare() {
	childName = "";
	consortId = (long)0;
	childLevel = 0;
	childStep = 0;
	childResId = (long)0;
	childCareerId = (long)0;
	childQualityId = (long)0;
	attrType = 0;
	earnings = (long)0;
	educationExpValue = (long)0;
	educatingBaseAwards = (long)0;
	childQualityBonus = (long)0;
	consortBonus = (long)0;
	heroBonus = (long)0;
	isSuper = false;
	cid = (long)0;
	isGraduated = false;
	adultId = (long)0;
}

public NPCommon_ChatContent_ChildShare(
	string _childName
	, long _consortId
	, int _childLevel
	, int _childStep
	, long _childResId
	, long _childCareerId
	, long _childQualityId
	, CommonEnum.ESpecAttrType _attrType
	, long _earnings
	, long _educationExpValue
	, long _educatingBaseAwards
	, long _childQualityBonus
	, long _consortBonus
	, long _heroBonus
	, bool _isSuper
	, long _cid
	, bool _isGraduated
	, long _adultId
) {	childName = _childName;
	consortId = _consortId;
	childLevel = _childLevel;
	childStep = _childStep;
	childResId = _childResId;
	childCareerId = _childCareerId;
	childQualityId = _childQualityId;
	attrType = _attrType;
	earnings = _earnings;
	educationExpValue = _educationExpValue;
	educatingBaseAwards = _educatingBaseAwards;
	childQualityBonus = _childQualityBonus;
	consortBonus = _consortBonus;
	heroBonus = _heroBonus;
	isSuper = _isSuper;
	cid = _cid;
	isGraduated = _isGraduated;
	adultId = _adultId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 子嗣名称
/// </summary>
public string getChildName() { return childName; }
/// <summary>
/// 子嗣名称
/// </summary>
public void setChildName(string _childName) { childName = _childName; }
/// <summary>
/// 监护人id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 监护人id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 子嗣等级
/// </summary>
public int getChildLevel() { return childLevel; }
/// <summary>
/// 子嗣等级
/// </summary>
public void setChildLevel(int _childLevel) { childLevel = _childLevel; }
/// <summary>
/// 子嗣升学阶段
/// </summary>
public int getChildStep() { return childStep; }
/// <summary>
/// 子嗣升学阶段
/// </summary>
public void setChildStep(int _childStep) { childStep = _childStep; }
/// <summary>
/// 子嗣资源id
/// </summary>
public long getChildResId() { return childResId; }
/// <summary>
/// 子嗣资源id
/// </summary>
public void setChildResId(long _childResId) { childResId = _childResId; }
/// <summary>
/// 子嗣职业id
/// </summary>
public long getChildCareerId() { return childCareerId; }
/// <summary>
/// 子嗣职业id
/// </summary>
public void setChildCareerId(long _childCareerId) { childCareerId = _childCareerId; }
/// <summary>
/// 子嗣品质id
/// </summary>
public long getChildQualityId() { return childQualityId; }
/// <summary>
/// 子嗣品质id
/// </summary>
public void setChildQualityId(long _childQualityId) { childQualityId = _childQualityId; }
/// <summary>
/// 相性
/// </summary>
public CommonEnum.ESpecAttrType getAttrType() { return attrType; }
/// <summary>
/// 相性
/// </summary>
public void setAttrType(CommonEnum.ESpecAttrType _attrType) { attrType = _attrType; }
/// <summary>
/// 收益
/// </summary>
public long getEarnings() { return earnings; }
/// <summary>
/// 收益
/// </summary>
public void setEarnings(long _earnings) { earnings = _earnings; }
/// <summary>
/// 单次教学奖励
/// </summary>
public long getEducationExpValue() { return educationExpValue; }
/// <summary>
/// 单次教学奖励
/// </summary>
public void setEducationExpValue(long _educationExpValue) { educationExpValue = _educationExpValue; }
/// <summary>
/// 基础奖励
/// </summary>
public long getEducatingBaseAwards() { return educatingBaseAwards; }
/// <summary>
/// 基础奖励
/// </summary>
public void setEducatingBaseAwards(long _educatingBaseAwards) { educatingBaseAwards = _educatingBaseAwards; }
/// <summary>
/// 天资加成
/// </summary>
public long getChildQualityBonus() { return childQualityBonus; }
/// <summary>
/// 天资加成
/// </summary>
public void setChildQualityBonus(long _childQualityBonus) { childQualityBonus = _childQualityBonus; }
/// <summary>
/// 情人羁绊加成
/// </summary>
public long getConsortBonus() { return consortBonus; }
/// <summary>
/// 情人羁绊加成
/// </summary>
public void setConsortBonus(long _consortBonus) { consortBonus = _consortBonus; }
/// <summary>
/// 顾问技能加成
/// </summary>
public long getHeroBonus() { return heroBonus; }
/// <summary>
/// 顾问技能加成
/// </summary>
public void setHeroBonus(long _heroBonus) { heroBonus = _heroBonus; }
/// <summary>
/// 是否卷王
/// </summary>
public bool getIsSuper() { return isSuper; }
/// <summary>
/// 是否卷王
/// </summary>
public void setIsSuper(bool _isSuper) { isSuper = _isSuper; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 是否毕业，未毕业的adultId无效
/// </summary>
public bool getIsGraduated() { return isGraduated; }
/// <summary>
/// 是否毕业，未毕业的adultId无效
/// </summary>
public void setIsGraduated(bool _isGraduated) { isGraduated = _isGraduated; }
public long getAdultId() { return adultId; }
public void setAdultId(long _adultId) { adultId = _adultId; }


public int GetBufSize() {
	int _size = 110;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(childName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 112;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(childName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childCareerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childQualityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attrType = (CommonEnum.ESpecAttrType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	educationExpValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	educatingBaseAwards = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childQualityBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSuper = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isGraduated = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	adultId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(childName);
	_buf.putLong(consortId);
	_buf.putInt(childLevel);
	_buf.putInt(childStep);
	_buf.putLong(childResId);
	_buf.putLong(childCareerId);
	_buf.putLong(childQualityId);
	_buf.putInt((int)attrType);

	_buf.putLong(earnings);
	_buf.putLong(educationExpValue);
	_buf.putLong(educatingBaseAwards);
	_buf.putLong(childQualityBonus);
	_buf.putLong(consortBonus);
	_buf.putLong(heroBonus);
	_buf.put(isSuper?(byte)1:(byte)0);
	_buf.putLong(cid);
	_buf.put(isGraduated?(byte)1:(byte)0);
	_buf.putLong(adultId);
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
	builder.Append("childName").Append(":").Append(childName.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("childLevel").Append(":").Append(childLevel.ToString()).Append(", ");
	builder.Append("childStep").Append(":").Append(childStep.ToString()).Append(", ");
	builder.Append("childResId").Append(":").Append(childResId.ToString()).Append(", ");
	builder.Append("childCareerId").Append(":").Append(childCareerId.ToString()).Append(", ");
	builder.Append("childQualityId").Append(":").Append(childQualityId.ToString()).Append(", ");
	builder.Append("attrType").Append(":").Append(attrType.ToString()).Append(", ");
	builder.Append("earnings").Append(":").Append(earnings.ToString()).Append(", ");
	builder.Append("educationExpValue").Append(":").Append(educationExpValue.ToString()).Append(", ");
	builder.Append("educatingBaseAwards").Append(":").Append(educatingBaseAwards.ToString()).Append(", ");
	builder.Append("childQualityBonus").Append(":").Append(childQualityBonus.ToString()).Append(", ");
	builder.Append("consortBonus").Append(":").Append(consortBonus.ToString()).Append(", ");
	builder.Append("heroBonus").Append(":").Append(heroBonus.ToString()).Append(", ");
	builder.Append("isSuper").Append(":").Append(isSuper.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("isGraduated").Append(":").Append(isGraduated.ToString()).Append(", ");
	builder.Append("adultId").Append(":").Append(adultId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

