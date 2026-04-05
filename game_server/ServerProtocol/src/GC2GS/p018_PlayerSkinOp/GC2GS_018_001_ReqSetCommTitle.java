package GC2GS.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 穿戴普通称号
 **/
public class GC2GS_018_001_ReqSetCommTitle implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long title;


public GC2GS_018_001_ReqSetCommTitle() {
	title = (long)0;
}

public GC2GS_018_001_ReqSetCommTitle(
	 long _title
) {	title = _title;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)1; }

/** 空 */
public long getTitle() { return title; }
/** 空 */
public void setTitle(long _title) { title = _title; }


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
	if(_buf.remaining() > 0) title = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(title);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)1);
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

