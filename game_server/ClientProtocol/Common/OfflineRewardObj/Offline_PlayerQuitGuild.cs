using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 玩家退出联盟
/// </summary>
public class Offline_PlayerQuitGuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 原联盟ID
/// </summary>
private long oriGuildId;
private string oriGuildName;
private long timestamp;
/// <summary>
/// 是否被踢出
/// </summary>
private bool isKick;


public Offline_PlayerQuitGuild() {
	oriGuildId = (long)0;
	oriGuildName = "";
	timestamp = (long)0;
	isKick = false;
}

public Offline_PlayerQuitGuild(
	long _oriGuildId
	, string _oriGuildName
	, long _timestamp
	, bool _isKick
) {	oriGuildId = _oriGuildId;
	oriGuildName = _oriGuildName;
	timestamp = _timestamp;
	isKick = _isKick;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 原联盟ID
/// </summary>
public long getOriGuildId() { return oriGuildId; }
/// <summary>
/// 原联盟ID
/// </summary>
public void setOriGuildId(long _oriGuildId) { oriGuildId = _oriGuildId; }
public string getOriGuildName() { return oriGuildName; }
public void setOriGuildName(string _oriGuildName) { oriGuildName = _oriGuildName; }
public long getTimestamp() { return timestamp; }
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }
/// <summary>
/// 是否被踢出
/// </summary>
public bool getIsKick() { return isKick; }
/// <summary>
/// 是否被踢出
/// </summary>
public void setIsKick(bool _isKick) { isKick = _isKick; }


public int GetBufSize() {
	int _size = 17;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriGuildName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriGuildName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oriGuildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oriGuildName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timestamp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isKick = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oriGuildId);
	_buf.putString(oriGuildName);
	_buf.putLong(timestamp);
	_buf.put(isKick?(byte)1:(byte)0);
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
	builder.Append("oriGuildId").Append(":").Append(oriGuildId.ToString()).Append(", ");
	builder.Append("oriGuildName").Append(":").Append(oriGuildName.ToString()).Append(", ");
	builder.Append("timestamp").Append(":").Append(timestamp.ToString()).Append(", ");
	builder.Append("isKick").Append(":").Append(isKick.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

