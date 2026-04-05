using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人经营技能属性汇总
/// </summary>
public class Consort_BusinessSkillPropertySum : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 相性
/// </summary>
private CommonEnum.ESpecAttrType attr;
/// <summary>
/// 概率加成
/// </summary>
private long proAddSum;


public Consort_BusinessSkillPropertySum() {
	attr = 0;
	proAddSum = (long)0;
}

public Consort_BusinessSkillPropertySum(
	CommonEnum.ESpecAttrType _attr
	, long _proAddSum
) {	attr = _attr;
	proAddSum = _proAddSum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 相性
/// </summary>
public CommonEnum.ESpecAttrType getAttr() { return attr; }
/// <summary>
/// 相性
/// </summary>
public void setAttr(CommonEnum.ESpecAttrType _attr) { attr = _attr; }
/// <summary>
/// 概率加成
/// </summary>
public long getProAddSum() { return proAddSum; }
/// <summary>
/// 概率加成
/// </summary>
public void setProAddSum(long _proAddSum) { proAddSum = _proAddSum; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attr = (CommonEnum.ESpecAttrType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	proAddSum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)attr);

	_buf.putLong(proAddSum);
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
	builder.Append("attr").Append(":").Append(attr.ToString()).Append(", ");
	builder.Append("proAddSum").Append(":").Append(proAddSum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

