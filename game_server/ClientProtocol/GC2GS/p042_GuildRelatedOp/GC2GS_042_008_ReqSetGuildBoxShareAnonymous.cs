using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p042_GuildRelatedOp
{

/// <summary>
/// 设置联盟宝箱匿名分享
/// </summary>
public class GC2GS_042_008_ReqSetGuildBoxShareAnonymous : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否匿名 true-匿名
/// </summary>
private bool isAnonymous;


public GC2GS_042_008_ReqSetGuildBoxShareAnonymous() {
	isAnonymous = false;
}

public GC2GS_042_008_ReqSetGuildBoxShareAnonymous(
	bool _isAnonymous
) {	isAnonymous = _isAnonymous;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 是否匿名 true-匿名
/// </summary>
public bool getIsAnonymous() { return isAnonymous; }
/// <summary>
/// 是否匿名 true-匿名
/// </summary>
public void setIsAnonymous(bool _isAnonymous) { isAnonymous = _isAnonymous; }


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
	isAnonymous = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isAnonymous?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)8);
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
	builder.Append("isAnonymous").Append(":").Append(isAnonymous.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

