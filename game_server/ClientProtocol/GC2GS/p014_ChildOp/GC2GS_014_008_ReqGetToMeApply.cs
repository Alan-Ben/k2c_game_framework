using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 获取对玩家的指定联姻请求
/// </summary>
public class GC2GS_014_008_ReqGetToMeApply : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 发起请求的玩家子嗣实例ID
/// </summary>
private long applyAdultId;


public GC2GS_014_008_ReqGetToMeApply() {
	applyAdultId = (long)0;
}

public GC2GS_014_008_ReqGetToMeApply(
	long _applyAdultId
) {	applyAdultId = _applyAdultId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 发起请求的玩家子嗣实例ID
/// </summary>
public long getApplyAdultId() { return applyAdultId; }
/// <summary>
/// 发起请求的玩家子嗣实例ID
/// </summary>
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


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
	applyAdultId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(applyAdultId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)8);
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
	builder.Append("applyAdultId").Append(":").Append(applyAdultId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

