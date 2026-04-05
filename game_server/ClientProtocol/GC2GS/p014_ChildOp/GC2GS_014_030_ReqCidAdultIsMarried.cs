using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 请求玩家子嗣是否结婚的信息
/// </summary>
public class GC2GS_014_030_ReqCidAdultIsMarried : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long adultId;


public GC2GS_014_030_ReqCidAdultIsMarried() {
	cid = (long)0;
	adultId = (long)0;
}

public GC2GS_014_030_ReqCidAdultIsMarried(
	long _cid
	, long _adultId
) {	cid = _cid;
	adultId = _adultId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)30; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getAdultId() { return adultId; }
public void setAdultId(long _adultId) { adultId = _adultId; }


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
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	adultId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(adultId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)30);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("adultId").Append(":").Append(adultId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

