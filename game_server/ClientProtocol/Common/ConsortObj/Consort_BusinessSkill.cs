using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人经营技能
/// </summary>
public class Consort_BusinessSkill : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 技能ID
/// </summary>
private long skillId;
/// <summary>
/// 概率加成
/// </summary>
private long proAdd;
/// <summary>
/// 普通领悟次数
/// </summary>
private int normalOpCount;
/// <summary>
/// 高级领悟次数
/// </summary>
private int advanceOpCount;


public Consort_BusinessSkill() {
	skillId = (long)0;
	proAdd = (long)0;
	normalOpCount = 0;
	advanceOpCount = 0;
}

public Consort_BusinessSkill(
	long _skillId
	, long _proAdd
	, int _normalOpCount
	, int _advanceOpCount
) {	skillId = _skillId;
	proAdd = _proAdd;
	normalOpCount = _normalOpCount;
	advanceOpCount = _advanceOpCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 技能ID
/// </summary>
public long getSkillId() { return skillId; }
/// <summary>
/// 技能ID
/// </summary>
public void setSkillId(long _skillId) { skillId = _skillId; }
/// <summary>
/// 概率加成
/// </summary>
public long getProAdd() { return proAdd; }
/// <summary>
/// 概率加成
/// </summary>
public void setProAdd(long _proAdd) { proAdd = _proAdd; }
/// <summary>
/// 普通领悟次数
/// </summary>
public int getNormalOpCount() { return normalOpCount; }
/// <summary>
/// 普通领悟次数
/// </summary>
public void setNormalOpCount(int _normalOpCount) { normalOpCount = _normalOpCount; }
/// <summary>
/// 高级领悟次数
/// </summary>
public int getAdvanceOpCount() { return advanceOpCount; }
/// <summary>
/// 高级领悟次数
/// </summary>
public void setAdvanceOpCount(int _advanceOpCount) { advanceOpCount = _advanceOpCount; }


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
	skillId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	proAdd = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	normalOpCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	advanceOpCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(skillId);
	_buf.putLong(proAdd);
	_buf.putInt(normalOpCount);
	_buf.putInt(advanceOpCount);
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
	builder.Append("skillId").Append(":").Append(skillId.ToString()).Append(", ");
	builder.Append("proAdd").Append(":").Append(proAdd.ToString()).Append(", ");
	builder.Append("normalOpCount").Append(":").Append(normalOpCount.ToString()).Append(", ");
	builder.Append("advanceOpCount").Append(":").Append(advanceOpCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

