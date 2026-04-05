package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsGetPreDinnerInfoBySort_Return implements ALBasicProtocolPack._IALProtocolStructure {
private long instanceId;
private int idx;
private boolean hasPre;
private boolean hasNext;


public DnsGetPreDinnerInfoBySort_Return() {
	instanceId = (long)0;
	idx = 0;
	hasPre = false;
	hasNext = false;
}

public DnsGetPreDinnerInfoBySort_Return(
	 long _instanceId
	, int _idx
	, boolean _hasPre
	, boolean _hasNext
) {	instanceId = _instanceId;
	idx = _idx;
	hasPre = _hasPre;
	hasNext = _hasNext;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public int getIdx() { return idx; }
public void setIdx(int _idx) { idx = _idx; }
public boolean getHasPre() { return hasPre; }
public void setHasPre(boolean _hasPre) { hasPre = _hasPre; }
public boolean getHasNext() { return hasNext; }
public void setHasNext(boolean _hasNext) { hasNext = _hasNext; }


public final int GetBufSize() {
	int _size = 14;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) idx = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasPre = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasNext = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(idx);
	_buf.put(hasPre?(byte)1:(byte)0);
	_buf.put(hasNext?(byte)1:(byte)0);
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

