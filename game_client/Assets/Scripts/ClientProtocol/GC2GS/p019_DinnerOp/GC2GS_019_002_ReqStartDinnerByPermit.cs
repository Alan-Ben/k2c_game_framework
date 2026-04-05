using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p019_DinnerOp
{

/// <summary>
/// 开启宴会（许可证模式）
/// </summary>
public class GC2GS_019_002_ReqStartDinnerByPermit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 许可证实例ID
/// </summary>
private long permitInstanceId;


public GC2GS_019_002_ReqStartDinnerByPermit() {
	permitInstanceId = (long)0;
}

public GC2GS_019_002_ReqStartDinnerByPermit(
	long _permitInstanceId
) {	permitInstanceId = _permitInstanceId;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 许可证实例ID
/// </summary>
public long getPermitInstanceId() { return permitInstanceId; }
/// <summary>
/// 许可证实例ID
/// </summary>
public void setPermitInstanceId(long _permitInstanceId) { permitInstanceId = _permitInstanceId; }


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
	permitInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(permitInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
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
	builder.Append("permitInstanceId").Append(":").Append(permitInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

