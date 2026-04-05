package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_PlayerBuffInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long buffId;
private int layer;
private long endMs;
private long startMs;


public NPCommon_PlayerBuffInfo() {
	buffId = (long)0;
	layer = 0;
	endMs = (long)0;
	startMs = (long)0;
}

public NPCommon_PlayerBuffInfo(
	 long _buffId
	, int _layer
	, long _endMs
	, long _startMs
) {	buffId = _buffId;
	layer = _layer;
	endMs = _endMs;
	startMs = _startMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }
public int getLayer() { return layer; }
public void setLayer(int _layer) { layer = _layer; }
public long getEndMs() { return endMs; }
public void setEndMs(long _endMs) { endMs = _endMs; }
public long getStartMs() { return startMs; }
public void setStartMs(long _startMs) { startMs = _startMs; }


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
	if(_buf.remaining() > 0) layer = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buffId);
	_buf.putInt(layer);
	_buf.putLong(endMs);
	_buf.putLong(startMs);
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

