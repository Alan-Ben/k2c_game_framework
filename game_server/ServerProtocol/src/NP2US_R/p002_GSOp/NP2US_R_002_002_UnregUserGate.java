package NP2US_R.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_R_002_002_UnregUserGate implements ALBasicProtocolPack._IALProtocolStructure {
private long sessionId;
private long cid;
private long serialize;


public NP2US_R_002_002_UnregUserGate() {
	sessionId = (long)0;
	cid = (long)0;
	serialize = (long)0;
}

public NP2US_R_002_002_UnregUserGate(
	 long _sessionId
	, long _cid
	, long _serialize
) {	sessionId = _sessionId;
	cid = _cid;
	serialize = _serialize;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)2; }

public long getSessionId() { return sessionId; }
public void setSessionId(long _sessionId) { sessionId = _sessionId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getSerialize() { return serialize; }
public void setSerialize(long _serialize) { serialize = _serialize; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialize = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sessionId);
	_buf.putLong(cid);
	_buf.putLong(serialize);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)2);
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

