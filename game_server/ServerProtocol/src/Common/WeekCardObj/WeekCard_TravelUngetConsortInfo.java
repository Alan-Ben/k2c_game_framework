package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-游历未获得妃子结算信息
 **/
public class WeekCard_TravelUngetConsortInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 妃子id */
private long consortId;
/** 获得好感值 */
private long like;


public WeekCard_TravelUngetConsortInfo() {
	consortId = (long)0;
	like = (long)0;
}

public WeekCard_TravelUngetConsortInfo(
	 long _consortId
	, long _like
) {	consortId = _consortId;
	like = _like;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 妃子id */
public long getConsortId() { return consortId; }
/** 妃子id */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 获得好感值 */
public long getLike() { return like; }
/** 获得好感值 */
public void setLike(long _like) { like = _like; }


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
	if(_buf.remaining() > 0) like = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(like);
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

