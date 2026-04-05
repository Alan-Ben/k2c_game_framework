package Common;

import java.nio.ByteBuffer;
/*********
 * 通用上下文
 **/
public class Common_Context implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件id */
private int eventId;
/** 上下文唯一id */
private long guid;


public Common_Context() {
	eventId = 0;
	guid = (long)0;
}

public Common_Context(
	 int _eventId
	, long _guid
) {	eventId = _eventId;
	guid = _guid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 事件id */
public int getEventId() { return eventId; }
/** 事件id */
public void setEventId(int _eventId) { eventId = _eventId; }
/** 上下文唯一id */
public long getGuid() { return guid; }
/** 上下文唯一id */
public void setGuid(long _guid) { guid = _guid; }


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
	if(_buf.remaining() > 0) eventId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(eventId);
	_buf.putLong(guid);
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

