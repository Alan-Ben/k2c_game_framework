package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 训练位变动推送
 **/
public class GS2GC_014_053_OnSeatChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 训练位数据 */
private Common.ChildObj.Child_SeatInfo seat;


public GS2GC_014_053_OnSeatChg() {
	seat = new Common.ChildObj.Child_SeatInfo();
}

public GS2GC_014_053_OnSeatChg(
	 Common.ChildObj.Child_SeatInfo _seat
) {	seat = _seat;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)53; }

/** 训练位数据 */
public Common.ChildObj.Child_SeatInfo getSeat() { return seat; }
/** 训练位数据 */
public void setSeat(Common.ChildObj.Child_SeatInfo _seat) { seat = _seat; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _seatCustLen = _buf.getInt();
	int _seatCurPos = _buf.position();
	seat.ReadUnzipBuf(_buf, _seatCurPos + _seatCustLen);
	_buf.position(_seatCurPos + _seatCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(seat.GetBufSize());
	seat.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)53);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

