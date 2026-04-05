package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 取消弹劾事件
 **/
public class GC2GS_032_025_ReqCancelLeaderImpeachEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件数据ID */
private long eventDbId;


public GC2GS_032_025_ReqCancelLeaderImpeachEvent() {
	eventDbId = (long)0;
}

public GC2GS_032_025_ReqCancelLeaderImpeachEvent(
	 long _eventDbId
) {	eventDbId = _eventDbId;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)25; }

/** 事件数据ID */
public long getEventDbId() { return eventDbId; }
/** 事件数据ID */
public void setEventDbId(long _eventDbId) { eventDbId = _eventDbId; }


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
	if(_buf.remaining() > 0) eventDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventDbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)25);
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

