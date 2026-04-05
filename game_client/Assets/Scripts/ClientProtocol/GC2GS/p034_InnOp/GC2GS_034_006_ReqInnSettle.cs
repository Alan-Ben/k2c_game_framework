using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p034_InnOp
{

/// <summary>
/// 旅店结算
/// </summary>
public class GC2GS_034_006_ReqInnSettle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 结算客人数量
/// </summary>
private int guestNum;


public GC2GS_034_006_ReqInnSettle() {
	guestNum = 0;
}

public GC2GS_034_006_ReqInnSettle(
	int _guestNum
) {	guestNum = _guestNum;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 结算客人数量
/// </summary>
public int getGuestNum() { return guestNum; }
/// <summary>
/// 结算客人数量
/// </summary>
public void setGuestNum(int _guestNum) { guestNum = _guestNum; }


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
	guestNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(guestNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)6);
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
	builder.Append("guestNum").Append(":").Append(guestNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

