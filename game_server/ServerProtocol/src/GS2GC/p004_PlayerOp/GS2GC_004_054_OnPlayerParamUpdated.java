package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_054_OnPlayerParamUpdated implements ALBasicProtocolPack._IALProtocolStructure {
private int index;
private long paramValue;


public GS2GC_004_054_OnPlayerParamUpdated() {
	index = 0;
	paramValue = (long)0;
}

public GS2GC_004_054_OnPlayerParamUpdated(
	 int _index
	, long _paramValue
) {	index = _index;
	paramValue = _paramValue;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)54; }

public int getIndex() { return index; }
public void setIndex(int _index) { index = _index; }
public long getParamValue() { return paramValue; }
public void setParamValue(long _paramValue) { paramValue = _paramValue; }


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
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) paramValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putLong(paramValue);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)54);
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

