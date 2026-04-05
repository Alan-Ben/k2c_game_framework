using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p018_PlayerSkinOp
{

/// <summary>
/// 设置称号是否可展示
/// </summary>
public class GC2GS_018_003_ReqSetTitleShow : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private bool isShow;


public GC2GS_018_003_ReqSetTitleShow() {
	isShow = false;
}

public GC2GS_018_003_ReqSetTitleShow(
	bool _isShow
) {	isShow = _isShow;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 空
/// </summary>
public bool getIsShow() { return isShow; }
/// <summary>
/// 空
/// </summary>
public void setIsShow(bool _isShow) { isShow = _isShow; }


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
	isShow = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isShow?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)3);
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
	builder.Append("isShow").Append(":").Append(isShow.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

