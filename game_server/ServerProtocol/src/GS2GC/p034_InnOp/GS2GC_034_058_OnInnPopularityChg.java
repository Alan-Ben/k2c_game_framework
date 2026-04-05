package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店人气值变更
 **/
public class GS2GC_034_058_OnInnPopularityChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 新人气值 */
private long newPopularity;


public GS2GC_034_058_OnInnPopularityChg() {
	newPopularity = (long)0;
}

public GS2GC_034_058_OnInnPopularityChg(
	 long _newPopularity
) {	newPopularity = _newPopularity;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)58; }

/** 新人气值 */
public long getNewPopularity() { return newPopularity; }
/** 新人气值 */
public void setNewPopularity(long _newPopularity) { newPopularity = _newPopularity; }


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
	if(_buf.remaining() > 0) newPopularity = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(newPopularity);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)58);
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

