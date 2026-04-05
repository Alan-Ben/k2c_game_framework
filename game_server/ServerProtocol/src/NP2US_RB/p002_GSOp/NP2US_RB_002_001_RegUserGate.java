package NP2US_RB.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_RB_002_001_RegUserGate implements ALBasicProtocolPack._IALProtocolStructure {
private boolean res;
private long newSerialize;


public NP2US_RB_002_001_RegUserGate() {
	res = false;
	newSerialize = (long)0;
}

public NP2US_RB_002_001_RegUserGate(
	 boolean _res
	, long _newSerialize
) {	res = _res;
	newSerialize = _newSerialize;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public long getNewSerialize() { return newSerialize; }
public void setNewSerialize(long _newSerialize) { newSerialize = _newSerialize; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newSerialize = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(res?(byte)1:(byte)0);
	_buf.putLong(newSerialize);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)1);
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

