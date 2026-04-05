using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_004_OnUSEnterDone : ALBasicProtocolPack._IALProtocolStructure {
private int errCode;
/// <summary>
/// 客户端登录操作的序列号
/// </summary>
private long clientInitSerialize;
/// <summary>
/// 服务器时间戳，用于心跳包处理
/// </summary>
private long serverTimeTag;
private int usId;
/// <summary>
/// 玩家cid
/// </summary>
private long cid;


public NPGS2GC_001_004_OnUSEnterDone() {
	errCode = 0;
	clientInitSerialize = (long)0;
	serverTimeTag = (long)0;
	usId = 0;
	cid = (long)0;
}

public NPGS2GC_001_004_OnUSEnterDone(
	int _errCode
	, long _clientInitSerialize
	, long _serverTimeTag
	, int _usId
	, long _cid
) {	errCode = _errCode;
	clientInitSerialize = _clientInitSerialize;
	serverTimeTag = _serverTimeTag;
	usId = _usId;
	cid = _cid;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)4; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
/// <summary>
/// 客户端登录操作的序列号
/// </summary>
public long getClientInitSerialize() { return clientInitSerialize; }
/// <summary>
/// 客户端登录操作的序列号
/// </summary>
public void setClientInitSerialize(long _clientInitSerialize) { clientInitSerialize = _clientInitSerialize; }
/// <summary>
/// 服务器时间戳，用于心跳包处理
/// </summary>
public long getServerTimeTag() { return serverTimeTag; }
/// <summary>
/// 服务器时间戳，用于心跳包处理
/// </summary>
public void setServerTimeTag(long _serverTimeTag) { serverTimeTag = _serverTimeTag; }
public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/// <summary>
/// 玩家cid
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家cid
/// </summary>
public void setCid(long _cid) { cid = _cid; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientInitSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(clientInitSerialize);
	_buf.putLong(serverTimeTag);
	_buf.putInt(usId);
	_buf.putLong(cid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("clientInitSerialize").Append(":").Append(clientInitSerialize.ToString()).Append(", ");
	builder.Append("serverTimeTag").Append(":").Append(serverTimeTag.ToString()).Append(", ");
	builder.Append("usId").Append(":").Append(usId.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

