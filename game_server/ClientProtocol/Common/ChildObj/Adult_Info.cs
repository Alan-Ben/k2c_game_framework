using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 成年子嗣数据
/// </summary>
public class Adult_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣ID
/// </summary>
private long id;
/// <summary>
/// 关联家人ID
/// </summary>
private long consortId;
/// <summary>
/// 子嗣初始形象配置ID
/// </summary>
private long initResId;
/// <summary>
/// 子嗣资质
/// </summary>
private long quality;
/// <summary>
/// 子嗣相性
/// </summary>
private CommonEnum.ESpecAttrType attrType;
/// <summary>
/// 子嗣职业配置ID
/// </summary>
private long careerId;
/// <summary>
/// 是否卷王
/// </summary>
private bool isGiftde;
/// <summary>
/// 初始亲密度
/// </summary>
private long initIntimacy;
/// <summary>
/// 子嗣名称
/// </summary>
private string name;
/// <summary>
/// 子嗣收益（上课收益+毕业收益）
/// </summary>
private long bonus;
/// <summary>
/// 毕业时间戳（秒）
/// </summary>
private int graduateTs;
/// <summary>
/// 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus
/// </summary>
private int initStudyBonus;


public Adult_Info() {
	id = (long)0;
	consortId = (long)0;
	initResId = (long)0;
	quality = (long)0;
	attrType = 0;
	careerId = (long)0;
	isGiftde = false;
	initIntimacy = (long)0;
	name = "";
	bonus = (long)0;
	graduateTs = 0;
	initStudyBonus = 0;
}

public Adult_Info(
	long _id
	, long _consortId
	, long _initResId
	, long _quality
	, CommonEnum.ESpecAttrType _attrType
	, long _careerId
	, bool _isGiftde
	, long _initIntimacy
	, string _name
	, long _bonus
	, int _graduateTs
	, int _initStudyBonus
) {	id = _id;
	consortId = _consortId;
	initResId = _initResId;
	quality = _quality;
	attrType = _attrType;
	careerId = _careerId;
	isGiftde = _isGiftde;
	initIntimacy = _initIntimacy;
	name = _name;
	bonus = _bonus;
	graduateTs = _graduateTs;
	initStudyBonus = _initStudyBonus;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 子嗣ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 子嗣ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 关联家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 关联家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 子嗣初始形象配置ID
/// </summary>
public long getInitResId() { return initResId; }
/// <summary>
/// 子嗣初始形象配置ID
/// </summary>
public void setInitResId(long _initResId) { initResId = _initResId; }
/// <summary>
/// 子嗣资质
/// </summary>
public long getQuality() { return quality; }
/// <summary>
/// 子嗣资质
/// </summary>
public void setQuality(long _quality) { quality = _quality; }
/// <summary>
/// 子嗣相性
/// </summary>
public CommonEnum.ESpecAttrType getAttrType() { return attrType; }
/// <summary>
/// 子嗣相性
/// </summary>
public void setAttrType(CommonEnum.ESpecAttrType _attrType) { attrType = _attrType; }
/// <summary>
/// 子嗣职业配置ID
/// </summary>
public long getCareerId() { return careerId; }
/// <summary>
/// 子嗣职业配置ID
/// </summary>
public void setCareerId(long _careerId) { careerId = _careerId; }
/// <summary>
/// 是否卷王
/// </summary>
public bool getIsGiftde() { return isGiftde; }
/// <summary>
/// 是否卷王
/// </summary>
public void setIsGiftde(bool _isGiftde) { isGiftde = _isGiftde; }
/// <summary>
/// 初始亲密度
/// </summary>
public long getInitIntimacy() { return initIntimacy; }
/// <summary>
/// 初始亲密度
/// </summary>
public void setInitIntimacy(long _initIntimacy) { initIntimacy = _initIntimacy; }
/// <summary>
/// 子嗣名称
/// </summary>
public string getName() { return name; }
/// <summary>
/// 子嗣名称
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 子嗣收益（上课收益+毕业收益）
/// </summary>
public long getBonus() { return bonus; }
/// <summary>
/// 子嗣收益（上课收益+毕业收益）
/// </summary>
public void setBonus(long _bonus) { bonus = _bonus; }
/// <summary>
/// 毕业时间戳（秒）
/// </summary>
public int getGraduateTs() { return graduateTs; }
/// <summary>
/// 毕业时间戳（秒）
/// </summary>
public void setGraduateTs(int _graduateTs) { graduateTs = _graduateTs; }
/// <summary>
/// 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus
/// </summary>
public int getInitStudyBonus() { return initStudyBonus; }
/// <summary>
/// 子嗣初始教学经验加成（万分比），来源：consort_fetters_lvl.study_bonus
/// </summary>
public void setInitStudyBonus(int _initStudyBonus) { initStudyBonus = _initStudyBonus; }


public int GetBufSize() {
	int _size = 69;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 71;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	initResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	quality = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attrType = (CommonEnum.ESpecAttrType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	careerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isGiftde = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	initIntimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	graduateTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	initStudyBonus = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(consortId);
	_buf.putLong(initResId);
	_buf.putLong(quality);
	_buf.putInt((int)attrType);

	_buf.putLong(careerId);
	_buf.put(isGiftde?(byte)1:(byte)0);
	_buf.putLong(initIntimacy);
	_buf.putString(name);
	_buf.putLong(bonus);
	_buf.putInt(graduateTs);
	_buf.putInt(initStudyBonus);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("initResId").Append(":").Append(initResId.ToString()).Append(", ");
	builder.Append("quality").Append(":").Append(quality.ToString()).Append(", ");
	builder.Append("attrType").Append(":").Append(attrType.ToString()).Append(", ");
	builder.Append("careerId").Append(":").Append(careerId.ToString()).Append(", ");
	builder.Append("isGiftde").Append(":").Append(isGiftde.ToString()).Append(", ");
	builder.Append("initIntimacy").Append(":").Append(initIntimacy.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("bonus").Append(":").Append(bonus.ToString()).Append(", ");
	builder.Append("graduateTs").Append(":").Append(graduateTs.ToString()).Append(", ");
	builder.Append("initStudyBonus").Append(":").Append(initStudyBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

