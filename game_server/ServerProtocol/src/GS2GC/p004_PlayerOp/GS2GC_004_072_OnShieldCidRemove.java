package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 屏蔽玩家CID移除推送
 **/
public class GS2GC_004_072_OnShieldCidRemove implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;


public GS2GC_004_072_OnShieldCidRemove() {
	cid = (long)0;
}

public GS2GC_004_072_OnShieldCidRemove(
	 long _cid
) {	cid = _cid;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)72; }

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
	_buf.put((byte)4);
	_buf.put((byte)72);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)72);
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

