package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 事件触发
 **/
public class GS2GC_040_057_OnMarsEventTrigger implements ALBasicProtocolPack._IALProtocolStructure {
/** 火星居民事件 */
private Common.MarsObj.Mars_Event marsEvent;


public GS2GC_040_057_OnMarsEventTrigger() {
	marsEvent = new Common.MarsObj.Mars_Event();
}

public GS2GC_040_057_OnMarsEventTrigger(
	 Common.MarsObj.Mars_Event _marsEvent
) {	marsEvent = _marsEvent;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)57; }

/** 火星居民事件 */
public Common.MarsObj.Mars_Event getMarsEvent() { return marsEvent; }
/** 火星居民事件 */
public void setMarsEvent(Common.MarsObj.Mars_Event _marsEvent) { marsEvent = _marsEvent; }


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
	if(_buf.remaining() <= 0) return;
	int _marsEventCustLen = _buf.getInt();
	int _marsEventCurPos = _buf.position();
	marsEvent.ReadUnzipBuf(_buf, _marsEventCurPos + _marsEventCustLen);
	_buf.position(_marsEventCurPos + _marsEventCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(marsEvent.GetBufSize());
	marsEvent.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)57);
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

