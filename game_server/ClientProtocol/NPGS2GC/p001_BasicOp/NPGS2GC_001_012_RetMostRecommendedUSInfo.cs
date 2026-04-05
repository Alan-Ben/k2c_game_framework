using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_012_RetMostRecommendedUSInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最推荐服务器信息
/// </summary>
private NPCommon.NP_SYS_ServerItem serverItem;
/// <summary>
/// 错误码
/// </summary>
private int errCode;


public NPGS2GC_001_012_RetMostRecommendedUSInfo() {
	serverItem = new NPCommon.NP_SYS_ServerItem();
	errCode = 0;
}

public NPGS2GC_001_012_RetMostRecommendedUSInfo(
	NPCommon.NP_SYS_ServerItem _serverItem
	, int _errCode
) {	serverItem = _serverItem;
	errCode = _errCode;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 最推荐服务器信息
/// </summary>
public NPCommon.NP_SYS_ServerItem getServerItem() { return serverItem; }
/// <summary>
/// 最推荐服务器信息
/// </summary>
public void setServerItem(NPCommon.NP_SYS_ServerItem _serverItem) { serverItem = _serverItem; }
/// <summary>
/// 错误码
/// </summary>
public int getErrCode() { return errCode; }
/// <summary>
/// 错误码
/// </summary>
public void setErrCode(int _errCode) { errCode = _errCode; }


public int GetBufSize() {
	int _size = 4;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _serverItemCustLen = _buf.getInt();
	int _serverItemCurPos = _buf.getCurPos();
	serverItem.ReadUnzipBuf(_buf, _serverItemCurPos + _serverItemCustLen);
	_buf.setPosition(_serverItemCurPos + _serverItemCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serverItem.GetBufSize());
	serverItem.PutUnzipBuf(_buf);
	_buf.putInt(errCode);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)12);
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
	builder.Append("serverItem").Append(":").Append(serverItem == null ? "null" : serverItem.ToString()).Append(", ");
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

