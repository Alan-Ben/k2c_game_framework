package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
public class GS2GC_042_009_RetMarsHelpAutoDailyRecord implements ALBasicProtocolPack._IALProtocolStructure {
private int count;


public GS2GC_042_009_RetMarsHelpAutoDailyRecord() {
	count = 0;
}

public GS2GC_042_009_RetMarsHelpAutoDailyRecord(
	 int _count
) {	count = _count;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)8; }

public int getCount() { return count; }
public void setCount(int _count) { count = _count; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)8);
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

