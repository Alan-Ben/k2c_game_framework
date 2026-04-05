package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_056_RetCountdownEventInit implements ALBasicProtocolPack._IALProtocolStructure {
private Common.CountdownEventObj.CountdownEvent_Info eventInfo;


public GS2GC_002_056_RetCountdownEventInit() {
	eventInfo = new Common.CountdownEventObj.CountdownEvent_Info();
}

public GS2GC_002_056_RetCountdownEventInit(
	 Common.CountdownEventObj.CountdownEvent_Info _eventInfo
) {	eventInfo = _eventInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)56; }

public Common.CountdownEventObj.CountdownEvent_Info getEventInfo() { return eventInfo; }
public void setEventInfo(Common.CountdownEventObj.CountdownEvent_Info _eventInfo) { eventInfo = _eventInfo; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

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
	_buf.put((byte)2);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)56);
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

