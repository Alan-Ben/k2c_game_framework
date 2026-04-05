package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 关卡事件信息变更
 **/
public class GS2GC_016_053_OnChapterEventChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件信息 */
private Common.ChapterObj.Chapter_EventInfo eventInfo;


public GS2GC_016_053_OnChapterEventChg() {
	eventInfo = new Common.ChapterObj.Chapter_EventInfo();
}

public GS2GC_016_053_OnChapterEventChg(
	 Common.ChapterObj.Chapter_EventInfo _eventInfo
) {	eventInfo = _eventInfo;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)53; }

/** 事件信息 */
public Common.ChapterObj.Chapter_EventInfo getEventInfo() { return eventInfo; }
/** 事件信息 */
public void setEventInfo(Common.ChapterObj.Chapter_EventInfo _eventInfo) { eventInfo = _eventInfo; }


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
	_buf.put((byte)16);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)53);
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

