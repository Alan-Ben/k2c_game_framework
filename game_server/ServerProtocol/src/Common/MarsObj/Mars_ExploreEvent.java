package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-事件数据
 **/
public class Mars_ExploreEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 事件状态 */
private Common.MarsEnum.EMarsExploreEventType eventType;
/** 事件ID */
private long eventId;
/** 事件位置 */
private long pos;
/** 是否完成 */
private boolean isDone;
/** 探索等级 */
private int exploreLvl;
/** 创建时间（毫秒） */
private long createdMs;


public Mars_ExploreEvent() {
	id = (long)0;
	eventType = Common.MarsEnum.EMarsExploreEventType.values()[0];
	eventId = (long)0;
	pos = (long)0;
	isDone = false;
	exploreLvl = 0;
	createdMs = (long)0;
}

public Mars_ExploreEvent(
	 long _id
	, Common.MarsEnum.EMarsExploreEventType _eventType
	, long _eventId
	, long _pos
	, boolean _isDone
	, int _exploreLvl
	, long _createdMs
) {	id = _id;
	eventType = _eventType;
	eventId = _eventId;
	pos = _pos;
	isDone = _isDone;
	exploreLvl = _exploreLvl;
	createdMs = _createdMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 事件状态 */
public Common.MarsEnum.EMarsExploreEventType getEventType() { return eventType; }
/** 事件状态 */
public void setEventType(Common.MarsEnum.EMarsExploreEventType _eventType) { eventType = _eventType; }
/** 事件ID */
public long getEventId() { return eventId; }
/** 事件ID */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 事件位置 */
public long getPos() { return pos; }
/** 事件位置 */
public void setPos(long _pos) { pos = _pos; }
/** 是否完成 */
public boolean getIsDone() { return isDone; }
/** 是否完成 */
public void setIsDone(boolean _isDone) { isDone = _isDone; }
/** 探索等级 */
public int getExploreLvl() { return exploreLvl; }
/** 探索等级 */
public void setExploreLvl(int _exploreLvl) { exploreLvl = _exploreLvl; }
/** 创建时间（毫秒） */
public long getCreatedMs() { return createdMs; }
/** 创建时间（毫秒） */
public void setCreatedMs(long _createdMs) { createdMs = _createdMs; }


public final int GetBufSize() {
	int _size = 41;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 43;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventType = Common.MarsEnum.EMarsExploreEventType.EMarsExploreEventType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDone = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exploreLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createdMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(eventType.ordinal());

	_buf.putLong(eventId);
	_buf.putLong(pos);
	_buf.put(isDone?(byte)1:(byte)0);
	_buf.putInt(exploreLvl);
	_buf.putLong(createdMs);
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

