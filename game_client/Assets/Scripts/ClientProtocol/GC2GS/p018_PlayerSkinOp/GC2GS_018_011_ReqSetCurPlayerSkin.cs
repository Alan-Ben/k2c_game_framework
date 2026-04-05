using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p018_PlayerSkinOp
{

/// <summary>
/// 穿戴皮肤
/// </summary>
public class GC2GS_018_011_ReqSetCurPlayerSkin : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long skinId;


public GC2GS_018_011_ReqSetCurPlayerSkin() {
	skinId = (long)0;
}

public GC2GS_018_011_ReqSetCurPlayerSkin(
	long _skinId
) {	skinId = _skinId;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 空
/// </summary>
public long getSkinId() { return skinId; }
/// <summary>
/// 空
/// </summary>
public void setSkinId(long _skinId) { skinId = _skinId; }


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
	skinId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(skinId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)11);
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
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

