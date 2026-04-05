using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-联盟战报索引数据
/// </summary>
public class Mars_GuildBattleReportIdx : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 战报数据库ID
/// </summary>
private long id;
/// <summary>
/// 发起战斗的玩家CID
/// </summary>
private long cid;
/// <summary>
/// 日志类型
/// </summary>
private Common.MarsEnum.EMarsExplorePVPLogType logType;
/// <summary>
/// 创建时间（毫秒）
/// </summary>
private long createdAt;
/// <summary>
/// 战报数据
/// </summary>
private byte[] logData;


public Mars_GuildBattleReportIdx() {
	id = (long)0;
	cid = (long)0;
	logType = 0;
	createdAt = (long)0;
	logData = null;
}

public Mars_GuildBattleReportIdx(
	long _id
	, long _cid
	, Common.MarsEnum.EMarsExplorePVPLogType _logType
	, long _createdAt
	, byte[] _logData
) {	id = _id;
	cid = _cid;
	logType = _logType;
	createdAt = _createdAt;
	logData = _logData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 战报数据库ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 战报数据库ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 发起战斗的玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 发起战斗的玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 日志类型
/// </summary>
public Common.MarsEnum.EMarsExplorePVPLogType getLogType() { return logType; }
/// <summary>
/// 日志类型
/// </summary>
public void setLogType(Common.MarsEnum.EMarsExplorePVPLogType _logType) { logType = _logType; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public long getCreatedAt() { return createdAt; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }
/// <summary>
/// 战报数据
/// </summary>
public byte[] getLogData() { return logData; }

/// <summary>
/// 战报数据
/// </summary>
public void setLogData(byte[] _logData) { logData = _logData; }



public int GetBufSize() {
	int _size = 28;
	_size += 4 + (logData == null ? 0 : logData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 4 + (logData == null ? 0 : logData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logType = (Common.MarsEnum.EMarsExplorePVPLogType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createdAt = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(cid);
	_buf.putInt((int)logType);

	_buf.putLong(createdAt);
	_buf.putByteBuffer(logData);

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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("logType").Append(":").Append(logType.ToString()).Append(", ");
	builder.Append("createdAt").Append(":").Append(createdAt.ToString()).Append(", ");
	builder.Append("logData").Append(":").Append(logData == null ? "null" : logData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

