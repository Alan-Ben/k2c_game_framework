package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 设置联盟宝箱匿名分享
 **/
public class GC2GS_042_008_ReqSetGuildBoxShareAnonymous implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否匿名 true-匿名 */
private boolean isAnonymous;


public GC2GS_042_008_ReqSetGuildBoxShareAnonymous() {
	isAnonymous = false;
}

public GC2GS_042_008_ReqSetGuildBoxShareAnonymous(
	 boolean _isAnonymous
) {	isAnonymous = _isAnonymous;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)8; }

/** 是否匿名 true-匿名 */
public boolean getIsAnonymous() { return isAnonymous; }
/** 是否匿名 true-匿名 */
public void setIsAnonymous(boolean _isAnonymous) { isAnonymous = _isAnonymous; }


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
	if(_buf.remaining() > 0) isAnonymous = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAnonymous?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)8);
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

