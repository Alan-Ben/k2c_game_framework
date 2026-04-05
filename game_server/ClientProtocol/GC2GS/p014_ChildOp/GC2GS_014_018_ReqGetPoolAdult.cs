using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 获取联姻池待匹配子嗣数据
/// </summary>
public class GC2GS_014_018_ReqGetPoolAdult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家CID
/// </summary>
private long cid;
/// <summary>
/// 子嗣实例ID
/// </summary>
private long adultId;


public GC2GS_014_018_ReqGetPoolAdult() {
	cid = (long)0;
	adultId = (long)0;
}

public GC2GS_014_018_ReqGetPoolAdult(
	long _cid
	, long _adultId
) {	cid = _cid;
	adultId = _adultId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)18; }

/// <summary>
/// 玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 子嗣实例ID
/// </summary>
public long getAdultId() { return adultId; }
/// <summary>
/// 子嗣实例ID
/// </summary>
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
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)18);
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

