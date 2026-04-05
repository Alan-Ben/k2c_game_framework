package GS2GC.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星系统-额外心情指数变化
 **/
public class GS2GC_039_061_OnExtMoodIndexChg implements ALBasicProtocolPack._IALProtocolStructure {
private long value;


public GS2GC_039_061_OnExtMoodIndexChg() {
	value = (long)0;
}

public GS2GC_039_061_OnExtMoodIndexChg(
	 long _value
) {	value = _value;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)61; }

public long getValue() { return value; }
public void setValue(long _value) { value = _value; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(value);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)61);
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

