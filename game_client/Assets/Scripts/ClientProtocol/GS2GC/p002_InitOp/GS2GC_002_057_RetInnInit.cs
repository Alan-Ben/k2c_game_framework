using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_057_RetInnInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 旅店信息
/// </summary>
private Common.InnObj.Inn_Info innInfo;


public GS2GC_002_057_RetInnInit() {
	innInfo = new Common.InnObj.Inn_Info();
}

public GS2GC_002_057_RetInnInit(
	Common.InnObj.Inn_Info _innInfo
) {	innInfo = _innInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 旅店信息
/// </summary>
public Common.InnObj.Inn_Info getInnInfo() { return innInfo; }
/// <summary>
/// 旅店信息
/// </summary>
public void setInnInfo(Common.InnObj.Inn_Info _innInfo) { innInfo = _innInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + innInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + innInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _innInfoCustLen = _buf.getInt();
	int _innInfoCurPos = _buf.getCurPos();
	innInfo.ReadUnzipBuf(_buf, _innInfoCurPos + _innInfoCustLen);
	_buf.setPosition(_innInfoCurPos + _innInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(innInfo.GetBufSize());
	innInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)57);
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
	builder.Append("innInfo").Append(":").Append(innInfo == null ? "null" : innInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

