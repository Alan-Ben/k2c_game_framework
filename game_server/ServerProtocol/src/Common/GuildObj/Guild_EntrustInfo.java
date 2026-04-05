package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟杂物委托信息
 **/
public class Guild_EntrustInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 序列号 */
private long serial;
/** 配表id */
private long refId;
/** 事件id */
private long eventId;
/** 当前进度 */
private int point;


public Guild_EntrustInfo() {
	serial = (long)0;
	refId = (long)0;
	eventId = (long)0;
	point = 0;
}

public Guild_EntrustInfo(
	 long _serial
	, long _refId
	, long _eventId
	, int _point
) {	serial = _serial;
	refId = _refId;
	eventId = _eventId;
	point = _point;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 序列号 */
public long getSerial() { return serial; }
/** 序列号 */
public void setSerial(long _serial) { serial = _serial; }
/** 配表id */
public long getRefId() { return refId; }
/** 配表id */
public void setRefId(long _refId) { refId = _refId; }
/** 事件id */
public long getEventId() { return eventId; }
/** 事件id */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 当前进度 */
public int getPoint() { return point; }
/** 当前进度 */
public void setPoint(int _point) { point = _point; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) point = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(serial);
	_buf.putLong(refId);
	_buf.putLong(eventId);
	_buf.putInt(point);
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

