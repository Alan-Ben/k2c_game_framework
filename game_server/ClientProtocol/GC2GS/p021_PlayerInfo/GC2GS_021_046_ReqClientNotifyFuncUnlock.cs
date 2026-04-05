using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

public class GC2GS_021_046_ReqClientNotifyFuncUnlock : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 功能类型
/// </summary>
private NPEnum.ENPFunctionType funcType;


public GC2GS_021_046_ReqClientNotifyFuncUnlock() {
	funcType = 0;
}

public GC2GS_021_046_ReqClientNotifyFuncUnlock(
	NPEnum.ENPFunctionType _funcType
) {	funcType = _funcType;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)46; }

/// <summary>
/// 功能类型
/// </summary>
public NPEnum.ENPFunctionType getFuncType() { return funcType; }
/// <summary>
/// 功能类型
/// </summary>
public void setFuncType(NPEnum.ENPFunctionType _funcType) { funcType = _funcType; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	funcType = (NPEnum.ENPFunctionType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)funcType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)46);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)46);
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
	builder.Append("funcType").Append(":").Append(funcType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

