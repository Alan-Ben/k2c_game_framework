using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_USInfo : ALBasicProtocolPack._IALProtocolStructure {
private int serverTypeId;
private long usServerId;


public Common_USInfo() {
	serverTypeId = 0;
	usServerId = (long)0;
}

public Common_USInfo(
	int _serverTypeId
	, long _usServerId
) {	serverTypeId = _serverTypeId;
	usServerId = _usServerId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getServerTypeId() { return serverTypeId; }
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
public long getUsServerId() { return usServerId; }
public void setUsServerId(long _usServerId) { usServerId = _usServerId; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	usServerId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serverTypeId);
	_buf.putLong(usServerId);
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
	builder.Append("serverTypeId").Append(":").Append(serverTypeId.ToString()).Append(", ");
	builder.Append("usServerId").Append(":").Append(usServerId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

