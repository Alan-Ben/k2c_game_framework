using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CollegeObj
{

/// <summary>
/// 大学学习结束信息
/// </summary>
public class College_StudySettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 资质经验
/// </summary>
private long talentExp;
/// <summary>
/// 技能点
/// </summary>
private long skillPoint;


public College_StudySettleInfo() {
	heroId = (long)0;
	talentExp = (long)0;
	skillPoint = (long)0;
}

public College_StudySettleInfo(
	long _heroId
	, long _talentExp
	, long _skillPoint
) {	heroId = _heroId;
	talentExp = _talentExp;
	skillPoint = _skillPoint;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 资质经验
/// </summary>
public long getTalentExp() { return talentExp; }
/// <summary>
/// 资质经验
/// </summary>
public void setTalentExp(long _talentExp) { talentExp = _talentExp; }
/// <summary>
/// 技能点
/// </summary>
public long getSkillPoint() { return skillPoint; }
/// <summary>
/// 技能点
/// </summary>
public void setSkillPoint(long _skillPoint) { skillPoint = _skillPoint; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	talentExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillPoint = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putLong(talentExp);
	_buf.putLong(skillPoint);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("talentExp").Append(":").Append(talentExp.ToString()).Append(", ");
	builder.Append("skillPoint").Append(":").Append(skillPoint.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

