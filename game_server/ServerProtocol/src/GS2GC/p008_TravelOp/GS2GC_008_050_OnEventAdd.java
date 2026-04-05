package GS2GC.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 新增事件推送
 **/
public class GS2GC_008_050_OnEventAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.TravelObj.Travel_Event eventInfo;


public GS2GC_008_050_OnEventAdd() {
	eventInfo = new Common.TravelObj.Travel_Event();
}

public GS2GC_008_050_OnEventAdd(
	 Common.TravelObj.Travel_Event _eventInfo
) {	eventInfo = _eventInfo;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)50; }

/** 空 */
public Common.TravelObj.Travel_Event getEventInfo() { return eventInfo; }
/** 空 */
public void setEventInfo(Common.TravelObj.Travel_Event _eventInfo) { eventInfo = _eventInfo; }


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
	_buf.put((byte)8);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)50);
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

