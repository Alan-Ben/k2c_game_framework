using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣经营技能变更
/// </summary>
public class GS2GC_013_054_OnHeroBusinessSkillChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 经营技能信息
/// </summary>
private Common.HeroObj.Hero_BusinessSkillInfo businessSkillInfo;


public GS2GC_013_054_OnHeroBusinessSkillChg() {
	heroId = (long)0;
	businessSkillInfo = new Common.HeroObj.Hero_BusinessSkillInfo();
}

public GS2GC_013_054_OnHeroBusinessSkillChg(
	long _heroId
	, Common.HeroObj.Hero_BusinessSkillInfo _businessSkillInfo
) {	heroId = _heroId;
	businessSkillInfo = _businessSkillInfo;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 经营技能信息
/// </summary>
public Common.HeroObj.Hero_BusinessSkillInfo getBusinessSkillInfo() { return businessSkillInfo; }
/// <summary>
/// 经营技能信息
/// </summary>
public void setBusinessSkillInfo(Common.HeroObj.Hero_BusinessSkillInfo _businessSkillInfo) { businessSkillInfo = _businessSkillInfo; }


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
	int _businessSkillInfoCustLen = _buf.getInt();
	int _businessSkillInfoCurPos = _buf.getCurPos();
	businessSkillInfo.ReadUnzipBuf(_buf, _businessSkillInfoCurPos + _businessSkillInfoCustLen);
	_buf.setPosition(_businessSkillInfoCurPos + _businessSkillInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(businessSkillInfo.GetBufSize());
	businessSkillInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)54);
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
	builder.Append("businessSkillInfo").Append(":").Append(businessSkillInfo == null ? "null" : businessSkillInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

