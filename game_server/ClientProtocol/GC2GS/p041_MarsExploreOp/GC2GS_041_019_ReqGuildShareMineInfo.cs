using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-获取联盟分享矿信息
/// </summary>
public class GC2GS_041_019_ReqGuildShareMineInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟分享矿信息消息ID
/// </summary>
private long guildShareMineMsgId;


public GC2GS_041_019_ReqGuildShareMineInfo() {
	guildShareMineMsgId = (long)0;
}

public GC2GS_041_019_ReqGuildShareMineInfo(
	long _guildShareMineMsgId
) {	guildShareMineMsgId = _guildShareMineMsgId;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)19; }

/// <summary>
/// 联盟分享矿信息消息ID
/// </summary>
public long getGuildShareMineMsgId() { return guildShareMineMsgId; }
/// <summary>
/// 联盟分享矿信息消息ID
/// </summary>
public void setGuildShareMineMsgId(long _guildShareMineMsgId) { guildShareMineMsgId = _guildShareMineMsgId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildShareMineMsgId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guildShareMineMsgId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)19);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

