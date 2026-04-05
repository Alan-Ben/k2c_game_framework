using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 联姻请求-对指定玩家发起请求
/// </summary>
public class GS2GC_014_013_RetApplyToPlayer : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否拒绝所有联姻请求
/// </summary>
private bool isPlayerRefuseAllRequest;


public GS2GC_014_013_RetApplyToPlayer() {
	isPlayerRefuseAllRequest = false;
}

public GS2GC_014_013_RetApplyToPlayer(
	bool _isPlayerRefuseAllRequest
) {	isPlayerRefuseAllRequest = _isPlayerRefuseAllRequest;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 是否拒绝所有联姻请求
/// </summary>
public bool getIsPlayerRefuseAllRequest() { return isPlayerRefuseAllRequest; }
/// <summary>
/// 是否拒绝所有联姻请求
/// </summary>
public void setIsPlayerRefuseAllRequest(bool _isPlayerRefuseAllRequest) { isPlayerRefuseAllRequest = _isPlayerRefuseAllRequest; }


public int GetBufSize() {
	int _size = 1;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isPlayerRefuseAllRequest = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isPlayerRefuseAllRequest?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)13);
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
	builder.Append("isPlayerRefuseAllRequest").Append(":").Append(isPlayerRefuseAllRequest.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

