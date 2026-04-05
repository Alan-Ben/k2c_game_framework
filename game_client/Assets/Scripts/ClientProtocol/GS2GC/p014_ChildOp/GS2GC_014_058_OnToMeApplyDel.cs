using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 对自身指定请求移除推送
/// </summary>
public class GS2GC_014_058_OnToMeApplyDel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 请求子嗣实例ID
/// </summary>
private long applyAdultId;


public GS2GC_014_058_OnToMeApplyDel() {
	applyAdultId = (long)0;
}

public GS2GC_014_058_OnToMeApplyDel(
	long _applyAdultId
) {	applyAdultId = _applyAdultId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 请求子嗣实例ID
/// </summary>
public long getApplyAdultId() { return applyAdultId; }
/// <summary>
/// 请求子嗣实例ID
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
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)58);
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

