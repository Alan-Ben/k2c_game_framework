package Common.TravelObj;

import java.nio.ByteBuffer;
/*********
 * 游历妃子数据
 **/
public class Travel_Consort implements ALBasicProtocolPack._IALProtocolStructure {
/** 妃子ID */
private long consortId;
/** 好感度 */
private int like;


public Travel_Consort() {
	consortId = (long)0;
	like = 0;
}

public Travel_Consort(
	 long _consortId
	, int _like
) {	consortId = _consortId;
	like = _like;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 妃子ID */
public long getConsortId() { return consortId; }
/** 妃子ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 好感度 */
public int getLike() { return like; }
/** 好感度 */
public void setLike(int _like) { like = _like; }


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
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) like = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putInt(like);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

