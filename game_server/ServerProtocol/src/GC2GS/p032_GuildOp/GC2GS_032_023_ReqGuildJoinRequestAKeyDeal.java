package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 一键处理入盟请求
 **/
public class GC2GS_032_023_ReqGuildJoinRequestAKeyDeal implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否同意 */
private boolean isAgree;


public GC2GS_032_023_ReqGuildJoinRequestAKeyDeal() {
	isAgree = false;
}

public GC2GS_032_023_ReqGuildJoinRequestAKeyDeal(
	 boolean _isAgree
) {	isAgree = _isAgree;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)23; }

/** 是否同意 */
public boolean getIsAgree() { return isAgree; }
/** 是否同意 */
public void setIsAgree(boolean _isAgree) { isAgree = _isAgree; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAgree = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAgree?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)23);
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

