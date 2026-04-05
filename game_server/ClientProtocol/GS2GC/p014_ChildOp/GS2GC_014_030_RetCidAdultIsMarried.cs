using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 返回其他玩家子嗣是否结婚的信息
/// </summary>
public class GS2GC_014_030_RetCidAdultIsMarried : ALBasicProtocolPack._IALProtocolStructure {
private bool isGiftde;
private bool isMarried;


public GS2GC_014_030_RetCidAdultIsMarried() {
	isGiftde = false;
	isMarried = false;
}

public GS2GC_014_030_RetCidAdultIsMarried(
	bool _isGiftde
	, bool _isMarried
) {	isGiftde = _isGiftde;
	isMarried = _isMarried;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)30; }

public bool getIsGiftde() { return isGiftde; }
public void setIsGiftde(bool _isGiftde) { isGiftde = _isGiftde; }
public bool getIsMarried() { return isMarried; }
public void setIsMarried(bool _isMarried) { isMarried = _isMarried; }


public int GetBufSize() {
	int _size = 2;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isGiftde = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMarried = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isGiftde?(byte)1:(byte)0);
	_buf.put(isMarried?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)30);
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
	builder.Append("isGiftde").Append(":").Append(isGiftde.ToString()).Append(", ");
	builder.Append("isMarried").Append(":").Append(isMarried.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

