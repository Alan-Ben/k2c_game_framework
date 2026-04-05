using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 设置拒绝联姻
/// </summary>
public class GC2GS_014_020_ReqSetRefuseMarry : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否拒绝联姻
/// </summary>
private bool isRefuse;


public GC2GS_014_020_ReqSetRefuseMarry() {
	isRefuse = false;
}

public GC2GS_014_020_ReqSetRefuseMarry(
	bool _isRefuse
) {	isRefuse = _isRefuse;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 是否拒绝联姻
/// </summary>
public bool getIsRefuse() { return isRefuse; }
/// <summary>
/// 是否拒绝联姻
/// </summary>
public void setIsRefuse(bool _isRefuse) { isRefuse = _isRefuse; }


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
	isRefuse = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isRefuse?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)20);
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
	builder.Append("isRefuse").Append(":").Append(isRefuse.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

