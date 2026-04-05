using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟活跃宝箱信息
/// </summary>
public class Guild_ActiveBoxInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 发送者cid
/// </summary>
private long senderCid;
/// <summary>
/// 发放时间
/// </summary>
private long sendTimeMs;
/// <summary>
/// 印章数量
/// </summary>
private long stamp;


public Guild_ActiveBoxInfo() {
	dbId = (long)0;
	senderCid = (long)0;
	sendTimeMs = (long)0;
	stamp = (long)0;
}

public Guild_ActiveBoxInfo(
	long _dbId
	, long _senderCid
	, long _sendTimeMs
	, long _stamp
) {	dbId = _dbId;
	senderCid = _senderCid;
	sendTimeMs = _sendTimeMs;
	stamp = _stamp;
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
/// <summary>
/// 发送者cid
/// </summary>
public long getSenderCid() { return senderCid; }
/// <summary>
/// 发送者cid
/// </summary>
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
/// <summary>
/// 发放时间
/// </summary>
public long getSendTimeMs() { return sendTimeMs; }
/// <summary>
/// 发放时间
/// </summary>
public void setSendTimeMs(long _sendTimeMs) { sendTimeMs = _sendTimeMs; }
/// <summary>
/// 印章数量
/// </summary>
public long getStamp() { return stamp; }
/// <summary>
/// 印章数量
/// </summary>
public void setStamp(long _stamp) { stamp = _stamp; }


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
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sendTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stamp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(senderCid);
	_buf.putLong(sendTimeMs);
	_buf.putLong(stamp);
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
	builder.Append("senderCid").Append(":").Append(senderCid.ToString()).Append(", ");
	builder.Append("sendTimeMs").Append(":").Append(sendTimeMs.ToString()).Append(", ");
	builder.Append("stamp").Append(":").Append(stamp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

