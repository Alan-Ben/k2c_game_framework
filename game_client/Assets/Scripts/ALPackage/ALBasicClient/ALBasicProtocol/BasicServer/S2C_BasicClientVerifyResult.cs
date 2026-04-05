using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace BasicServer
{

public class S2C_BasicClientVerifyResult : ALBasicProtocolPack._IALProtocolStructure {
private long socketID;
private string customRetMsg;


public S2C_BasicClientVerifyResult() {
	socketID = (long)0;
	customRetMsg = "";
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSocketID() { return socketID; }
public void setSocketID(long _socketID) { socketID = _socketID; }
public string getCustomRetMsg() { return customRetMsg; }
public void setCustomRetMsg(string _customRetMsg) { customRetMsg = _customRetMsg; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customRetMsg);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customRetMsg);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf) {
	socketID = _buf.getLong();
	customRetMsg = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(socketID);
	_buf.putString(customRetMsg);
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
	ReadUnzipBuf(_bufObj);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf);
}
}

}

