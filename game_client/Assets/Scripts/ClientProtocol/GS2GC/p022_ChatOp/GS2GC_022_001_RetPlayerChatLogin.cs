using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p022_ChatOp
{

public class GS2GC_022_001_RetPlayerChatLogin : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天系统ID
/// </summary>
private long systemId;
/// <summary>
/// 聊天系统标识
/// </summary>
private string systemTag;
/// <summary>
/// 聊天服ip
/// </summary>
private string ip;
/// <summary>
/// 聊天服端口
/// </summary>
private int port;
/// <summary>
/// 登录密码
/// </summary>
private string checkCode;


public GS2GC_022_001_RetPlayerChatLogin() {
	systemId = (long)0;
	systemTag = "";
	ip = "";
	port = 0;
	checkCode = "";
}

public GS2GC_022_001_RetPlayerChatLogin(
	long _systemId
	, string _systemTag
	, string _ip
	, int _port
	, string _checkCode
) {	systemId = _systemId;
	systemTag = _systemTag;
	ip = _ip;
	port = _port;
	checkCode = _checkCode;
}

public byte getMainOrder() { return (byte)22; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 聊天系统ID
/// </summary>
public long getSystemId() { return systemId; }
/// <summary>
/// 聊天系统ID
/// </summary>
public void setSystemId(long _systemId) { systemId = _systemId; }
/// <summary>
/// 聊天系统标识
/// </summary>
public string getSystemTag() { return systemTag; }
/// <summary>
/// 聊天系统标识
/// </summary>
public void setSystemTag(string _systemTag) { systemTag = _systemTag; }
/// <summary>
/// 聊天服ip
/// </summary>
public string getIp() { return ip; }
/// <summary>
/// 聊天服ip
/// </summary>
public void setIp(string _ip) { ip = _ip; }
/// <summary>
/// 聊天服端口
/// </summary>
public int getPort() { return port; }
/// <summary>
/// 聊天服端口
/// </summary>
public void setPort(int _port) { port = _port; }
/// <summary>
/// 登录密码
/// </summary>
public string getCheckCode() { return checkCode; }
/// <summary>
/// 登录密码
/// </summary>
public void setCheckCode(string _checkCode) { checkCode = _checkCode; }


public int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	systemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	systemTag = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ip = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	port = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	checkCode = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(systemId);
	_buf.putString(systemTag);
	_buf.putString(ip);
	_buf.putInt(port);
	_buf.putString(checkCode);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)1);
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
	builder.Append("systemId").Append(":").Append(systemId.ToString()).Append(", ");
	builder.Append("systemTag").Append(":").Append(systemTag.ToString()).Append(", ");
	builder.Append("ip").Append(":").Append(ip.ToString()).Append(", ");
	builder.Append("port").Append(":").Append(port.ToString()).Append(", ");
	builder.Append("checkCode").Append(":").Append(checkCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

