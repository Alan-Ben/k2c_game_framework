using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 午间副本宝箱
/// </summary>
public class ChatObj_MiddayDungeonBox : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 宝箱ID
/// </summary>
private long boxId;
/// <summary>
/// 过期时间
/// </summary>
private long expiredTimeMs;


public ChatObj_MiddayDungeonBox() {
	dbId = (long)0;
	boxId = (long)0;
	expiredTimeMs = (long)0;
}

public ChatObj_MiddayDungeonBox(
	long _dbId
	, long _boxId
	, long _expiredTimeMs
) {	dbId = _dbId;
	boxId = _boxId;
	expiredTimeMs = _expiredTimeMs;
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
/// 宝箱ID
/// </summary>
public long getBoxId() { return boxId; }
/// <summary>
/// 宝箱ID
/// </summary>
public void setBoxId(long _boxId) { boxId = _boxId; }
/// <summary>
/// 过期时间
/// </summary>
public long getExpiredTimeMs() { return expiredTimeMs; }
/// <summary>
/// 过期时间
/// </summary>
public void setExpiredTimeMs(long _expiredTimeMs) { expiredTimeMs = _expiredTimeMs; }


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
	boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(boxId);
	_buf.putLong(expiredTimeMs);
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
	builder.Append("boxId").Append(":").Append(boxId.ToString()).Append(", ");
	builder.Append("expiredTimeMs").Append(":").Append(expiredTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

