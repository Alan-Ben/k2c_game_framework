using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 午间副本_宝箱信息
/// </summary>
public class MiddayDungeon_BoxInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 宝箱ID
/// </summary>
private long boxId;
/// <summary>
/// 发送者ID
/// </summary>
private long senderCid;
/// <summary>
/// 发送者名字
/// </summary>
private string senderName;
/// <summary>
/// 过期时间戳
/// </summary>
private long expireTimeMS;


public MiddayDungeon_BoxInfo() {
	dbId = (long)0;
	boxId = (long)0;
	senderCid = (long)0;
	senderName = "";
	expireTimeMS = (long)0;
}

public MiddayDungeon_BoxInfo(
	long _dbId
	, long _boxId
	, long _senderCid
	, string _senderName
	, long _expireTimeMS
) {	dbId = _dbId;
	boxId = _boxId;
	senderCid = _senderCid;
	senderName = _senderName;
	expireTimeMS = _expireTimeMS;
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
/// 发送者ID
/// </summary>
public long getSenderCid() { return senderCid; }
/// <summary>
/// 发送者ID
/// </summary>
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
/// <summary>
/// 发送者名字
/// </summary>
public string getSenderName() { return senderName; }
/// <summary>
/// 发送者名字
/// </summary>
public void setSenderName(string _senderName) { senderName = _senderName; }
/// <summary>
/// 过期时间戳
/// </summary>
public long getExpireTimeMS() { return expireTimeMS; }
/// <summary>
/// 过期时间戳
/// </summary>
public void setExpireTimeMS(long _expireTimeMS) { expireTimeMS = _expireTimeMS; }


public int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTimeMS = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(boxId);
	_buf.putLong(senderCid);
	_buf.putString(senderName);
	_buf.putLong(expireTimeMS);
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
	builder.Append("senderCid").Append(":").Append(senderCid.ToString()).Append(", ");
	builder.Append("senderName").Append(":").Append(senderName.ToString()).Append(", ");
	builder.Append("expireTimeMS").Append(":").Append(expireTimeMS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

