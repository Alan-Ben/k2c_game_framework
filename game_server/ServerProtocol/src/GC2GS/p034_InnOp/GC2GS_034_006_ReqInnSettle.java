package GC2GS.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店结算
 **/
public class GC2GS_034_006_ReqInnSettle implements ALBasicProtocolPack._IALProtocolStructure {
/** 结算客人数量 */
private int guestNum;


public GC2GS_034_006_ReqInnSettle() {
	guestNum = 0;
}

public GC2GS_034_006_ReqInnSettle(
	 int _guestNum
) {	guestNum = _guestNum;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)6; }

/** 结算客人数量 */
public int getGuestNum() { return guestNum; }
/** 结算客人数量 */
public void setGuestNum(int _guestNum) { guestNum = _guestNum; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guestNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(guestNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)6);
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

