package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 禁用cid
 **/
public class NP2US_R_003_012_ReqBanCid implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家cid */
private long cid;
/** 封禁时间 */
private long freezeTimeMs;


public NP2US_R_003_012_ReqBanCid() {
	cid = (long)0;
	freezeTimeMs = (long)0;
}

public NP2US_R_003_012_ReqBanCid(
	 long _cid
	, long _freezeTimeMs
) {	cid = _cid;
	freezeTimeMs = _freezeTimeMs;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)12; }

/** 玩家cid */
public long getCid() { return cid; }
/** 玩家cid */
public void setCid(long _cid) { cid = _cid; }
/** 封禁时间 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 封禁时间 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(freezeTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

