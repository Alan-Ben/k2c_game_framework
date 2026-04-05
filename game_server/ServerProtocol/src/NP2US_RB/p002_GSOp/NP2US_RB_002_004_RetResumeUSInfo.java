package NP2US_RB.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_RB_002_004_RetResumeUSInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;


public NP2US_RB_002_004_RetResumeUSInfo() {
	cid = (long)0;
}

public NP2US_RB_002_004_RetResumeUSInfo(
	 long _cid
) {	cid = _cid;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)4; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)4);
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

