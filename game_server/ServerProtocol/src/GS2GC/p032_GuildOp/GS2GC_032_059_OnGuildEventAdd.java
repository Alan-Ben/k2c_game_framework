package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_059_OnGuildEventAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件信息 */
private Common.GuildObj.Guild_EventInfo eventInfo;


public GS2GC_032_059_OnGuildEventAdd() {
	eventInfo = new Common.GuildObj.Guild_EventInfo();
}

public GS2GC_032_059_OnGuildEventAdd(
	 Common.GuildObj.Guild_EventInfo _eventInfo
) {	eventInfo = _eventInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)59; }

/** 事件信息 */
public Common.GuildObj.Guild_EventInfo getEventInfo() { return eventInfo; }
/** 事件信息 */
public void setEventInfo(Common.GuildObj.Guild_EventInfo _eventInfo) { eventInfo = _eventInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + eventInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + eventInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _eventInfoCustLen = _buf.getInt();
	int _eventInfoCurPos = _buf.position();
	eventInfo.ReadUnzipBuf(_buf, _eventInfoCurPos + _eventInfoCustLen);
	_buf.position(_eventInfoCurPos + _eventInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(eventInfo.GetBufSize());
	eventInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)59);
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

