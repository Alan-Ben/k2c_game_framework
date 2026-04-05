package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 增加训练位脑力值
 **/
public class GC2GS_014_003_ReqAddSeatEnergy implements ALBasicProtocolPack._IALProtocolStructure {
/** 训练位ID */
private long seatId;
/** 增加数量 */
private int addCount;


public GC2GS_014_003_ReqAddSeatEnergy() {
	seatId = (long)0;
	addCount = 0;
}

public GC2GS_014_003_ReqAddSeatEnergy(
	 long _seatId
	, int _addCount
) {	seatId = _seatId;
	addCount = _addCount;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)3; }

/** 训练位ID */
public long getSeatId() { return seatId; }
/** 训练位ID */
public void setSeatId(long _seatId) { seatId = _seatId; }
/** 增加数量 */
public int getAddCount() { return addCount; }
/** 增加数量 */
public void setAddCount(int _addCount) { addCount = _addCount; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) seatId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(seatId);
	_buf.putInt(addCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)3);
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

