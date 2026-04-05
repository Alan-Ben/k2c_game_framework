package NP2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GS_001_003_OnUserDataLoaded implements ALBasicProtocolPack._IALProtocolStructure {
/** 错误码，0为成功 */
private int errCode;
private long gsSessionId;
private long infoSerialize;
private long cid;


public NP2GS_001_003_OnUserDataLoaded() {
	errCode = 0;
	gsSessionId = (long)0;
	infoSerialize = (long)0;
	cid = (long)0;
}

public NP2GS_001_003_OnUserDataLoaded(
	 int _errCode
	, long _gsSessionId
	, long _infoSerialize
	, long _cid
) {	errCode = _errCode;
	gsSessionId = _gsSessionId;
	infoSerialize = _infoSerialize;
	cid = _cid;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 错误码，0为成功 */
public int getErrCode() { return errCode; }
/** 错误码，0为成功 */
public void setErrCode(int _errCode) { errCode = _errCode; }
public long getGsSessionId() { return gsSessionId; }
public void setGsSessionId(long _gsSessionId) { gsSessionId = _gsSessionId; }
public long getInfoSerialize() { return infoSerialize; }
public void setInfoSerialize(long _infoSerialize) { infoSerialize = _infoSerialize; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gsSessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) infoSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
	_buf.putLong(gsSessionId);
	_buf.putLong(infoSerialize);
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

