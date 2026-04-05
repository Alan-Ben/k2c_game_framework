package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_TimerProductInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long refId;
private long unitMultiple;
private long unitDuration;
private long lastEndTimeMs;


public NPCommon_TimerProductInfo() {
	refId = (long)0;
	unitMultiple = (long)0;
	unitDuration = (long)0;
	lastEndTimeMs = (long)0;
}

public NPCommon_TimerProductInfo(
	 long _refId
	, long _unitMultiple
	, long _unitDuration
	, long _lastEndTimeMs
) {	refId = _refId;
	unitMultiple = _unitMultiple;
	unitDuration = _unitDuration;
	lastEndTimeMs = _lastEndTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
public long getUnitMultiple() { return unitMultiple; }
public void setUnitMultiple(long _unitMultiple) { unitMultiple = _unitMultiple; }
public long getUnitDuration() { return unitDuration; }
public void setUnitDuration(long _unitDuration) { unitDuration = _unitDuration; }
public long getLastEndTimeMs() { return lastEndTimeMs; }
public void setLastEndTimeMs(long _lastEndTimeMs) { lastEndTimeMs = _lastEndTimeMs; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) unitMultiple = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) unitDuration = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastEndTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
	_buf.putLong(unitMultiple);
	_buf.putLong(unitDuration);
	_buf.putLong(lastEndTimeMs);
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

