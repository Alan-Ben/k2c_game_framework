package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 拒绝对玩家的指定联姻请求
 **/
public class GC2GS_014_009_ReqRefuseToMeApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 发起请求的玩家子嗣实例ID */
private long applyAdultId;


public GC2GS_014_009_ReqRefuseToMeApply() {
	applyAdultId = (long)0;
}

public GC2GS_014_009_ReqRefuseToMeApply(
	 long _applyAdultId
) {	applyAdultId = _applyAdultId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)9; }

/** 发起请求的玩家子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 发起请求的玩家子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


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
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyAdultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)9);
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

