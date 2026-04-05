package Common.AnecdoteObj;

import java.nio.ByteBuffer;
/*********
 * 政务事件数据
 **/
public class Anecdote_EventInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long instanceId;
/** 位置id */
private long posId;
/** 事件id */
private long eventId;
/** 事件额外信息 */
private byte[] extraData;


public Anecdote_EventInfo() {
	instanceId = (long)0;
	posId = (long)0;
	eventId = (long)0;
	extraData = null;
}

public Anecdote_EventInfo(
	 long _instanceId
	, long _posId
	, long _eventId
	, byte[] _extraData
) {	instanceId = _instanceId;
	posId = _posId;
	eventId = _eventId;
	extraData = _extraData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getInstanceId() { return instanceId; }
/** 实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 位置id */
public long getPosId() { return posId; }
/** 位置id */
public void setPosId(long _posId) { posId = _posId; }
/** 事件id */
public long getEventId() { return eventId; }
/** 事件id */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 事件额外信息 */
public byte[] getExtraData() { return extraData; }
public java.nio.ByteBuffer get_buffer_ExtraData() { if(null == extraData)return null; else return ByteBuffer.wrap(extraData); }

/** 事件额外信息 */
public void setExtraData(byte[] _extraData) { extraData = _extraData; }
public void setExtraData(java.nio.ByteBuffer _extraData) 
{
	if(null == _extraData){return;}
	int _oldPos = _extraData.position();
	int _bufLength = _extraData.remaining();
	extraData = new byte[_bufLength];
	_extraData.get(extraData);
	_extraData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 24;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extraDataCount = _buf.getInt();
	if(0 < _extraDataCount){
		extraData = new byte[_extraDataCount];
		_buf.get(extraData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(posId);
	_buf.putLong(eventId);
	_buf.putInt((extraData == null ? 0 : extraData.length));
	if(null != extraData){_buf.put(extraData);}

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

