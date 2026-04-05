package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 设置玩家捏脸数据
 **/
public class GC2GS_004_032_ReqUnsetShieldPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 屏蔽玩家CID */
private long shieldCid;


public GC2GS_004_032_ReqUnsetShieldPlayer() {
	shieldCid = (long)0;
}

public GC2GS_004_032_ReqUnsetShieldPlayer(
	 long _shieldCid
) {	shieldCid = _shieldCid;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)32; }

/** 屏蔽玩家CID */
public long getShieldCid() { return shieldCid; }
/** 屏蔽玩家CID */
public void setShieldCid(long _shieldCid) { shieldCid = _shieldCid; }


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
	if(_buf.remaining() > 0) shieldCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shieldCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)32);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)32);
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

