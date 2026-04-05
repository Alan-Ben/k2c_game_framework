using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p042_GuildRelatedOp
{

/// <summary>
/// 请求查询单个集结信息
/// </summary>
public class GC2GS_042_062_ReqQueryRallyInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集结ID
/// </summary>
private long rallyId;


public GC2GS_042_062_ReqQueryRallyInfo() {
	rallyId = (long)0;
}

public GC2GS_042_062_ReqQueryRallyInfo(
	long _rallyId
) {	rallyId = _rallyId;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 集结ID
/// </summary>
public long getRallyId() { return rallyId; }
/// <summary>
/// 集结ID
/// </summary>
public void setRallyId(long _rallyId) { rallyId = _rallyId; }


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
	rallyId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rallyId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)62);
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
	builder.Append("rallyId").Append(":").Append(rallyId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

