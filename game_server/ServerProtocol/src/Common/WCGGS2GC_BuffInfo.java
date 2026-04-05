package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_BuffInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long buffId;
private long addTimeMs;
private long leftTimeMs;
private int layer;


public WCGGS2GC_BuffInfo() {
	buffId = (long)0;
	addTimeMs = (long)0;
	leftTimeMs = (long)0;
	layer = 0;
}

public WCGGS2GC_BuffInfo(
	 long _buffId
	, long _addTimeMs
	, long _leftTimeMs
	, int _layer
) {	buffId = _buffId;
	addTimeMs = _addTimeMs;
	leftTimeMs = _leftTimeMs;
	layer = _layer;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }
public long getAddTimeMs() { return addTimeMs; }
public void setAddTimeMs(long _addTimeMs) { addTimeMs = _addTimeMs; }
public long getLeftTimeMs() { return leftTimeMs; }
public void setLeftTimeMs(long _leftTimeMs) { leftTimeMs = _leftTimeMs; }
public int getLayer() { return layer; }
public void setLayer(int _layer) { layer = _layer; }


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
	if(_buf.remaining() > 0) buffId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leftTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) layer = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buffId);
	_buf.putLong(addTimeMs);
	_buf.putLong(leftTimeMs);
	_buf.putInt(layer);
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

