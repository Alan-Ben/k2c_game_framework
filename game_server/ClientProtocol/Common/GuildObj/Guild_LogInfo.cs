using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟日志信息
/// </summary>
public class Guild_LogInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日志类型
/// </summary>
private Common.GuildEnum.EGuildLogType logType;
/// <summary>
/// 数据
/// </summary>
private byte[] data;
/// <summary>
/// 时间戳
/// </summary>
private long sendTimeMs;


public Guild_LogInfo() {
	logType = 0;
	data = null;
	sendTimeMs = (long)0;
}

public Guild_LogInfo(
	Common.GuildEnum.EGuildLogType _logType
	, byte[] _data
	, long _sendTimeMs
) {	logType = _logType;
	data = _data;
	sendTimeMs = _sendTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日志类型
/// </summary>
public Common.GuildEnum.EGuildLogType getLogType() { return logType; }
/// <summary>
/// 日志类型
/// </summary>
public void setLogType(Common.GuildEnum.EGuildLogType _logType) { logType = _logType; }
/// <summary>
/// 数据
/// </summary>
public byte[] getData() { return data; }

/// <summary>
/// 数据
/// </summary>
public void setData(byte[] _data) { data = _data; }

/// <summary>
/// 时间戳
/// </summary>
public long getSendTimeMs() { return sendTimeMs; }
/// <summary>
/// 时间戳
/// </summary>
public void setSendTimeMs(long _sendTimeMs) { sendTimeMs = _sendTimeMs; }


public int GetBufSize() {
	int _size = 12;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (data == null ? 0 : data.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	logType = (Common.GuildEnum.EGuildLogType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	data = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sendTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)logType);

	_buf.putByteBuffer(data);

	_buf.putLong(sendTimeMs);
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
	builder.Append("data").Append(":").Append(data == null ? "null" : data.ToString()).Append(", ");
	builder.Append("sendTimeMs").Append(":").Append(sendTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

