using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 联姻请求-对指定群体发起请求
/// </summary>
public class GC2GS_014_014_ReqApplyToGroup : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣实例ID
/// </summary>
private long adultId;
/// <summary>
/// 最小比例最终值
/// </summary>
private long minValue;


public GC2GS_014_014_ReqApplyToGroup() {
	adultId = (long)0;
	minValue = (long)0;
}

public GC2GS_014_014_ReqApplyToGroup(
	long _adultId
	, long _minValue
) {	adultId = _adultId;
	minValue = _minValue;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)14; }

/// <summary>
/// 子嗣实例ID
/// </summary>
public long getAdultId() { return adultId; }
/// <summary>
/// 子嗣实例ID
/// </summary>
public void setAdultId(long _adultId) { adultId = _adultId; }
/// <summary>
/// 最小比例最终值
/// </summary>
public long getMinValue() { return minValue; }
/// <summary>
/// 最小比例最终值
/// </summary>
public void setMinValue(long _minValue) { minValue = _minValue; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	minValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(adultId);
	_buf.putLong(minValue);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)14);
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
	builder.Append("adultId").Append(":").Append(adultId.ToString()).Append(", ");
	builder.Append("minValue").Append(":").Append(minValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

