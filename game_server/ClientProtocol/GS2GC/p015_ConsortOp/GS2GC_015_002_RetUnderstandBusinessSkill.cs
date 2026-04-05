using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-领悟经营技能等级
/// </summary>
public class GS2GC_015_002_RetUnderstandBusinessSkill : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 操作是否成功
/// </summary>
private bool isProUp;


public GS2GC_015_002_RetUnderstandBusinessSkill() {
	isProUp = false;
}

public GS2GC_015_002_RetUnderstandBusinessSkill(
	bool _isProUp
) {	isProUp = _isProUp;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 操作是否成功
/// </summary>
public bool getIsProUp() { return isProUp; }
/// <summary>
/// 操作是否成功
/// </summary>
public void setIsProUp(bool _isProUp) { isProUp = _isProUp; }


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
	isProUp = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isProUp?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
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
	builder.Append("isProUp").Append(":").Append(isProUp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

