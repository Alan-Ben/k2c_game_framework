package NP2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GS_001_004_OnEnterUSButFreeze implements ALBasicProtocolPack._IALProtocolStructure {
private long gsSessionId;
private long cid;
private long freezeTimeMs;
private long infoSerialize;


public NP2GS_001_004_OnEnterUSButFreeze() {
	gsSessionId = (long)0;
	cid = (long)0;
	freezeTimeMs = (long)0;
	infoSerialize = (long)0;
}

public NP2GS_001_004_OnEnterUSButFreeze(
	 long _gsSessionId
	, long _cid
	, long _freezeTimeMs
	, long _infoSerialize
) {	gsSessionId = _gsSessionId;
	cid = _cid;
	freezeTimeMs = _freezeTimeMs;
	infoSerialize = _infoSerialize;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

public long getGsSessionId() { return gsSessionId; }
public void setGsSessionId(long _gsSessionId) { gsSessionId = _gsSessionId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getFreezeTimeMs() { return freezeTimeMs; }
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }
public long getInfoSerialize() { return infoSerialize; }
public void setInfoSerialize(long _infoSerialize) { infoSerialize = _infoSerialize; }


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
	if(_buf.remaining() > 0) gsSessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) infoSerialize = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(gsSessionId);
	_buf.putLong(cid);
	_buf.putLong(freezeTimeMs);
	_buf.putLong(infoSerialize);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

