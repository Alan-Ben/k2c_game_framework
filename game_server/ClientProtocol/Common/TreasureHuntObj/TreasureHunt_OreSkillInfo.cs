using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-矿石技能信息
/// </summary>
public class TreasureHunt_OreSkillInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 技能等级
/// </summary>
private int skillLevel;
/// <summary>
/// 技能点数
/// </summary>
private int skillPoint;


public TreasureHunt_OreSkillInfo() {
	skillLevel = 0;
	skillPoint = 0;
}

public TreasureHunt_OreSkillInfo(
	int _skillLevel
	, int _skillPoint
) {	skillLevel = _skillLevel;
	skillPoint = _skillPoint;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 技能等级
/// </summary>
public int getSkillLevel() { return skillLevel; }
/// <summary>
/// 技能等级
/// </summary>
public void setSkillLevel(int _skillLevel) { skillLevel = _skillLevel; }
/// <summary>
/// 技能点数
/// </summary>
public int getSkillPoint() { return skillPoint; }
/// <summary>
/// 技能点数
/// </summary>
public void setSkillPoint(int _skillPoint) { skillPoint = _skillPoint; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillPoint = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(skillLevel);
	_buf.putInt(skillPoint);
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
	builder.Append("skillLevel").Append(":").Append(skillLevel.ToString()).Append(", ");
	builder.Append("skillPoint").Append(":").Append(skillPoint.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

