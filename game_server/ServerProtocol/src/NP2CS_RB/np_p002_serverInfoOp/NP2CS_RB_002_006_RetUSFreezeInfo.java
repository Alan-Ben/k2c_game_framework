package NP2CS_RB.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_RB_002_006_RetUSFreezeInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否被冻结 */
private boolean isFreeze;
/** 冻结结束时间 */
private long freezeTimeMs;
/** 是否在白名单中 */
private boolean inWhitelist;


public NP2CS_RB_002_006_RetUSFreezeInfo() {
	isFreeze = false;
	freezeTimeMs = (long)0;
	inWhitelist = false;
}

public NP2CS_RB_002_006_RetUSFreezeInfo(
	 boolean _isFreeze
	, long _freezeTimeMs
	, boolean _inWhitelist
) {	isFreeze = _isFreeze;
	freezeTimeMs = _freezeTimeMs;
	inWhitelist = _inWhitelist;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)6; }

/** 是否被冻结 */
public boolean getIsFreeze() { return isFreeze; }
/** 是否被冻结 */
public void setIsFreeze(boolean _isFreeze) { isFreeze = _isFreeze; }
/** 冻结结束时间 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 冻结结束时间 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }
/** 是否在白名单中 */
public boolean getInWhitelist() { return inWhitelist; }
/** 是否在白名单中 */
public void setInWhitelist(boolean _inWhitelist) { inWhitelist = _inWhitelist; }


public final int GetBufSize() {
	int _size = 10;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) inWhitelist = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(freezeTimeMs);
	_buf.put(inWhitelist?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
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

