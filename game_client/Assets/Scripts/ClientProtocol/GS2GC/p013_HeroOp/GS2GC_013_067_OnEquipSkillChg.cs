using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 藏品技能信息变更
/// </summary>
public class GS2GC_013_067_OnEquipSkillChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 藏品数据id
/// </summary>
private long dbId;
private Common.HeroObj.Equip_SkillInfo skillInfo;


public GS2GC_013_067_OnEquipSkillChg() {
	dbId = (long)0;
	skillInfo = new Common.HeroObj.Equip_SkillInfo();
}

public GS2GC_013_067_OnEquipSkillChg(
	long _dbId
	, Common.HeroObj.Equip_SkillInfo _skillInfo
) {	dbId = _dbId;
	skillInfo = _skillInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)67; }

/// <summary>
/// 藏品数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 藏品数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
public Common.HeroObj.Equip_SkillInfo getSkillInfo() { return skillInfo; }
public void setSkillInfo(Common.HeroObj.Equip_SkillInfo _skillInfo) { skillInfo = _skillInfo; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _skillInfoCustLen = _buf.getInt();
	int _skillInfoCurPos = _buf.getCurPos();
	skillInfo.ReadUnzipBuf(_buf, _skillInfoCurPos + _skillInfoCustLen);
	_buf.setPosition(_skillInfoCurPos + _skillInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putInt(skillInfo.GetBufSize());
	skillInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)67);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("skillInfo").Append(":").Append(skillInfo == null ? "null" : skillInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

