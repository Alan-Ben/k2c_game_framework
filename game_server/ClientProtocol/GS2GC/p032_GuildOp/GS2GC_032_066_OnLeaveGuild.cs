using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_066_OnLeaveGuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否被踢出
/// </summary>
private bool isKick;


public GS2GC_032_066_OnLeaveGuild() {
	isKick = false;
}

public GS2GC_032_066_OnLeaveGuild(
	bool _isKick
) {	isKick = _isKick;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)66; }

/// <summary>
/// 是否被踢出
/// </summary>
public bool getIsKick() { return isKick; }
/// <summary>
/// 是否被踢出
/// </summary>
public void setIsKick(bool _isKick) { isKick = _isKick; }


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
	isKick = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isKick?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)66);
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
	builder.Append("isKick").Append(":").Append(isKick.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

