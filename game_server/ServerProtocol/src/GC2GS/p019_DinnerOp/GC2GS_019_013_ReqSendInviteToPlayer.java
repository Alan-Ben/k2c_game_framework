package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 对指定玩家发起邀请
 **/
public class GC2GS_019_013_ReqSendInviteToPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标玩家CID */
private long targetCid;


public GC2GS_019_013_ReqSendInviteToPlayer() {
	targetCid = (long)0;
}

public GC2GS_019_013_ReqSendInviteToPlayer(
	 long _targetCid
) {	targetCid = _targetCid;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)13; }

/** 目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }


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
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)13);
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

