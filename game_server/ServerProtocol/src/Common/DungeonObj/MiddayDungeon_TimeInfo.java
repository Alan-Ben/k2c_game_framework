package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本时间信息
 **/
public class MiddayDungeon_TimeInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 预告时间戳 */
private long previewTimeMs;
/** 开始时间戳 */
private long startTimeMs;
/** 结束时间戳 */
private long endTimeMs;


public MiddayDungeon_TimeInfo() {
	previewTimeMs = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
}

public MiddayDungeon_TimeInfo(
	 long _previewTimeMs
	, long _startTimeMs
	, long _endTimeMs
) {	previewTimeMs = _previewTimeMs;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 预告时间戳 */
public long getPreviewTimeMs() { return previewTimeMs; }
/** 预告时间戳 */
public void setPreviewTimeMs(long _previewTimeMs) { previewTimeMs = _previewTimeMs; }
/** 开始时间戳 */
public long getStartTimeMs() { return startTimeMs; }
/** 开始时间戳 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 结束时间戳 */
public long getEndTimeMs() { return endTimeMs; }
/** 结束时间戳 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }


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
	if(_buf.remaining() > 0) previewTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(previewTimeMs);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
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

