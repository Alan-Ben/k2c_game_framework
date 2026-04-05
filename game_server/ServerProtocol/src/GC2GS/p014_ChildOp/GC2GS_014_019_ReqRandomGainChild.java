package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 随机获得子嗣
 **/
public class GC2GS_014_019_ReqRandomGainChild implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置ID */
private int seatId;


public GC2GS_014_019_ReqRandomGainChild() {
	seatId = 0;
}

public GC2GS_014_019_ReqRandomGainChild(
	 int _seatId
) {	seatId = _seatId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)19; }

/** 位置ID */
public int getSeatId() { return seatId; }
/** 位置ID */
public void setSeatId(int _seatId) { seatId = _seatId; }


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
	if(_buf.remaining() > 0) seatId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(seatId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)19);
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

