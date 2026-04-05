package Common.TravelObj;

import java.nio.ByteBuffer;
/*********
 * 游历事件
 **/
public class Travel_Event implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long instanceId;
/** 事件ID */
private long eventId;
/** 位置 */
private long pos;


public Travel_Event() {
	instanceId = (long)0;
	eventId = (long)0;
	pos = (long)0;
}

public Travel_Event(
	 long _instanceId
	, long _eventId
	, long _pos
) {	instanceId = _instanceId;
	eventId = _eventId;
	pos = _pos;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 事件ID */
public long getEventId() { return eventId; }
/** 事件ID */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 位置 */
public long getPos() { return pos; }
/** 位置 */
public void setPos(long _pos) { pos = _pos; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(eventId);
	_buf.putLong(pos);
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

