using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 火星探险矿分享
/// </summary>
public class NPCommon_ChatContent_MarsExploreMineShare : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟分享矿消息id
/// </summary>
private long guildShareMineMsgId;
/// <summary>
/// 矿配表id
/// </summary>
private long refId;


public NPCommon_ChatContent_MarsExploreMineShare() {
	guildShareMineMsgId = (long)0;
	refId = (long)0;
}

public NPCommon_ChatContent_MarsExploreMineShare(
	long _guildShareMineMsgId
	, long _refId
) {	guildShareMineMsgId = _guildShareMineMsgId;
	refId = _refId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 联盟分享矿消息id
/// </summary>
public long getGuildShareMineMsgId() { return guildShareMineMsgId; }
/// <summary>
/// 联盟分享矿消息id
/// </summary>
public void setGuildShareMineMsgId(long _guildShareMineMsgId) { guildShareMineMsgId = _guildShareMineMsgId; }
/// <summary>
/// 矿配表id
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 矿配表id
/// </summary>
public void setRefId(long _refId) { refId = _refId; }


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
	guildShareMineMsgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guildShareMineMsgId);
	_buf.putLong(refId);
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
	builder.Append("guildShareMineMsgId").Append(":").Append(guildShareMineMsgId.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

