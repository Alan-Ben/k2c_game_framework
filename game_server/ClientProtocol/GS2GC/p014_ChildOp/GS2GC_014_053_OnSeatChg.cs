using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 训练位变动推送
/// </summary>
public class GS2GC_014_053_OnSeatChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 训练位数据
/// </summary>
private Common.ChildObj.Child_SeatInfo seat;


public GS2GC_014_053_OnSeatChg() {
	seat = new Common.ChildObj.Child_SeatInfo();
}

public GS2GC_014_053_OnSeatChg(
	Common.ChildObj.Child_SeatInfo _seat
) {	seat = _seat;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 训练位数据
/// </summary>
public Common.ChildObj.Child_SeatInfo getSeat() { return seat; }
/// <summary>
/// 训练位数据
/// </summary>
public void setSeat(Common.ChildObj.Child_SeatInfo _seat) { seat = _seat; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _seatCustLen = _buf.getInt();
	int _seatCurPos = _buf.getCurPos();
	seat.ReadUnzipBuf(_buf, _seatCurPos + _seatCustLen);
	_buf.setPosition(_seatCurPos + _seatCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(seat.GetBufSize());
	seat.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)53);
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
	builder.Append("seat").Append(":").Append(seat == null ? "null" : seat.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

