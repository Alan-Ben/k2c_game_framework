package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_073_OnRecuritRecordAdd implements ALBasicProtocolPack._IALProtocolStructure {
private long recuritId;


public GS2GC_007_073_OnRecuritRecordAdd() {
	recuritId = (long)0;
}

public GS2GC_007_073_OnRecuritRecordAdd(
	 long _recuritId
) {	recuritId = _recuritId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)73; }

public long getRecuritId() { return recuritId; }
public void setRecuritId(long _recuritId) { recuritId = _recuritId; }


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
	if(_buf.remaining() > 0) recuritId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(recuritId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)73);
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

