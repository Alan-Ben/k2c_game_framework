using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPLS2GC.p001_BasicOp
{

public class NPLS2GC_001_002_EnterGameRes : ALBasicProtocolPack._IALProtocolStructure {
private bool res;
private string gateServerIp;
private int gateServerPort;
private string uid;
private string checkCode;
/// <summary>
/// 是否白名单
/// </summary>
private bool isWhite;


public NPLS2GC_001_002_EnterGameRes() {
	res = false;
	gateServerIp = "";
	gateServerPort = 0;
	uid = "";
	checkCode = "";
	isWhite = false;
}

public NPLS2GC_001_002_EnterGameRes(
	bool _res
	, string _gateServerIp
	, int _gateServerPort
	, string _uid
	, string _checkCode
	, bool _isWhite
) {	res = _res;
	gateServerIp = _gateServerIp;
	gateServerPort = _gateServerPort;
	uid = _uid;
	checkCode = _checkCode;
	isWhite = _isWhite;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)2; }

public bool getRes() { return res; }
public void setRes(bool _res) { res = _res; }
public string getGateServerIp() { return gateServerIp; }
public void setGateServerIp(string _gateServerIp) { gateServerIp = _gateServerIp; }
public int getGateServerPort() { return gateServerPort; }
public void setGateServerPort(int _gateServerPort) { gateServerPort = _gateServerPort; }
public string getUid() { return uid; }
public void setUid(string _uid) { uid = _uid; }
public string getCheckCode() { return checkCode; }
public void setCheckCode(string _checkCode) { checkCode = _checkCode; }
/// <summary>
/// 是否白名单
/// </summary>
public bool getIsWhite() { return isWhite; }
/// <summary>
/// 是否白名单
/// </summary>
public void setIsWhite(bool _isWhite) { isWhite = _isWhite; }


public int GetBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gateServerIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gateServerIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gateServerIp = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gateServerPort = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	checkCode = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isWhite = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(res?(byte)1:(byte)0);
	_buf.putString(gateServerIp);
	_buf.putInt(gateServerPort);
	_buf.putString(uid);
	_buf.putString(checkCode);
	_buf.put(isWhite?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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
	builder.Append("res").Append(":").Append(res.ToString()).Append(", ");
	builder.Append("gateServerIp").Append(":").Append(gateServerIp.ToString()).Append(", ");
	builder.Append("gateServerPort").Append(":").Append(gateServerPort.ToString()).Append(", ");
	builder.Append("uid").Append(":").Append(uid.ToString()).Append(", ");
	builder.Append("checkCode").Append(":").Append(checkCode.ToString()).Append(", ");
	builder.Append("isWhite").Append(":").Append(isWhite.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

