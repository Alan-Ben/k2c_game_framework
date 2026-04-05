package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人加护点推送
 **/
public class GS2GC_015_053_OnConsortCharmPointChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 空 */
private long charmPoint;


public GS2GC_015_053_OnConsortCharmPointChg() {
	consortId = (long)0;
	charmPoint = (long)0;
}

public GS2GC_015_053_OnConsortCharmPointChg(
	 long _consortId
	, long _charmPoint
) {	consortId = _consortId;
	charmPoint = _charmPoint;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)53; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 空 */
public long getCharmPoint() { return charmPoint; }
/** 空 */
public void setCharmPoint(long _charmPoint) { charmPoint = _charmPoint; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) charmPoint = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(charmPoint);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
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

