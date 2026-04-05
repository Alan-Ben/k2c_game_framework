package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_UniformItemTranform implements ALBasicProtocolPack._IALProtocolStructure {
private long fromId;
private long fromNum;
private long toId;
private long toNum;


public WCGGS2GC_UniformItemTranform() {
	fromId = (long)0;
	fromNum = (long)0;
	toId = (long)0;
	toNum = (long)0;
}

public WCGGS2GC_UniformItemTranform(
	 long _fromId
	, long _fromNum
	, long _toId
	, long _toNum
) {	fromId = _fromId;
	fromNum = _fromNum;
	toId = _toId;
	toNum = _toNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getFromId() { return fromId; }
public void setFromId(long _fromId) { fromId = _fromId; }
public long getFromNum() { return fromNum; }
public void setFromNum(long _fromNum) { fromNum = _fromNum; }
public long getToId() { return toId; }
public void setToId(long _toId) { toId = _toId; }
public long getToNum() { return toNum; }
public void setToNum(long _toNum) { toNum = _toNum; }


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
	if(_buf.remaining() > 0) fromId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fromNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) toId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) toNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fromId);
	_buf.putLong(fromNum);
	_buf.putLong(toId);
	_buf.putLong(toNum);
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

