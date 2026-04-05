using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

public class GS2GC_013_027_RetEquipSkillList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 技能列表
/// </summary>
private List<Common.HeroObj.Equip_SkillInfo> skillList;


public GS2GC_013_027_RetEquipSkillList() {
	skillList = new List<Common.HeroObj.Equip_SkillInfo>();
}

public GS2GC_013_027_RetEquipSkillList(
	List<Common.HeroObj.Equip_SkillInfo> _skillList
) {	skillList = _skillList;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)27; }

/// <summary>
/// 技能列表
/// </summary>
public List<Common.HeroObj.Equip_SkillInfo> getSkillList() { return skillList; }
/// <summary>
/// 技能列表
/// </summary>
public void addSkillList(Common.HeroObj.Equip_SkillInfo _skillList) { skillList.Add(_skillList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (skillList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (skillList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
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
	_buf.putShort((short)skillList.Count);
	for(int _i = 0; _i < skillList.Count; _i++) { 
		_buf.putInt(skillList[_i].GetBufSize());
	skillList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)27);
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
	builder.Append("skillList").Append(":").Append(skillList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

