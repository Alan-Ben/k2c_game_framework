using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

public class GC2GS_021_041_ReqDealCommonRefresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 刷新id
/// </summary>
private long refId;


public GC2GS_021_041_ReqDealCommonRefresh() {
	refId = (long)0;
}

public GC2GS_021_041_ReqDealCommonRefresh(
	long _refId
) {	refId = _refId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)41; }

/// <summary>
/// 刷新id
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 刷新id
/// </summary>
public void setRefId(long _refId) { refId = _refId; }


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
	refId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)41);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

