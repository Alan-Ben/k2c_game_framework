package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_Skininfo implements ALBasicProtocolPack._IALProtocolStructure {
private long skinId;
private long remaniningTime;
private boolean isUsing;


public WCGGS2GC_Skininfo() {
	skinId = (long)0;
	remaniningTime = (long)0;
	isUsing = false;
}

public WCGGS2GC_Skininfo(
	 long _skinId
	, long _remaniningTime
	, boolean _isUsing
) {	skinId = _skinId;
	remaniningTime = _remaniningTime;
	isUsing = _isUsing;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }
public long getRemaniningTime() { return remaniningTime; }
public void setRemaniningTime(long _remaniningTime) { remaniningTime = _remaniningTime; }
public boolean getIsUsing() { return isUsing; }
public void setIsUsing(boolean _isUsing) { isUsing = _isUsing; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remaniningTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUsing = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(skinId);
	_buf.putLong(remaniningTime);
	_buf.put(isUsing?(byte)1:(byte)0);
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

