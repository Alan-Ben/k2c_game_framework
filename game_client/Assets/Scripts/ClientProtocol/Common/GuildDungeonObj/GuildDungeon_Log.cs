using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本日志
/// </summary>
public class GuildDungeon_Log : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日志类型
/// </summary>
private Common.GuildDungeonEnum.EGuildDungeon_LogType logType;
/// <summary>
/// 创建时间
/// </summary>
private int createdAt;
/// <summary>
/// 日志数据
/// </summary>
private byte[] info;


public GuildDungeon_Log() {
	logType = 0;
	createdAt = 0;
	info = null;
}

public GuildDungeon_Log(
	Common.GuildDungeonEnum.EGuildDungeon_LogType _logType
	, int _createdAt
	, byte[] _info
) {	logType = _logType;
	createdAt = _createdAt;
	info = _info;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日志类型
/// </summary>
public Common.GuildDungeonEnum.EGuildDungeon_LogType getLogType() { return logType; }
/// <summary>
/// 日志类型
/// </summary>
public void setLogType(Common.GuildDungeonEnum.EGuildDungeon_LogType _logType) { logType = _logType; }
/// <summary>
/// 创建时间
/// </summary>
public int getCreatedAt() { return createdAt; }
/// <summary>
/// 创建时间
/// </summary>
public void setCreatedAt(int _createdAt) { createdAt = _createdAt; }
/// <summary>
/// 日志数据
/// </summary>
public byte[] getInfo() { return info; }

/// <summary>
/// 日志数据
/// </summary>
public void setInfo(byte[] _info) { info = _info; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (info == null ? 0 : info.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (info == null ? 0 : info.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logType = (Common.GuildDungeonEnum.EGuildDungeon_LogType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createdAt = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	info = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)logType);

	_buf.putInt(createdAt);
	_buf.putByteBuffer(info);

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
	builder.Append("logType").Append(":").Append(logType.ToString()).Append(", ");
	builder.Append("createdAt").Append(":").Append(createdAt.ToString()).Append(", ");
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

