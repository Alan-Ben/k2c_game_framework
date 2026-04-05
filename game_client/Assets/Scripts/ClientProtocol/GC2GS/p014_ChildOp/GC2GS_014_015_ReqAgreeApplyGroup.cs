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
public class GC2GS_014_015_ReqAgreeApplyGroup : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 匹配的子嗣实例ID
/// </summary>
private long adultId;
/// <summary>
/// 发起请求的玩家CID
/// </summary>
private long applyCid;
/// <summary>
/// 发起请求的子嗣实例ID
/// </summary>
private long applyAdultId;


public GC2GS_014_015_ReqAgreeApplyGroup() {
	adultId = (long)0;
	applyCid = (long)0;
	applyAdultId = (long)0;
}

public GC2GS_014_015_ReqAgreeApplyGroup(
	long _adultId
	, long _applyCid
	, long _applyAdultId
) {	adultId = _adultId;
	applyCid = _applyCid;
	applyAdultId = _applyAdultId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)15; }

/// <summary>
/// 匹配的子嗣实例ID
/// </summary>
public long getAdultId() { return adultId; }
/// <summary>
/// 匹配的子嗣实例ID
/// </summary>
public void setAdultId(long _adultId) { adultId = _adultId; }
/// <summary>
/// 发起请求的玩家CID
/// </summary>
public long getApplyCid() { return applyCid; }
/// <summary>
/// 发起请求的玩家CID
/// </summary>
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/// <summary>
/// 发起请求的子嗣实例ID
/// </summary>
public long getApplyAdultId() { return applyAdultId; }
/// <summary>
/// 发起请求的子嗣实例ID
/// </summary>
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


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
	adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyAdultId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(adultId);
	_buf.putLong(applyCid);
	_buf.putLong(applyAdultId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)15);
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
	builder.Append("applyCid").Append(":").Append(applyCid.ToString()).Append(", ");
	builder.Append("applyAdultId").Append(":").Append(applyAdultId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

