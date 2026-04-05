package Common.ChapterObj;

import java.nio.ByteBuffer;
/*********
 * 关卡事件信息
 **/
public class Chapter_EventInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long eventId;
private byte[] extraData;


public Chapter_EventInfo() {
	eventId = (long)0;
	extraData = null;
}

public Chapter_EventInfo(
	 long _eventId
	, byte[] _extraData
) {	eventId = _eventId;
	extraData = _extraData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getEventId() { return eventId; }
public void setEventId(long _eventId) { eventId = _eventId; }
public byte[] getExtraData() { return extraData; }
public java.nio.ByteBuffer get_buffer_ExtraData() { if(null == extraData)return null; else return ByteBuffer.wrap(extraData); }

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
	int _size = 8;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
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

