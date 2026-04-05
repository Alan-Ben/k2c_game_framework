package GC2GS.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 查看组合称号前缀
 **/
public class GC2GS_018_007_ReqviewComboTitlePre implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long preId;


public GC2GS_018_007_ReqviewComboTitlePre() {
	preId = (long)0;
}

public GC2GS_018_007_ReqviewComboTitlePre(
	 long _preId
) {	preId = _preId;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)7; }

/** 空 */
public long getPreId() { return preId; }
/** 空 */
public void setPreId(long _preId) { preId = _preId; }


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
	if(_buf.remaining() > 0) preId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(preId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)7);
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

