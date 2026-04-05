using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟加入请求信息
/// </summary>
public class Guild_JoinRequestInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
private long cid;
/// <summary>
/// 请求入盟时间
/// </summary>
private long requestTimeMs;


public Guild_JoinRequestInfo() {
	dbId = (long)0;
	cid = (long)0;
	requestTimeMs = (long)0;
}

public Guild_JoinRequestInfo(
	long _dbId
	, long _cid
	, long _requestTimeMs
) {	dbId = _dbId;
	cid = _cid;
	requestTimeMs = _requestTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 请求入盟时间
/// </summary>
public long getRequestTimeMs() { return requestTimeMs; }
/// <summary>
/// 请求入盟时间
/// </summary>
public void setRequestTimeMs(long _requestTimeMs) { requestTimeMs = _requestTimeMs; }


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
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	requestTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(cid);
	_buf.putLong(requestTimeMs);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("requestTimeMs").Append(":").Append(requestTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

