using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣资质技能变更
/// </summary>
public class GS2GC_013_052_OnHeroTalentSkillChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 资质技能信息
/// </summary>
private Common.HeroObj.Hero_TalentSkillInfo talentSkillInfo;


public GS2GC_013_052_OnHeroTalentSkillChg() {
	heroId = (long)0;
	talentSkillInfo = new Common.HeroObj.Hero_TalentSkillInfo();
}

public GS2GC_013_052_OnHeroTalentSkillChg(
	long _heroId
	, Common.HeroObj.Hero_TalentSkillInfo _talentSkillInfo
) {	heroId = _heroId;
	talentSkillInfo = _talentSkillInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 资质技能信息
/// </summary>
public Common.HeroObj.Hero_TalentSkillInfo getTalentSkillInfo() { return talentSkillInfo; }
/// <summary>
/// 资质技能信息
/// </summary>
public void setTalentSkillInfo(Common.HeroObj.Hero_TalentSkillInfo _talentSkillInfo) { talentSkillInfo = _talentSkillInfo; }


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
	int _talentSkillInfoCustLen = _buf.getInt();
	int _talentSkillInfoCurPos = _buf.getCurPos();
	talentSkillInfo.ReadUnzipBuf(_buf, _talentSkillInfoCurPos + _talentSkillInfoCustLen);
	_buf.setPosition(_talentSkillInfoCurPos + _talentSkillInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(talentSkillInfo.GetBufSize());
	talentSkillInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)52);
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
	builder.Append("talentSkillInfo").Append(":").Append(talentSkillInfo == null ? "null" : talentSkillInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

