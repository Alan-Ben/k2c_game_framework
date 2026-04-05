using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-领悟经营技能等级
/// </summary>
public class GC2GS_015_002_ReqUnderstandBusinessSkill : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 经营技能ID
/// </summary>
private long skillId;
/// <summary>
/// 是否高级领悟
/// </summary>
private bool isAdvanced;


public GC2GS_015_002_ReqUnderstandBusinessSkill() {
	consortId = (long)0;
	skillId = (long)0;
	isAdvanced = false;
}

public GC2GS_015_002_ReqUnderstandBusinessSkill(
	long _consortId
	, long _skillId
	, bool _isAdvanced
) {	consortId = _consortId;
	skillId = _skillId;
	isAdvanced = _isAdvanced;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 经营技能ID
/// </summary>
public long getSkillId() { return skillId; }
/// <summary>
/// 经营技能ID
/// </summary>
public void setSkillId(long _skillId) { skillId = _skillId; }
/// <summary>
/// 是否高级领悟
/// </summary>
public bool getIsAdvanced() { return isAdvanced; }
/// <summary>
/// 是否高级领悟
/// </summary>
public void setIsAdvanced(bool _isAdvanced) { isAdvanced = _isAdvanced; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAdvanced = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(skillId);
	_buf.put(isAdvanced?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)2);
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
	builder.Append("skillId").Append(":").Append(skillId.ToString()).Append(", ");
	builder.Append("isAdvanced").Append(":").Append(isAdvanced.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

