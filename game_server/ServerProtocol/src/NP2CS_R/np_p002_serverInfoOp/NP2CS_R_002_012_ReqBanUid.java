package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_012_ReqBanUid implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家uid */
private String uid;
/** 冻结时长 */
private long freezeTimeMs;


public NP2CS_R_002_012_ReqBanUid() {
	uid = "";
	freezeTimeMs = (long)0;
}

public NP2CS_R_002_012_ReqBanUid(
	 String _uid
	, long _freezeTimeMs
) {	uid = _uid;
	freezeTimeMs = _freezeTimeMs;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)12; }

/** 玩家uid */
public String getUid() { return uid; }
/** 玩家uid */
public void setUid(String _uid) { uid = _uid; }
/** 冻结时长 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 冻结时长 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	_buf.putLong(freezeTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)12);
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

