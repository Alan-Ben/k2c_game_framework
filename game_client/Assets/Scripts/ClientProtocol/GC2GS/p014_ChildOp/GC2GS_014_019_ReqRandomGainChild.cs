using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p014_ChildOp
{

/// <summary>
/// 随机获得子嗣
/// </summary>
public class GC2GS_014_019_ReqRandomGainChild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置ID
/// </summary>
private int seatId;


public GC2GS_014_019_ReqRandomGainChild() {
	seatId = 0;
}

public GC2GS_014_019_ReqRandomGainChild(
	int _seatId
) {	seatId = _seatId;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)19; }

/// <summary>
/// 位置ID
/// </summary>
public int getSeatId() { return seatId; }
/// <summary>
/// 位置ID
/// </summary>
public void setSeatId(int _seatId) { seatId = _seatId; }


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
	seatId = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(seatId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)19);
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
	builder.Append("seatId").Append(":").Append(seatId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

