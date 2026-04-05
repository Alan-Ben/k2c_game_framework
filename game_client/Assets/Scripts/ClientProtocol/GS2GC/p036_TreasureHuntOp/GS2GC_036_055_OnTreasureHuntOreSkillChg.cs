using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-矿石技能变更
/// </summary>
public class GS2GC_036_055_OnTreasureHuntOreSkillChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// true普通技能 false高级技能
/// </summary>
private bool isNormal;
/// <summary>
/// 技能信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo skillInfo;


public GS2GC_036_055_OnTreasureHuntOreSkillChg() {
	oreId = (long)0;
	isNormal = false;
	skillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
}

public GS2GC_036_055_OnTreasureHuntOreSkillChg(
	long _oreId
	, bool _isNormal
	, Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _skillInfo
) {	oreId = _oreId;
	isNormal = _isNormal;
	skillInfo = _skillInfo;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// true普通技能 false高级技能
/// </summary>
public bool getIsNormal() { return isNormal; }
/// <summary>
/// true普通技能 false高级技能
/// </summary>
public void setIsNormal(bool _isNormal) { isNormal = _isNormal; }
/// <summary>
/// 技能信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getSkillInfo() { return skillInfo; }
/// <summary>
/// 技能信息
/// </summary>
public void setSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _skillInfo) { skillInfo = _skillInfo; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormal = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _skillInfoCustLen = _buf.getInt();
	int _skillInfoCurPos = _buf.getCurPos();
	skillInfo.ReadUnzipBuf(_buf, _skillInfoCurPos + _skillInfoCustLen);
	_buf.setPosition(_skillInfoCurPos + _skillInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.put(isNormal?(byte)1:(byte)0);
	_buf.putInt(skillInfo.GetBufSize());
	skillInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)55);
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
	builder.Append("oreId").Append(":").Append(oreId.ToString()).Append(", ");
	builder.Append("isNormal").Append(":").Append(isNormal.ToString()).Append(", ");
	builder.Append("skillInfo").Append(":").Append(skillInfo == null ? "null" : skillInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

