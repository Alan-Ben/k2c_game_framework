using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人加护技能数据变更
/// </summary>
public class GS2GC_015_059_OnBlessSkillChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long consortId;
/// <summary>
/// 家人加护技能数据
/// </summary>
private Common.ConsortObj.Consort_BlessSkill skill;


public GS2GC_015_059_OnBlessSkillChg() {
	consortId = (long)0;
	skill = new Common.ConsortObj.Consort_BlessSkill();
}

public GS2GC_015_059_OnBlessSkillChg(
	long _consortId
	, Common.ConsortObj.Consort_BlessSkill _skill
) {	consortId = _consortId;
	skill = _skill;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 空
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 空
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 家人加护技能数据
/// </summary>
public Common.ConsortObj.Consort_BlessSkill getSkill() { return skill; }
/// <summary>
/// 家人加护技能数据
/// </summary>
public void setSkill(Common.ConsortObj.Consort_BlessSkill _skill) { skill = _skill; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _skillCustLen = _buf.getInt();
	int _skillCurPos = _buf.getCurPos();
	skill.ReadUnzipBuf(_buf, _skillCurPos + _skillCustLen);
	_buf.setPosition(_skillCurPos + _skillCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putInt(skill.GetBufSize());
	skill.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)59);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("skill").Append(":").Append(skill == null ? "null" : skill.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

