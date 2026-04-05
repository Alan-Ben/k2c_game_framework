package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
public class GS2GC_042_004_RetMarsHelpDealedList implements ALBasicProtocolPack._IALProtocolStructure {
/** 已互助的数量 */
private int dealedCount;
/** 允许互助的上限 */
private int dealLimit;


public GS2GC_042_004_RetMarsHelpDealedList() {
	dealedCount = 0;
	dealLimit = 0;
}

public GS2GC_042_004_RetMarsHelpDealedList(
	 int _dealedCount
	, int _dealLimit
) {	dealedCount = _dealedCount;
	dealLimit = _dealLimit;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)4; }

/** 已互助的数量 */
public int getDealedCount() { return dealedCount; }
/** 已互助的数量 */
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/** 允许互助的上限 */
public int getDealLimit() { return dealLimit; }
/** 允许互助的上限 */
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealLimit = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dealedCount);
	_buf.putInt(dealLimit);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)4);
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

