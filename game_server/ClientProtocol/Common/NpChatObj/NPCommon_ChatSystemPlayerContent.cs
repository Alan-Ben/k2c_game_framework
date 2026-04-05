using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

public class NPCommon_ChatSystemPlayerContent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 系统用户展示信息id
/// </summary>
private long systemPlayerId;


public NPCommon_ChatSystemPlayerContent() {
	systemPlayerId = (long)0;
}

public NPCommon_ChatSystemPlayerContent(
	long _systemPlayerId
) {	systemPlayerId = _systemPlayerId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 系统用户展示信息id
/// </summary>
public long getSystemPlayerId() { return systemPlayerId; }
/// <summary>
/// 系统用户展示信息id
/// </summary>
public void setSystemPlayerId(long _systemPlayerId) { systemPlayerId = _systemPlayerId; }


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
	systemPlayerId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(systemPlayerId);
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
	builder.Append("systemPlayerId").Append(":").Append(systemPlayerId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

