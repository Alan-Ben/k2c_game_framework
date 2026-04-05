using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 藏品信息
/// </summary>
public class Equip_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础信息
/// </summary>
private Common.HeroObj.Equip_BaseInfo baseInfo;
/// <summary>
/// 技能列表
/// </summary>
private List<Common.HeroObj.Equip_SkillInfo> skillList;


public Equip_Info() {
	baseInfo = new Common.HeroObj.Equip_BaseInfo();
	skillList = new List<Common.HeroObj.Equip_SkillInfo>();
}

public Equip_Info(
	Common.HeroObj.Equip_BaseInfo _baseInfo
	, List<Common.HeroObj.Equip_SkillInfo> _skillList
) {	baseInfo = _baseInfo;
	skillList = _skillList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 基础信息
/// </summary>
public Common.HeroObj.Equip_BaseInfo getBaseInfo() { return baseInfo; }
/// <summary>
/// 基础信息
/// </summary>
public void setBaseInfo(Common.HeroObj.Equip_BaseInfo _baseInfo) { baseInfo = _baseInfo; }
/// <summary>
/// 技能列表
/// </summary>
public List<Common.HeroObj.Equip_SkillInfo> getSkillList() { return skillList; }
/// <summary>
/// 技能列表
/// </summary>
public void addSkillList(Common.HeroObj.Equip_SkillInfo _skillList) { skillList.Add(_skillList); }


public int GetBufSize() {
	int _size = 41;
	_size += 2 + (skillList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 43;
	_size += 2 + (skillList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.getCurPos();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.setPosition(_baseInfoCurPos + _baseInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _skillListCount = _buf.getShort();
	for(int _i = 0; _i < _skillListCount; _i++) { 
		Common.HeroObj.Equip_SkillInfo _skillList = new Common.HeroObj.Equip_SkillInfo();
		int __skillListCustLen = _buf.getInt();
	int __skillListCurPos = _buf.getCurPos();
	_skillList.ReadUnzipBuf(_buf, __skillListCurPos + __skillListCustLen);
	_buf.setPosition(__skillListCurPos + __skillListCustLen);

		skillList.Add(_skillList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)skillList.Count);
	for(int _i = 0; _i < skillList.Count; _i++) { 
		_buf.putInt(skillList[_i].GetBufSize());
	skillList[_i].PutUnzipBuf(_buf);
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
	builder.Append("baseInfo").Append(":").Append(baseInfo == null ? "null" : baseInfo.ToString()).Append(", ");
	builder.Append("skillList").Append(":").Append(skillList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

