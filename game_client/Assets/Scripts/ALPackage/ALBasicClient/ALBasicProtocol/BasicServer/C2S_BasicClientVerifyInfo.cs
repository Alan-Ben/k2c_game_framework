using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace BasicServer
{

public class C2S_BasicClientVerifyInfo : ALBasicProtocolPack._IALProtocolStructure {
private int clientType;
private string userName;
private string userPassword;
private string customMsg;


public C2S_BasicClientVerifyInfo() {
	clientType = 0;
	userName = "";
	userPassword = "";
	customMsg = "";
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getClientType() { return clientType; }
public void setClientType(int _clientType) { clientType = _clientType; }
public string getUserName() { return userName; }
public void setUserName(string _userName) { userName = _userName; }
public string getUserPassword() { return userPassword; }
public void setUserPassword(string _userPassword) { userPassword = _userPassword; }
public string getCustomMsg() { return customMsg; }
public void setCustomMsg(string _customMsg) { customMsg = _customMsg; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userPassword);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customMsg);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userPassword);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customMsg);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf) {
	clientType = _buf.getInt();
	userName = _buf.getString();
	userPassword = _buf.getString();
	customMsg = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(clientType);
	_buf.putString(userName);
	_buf.putString(userPassword);
	_buf.putString(customMsg);
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

