package GC2GS.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 穿戴组合称号
 **/
public class GC2GS_018_002_ReqSetComboTitle implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long preId;
/** 空 */
private long sfxId;
/** 空 */
private long bgId;


public GC2GS_018_002_ReqSetComboTitle() {
	preId = (long)0;
	sfxId = (long)0;
	bgId = (long)0;
}

public GC2GS_018_002_ReqSetComboTitle(
	 long _preId
	, long _sfxId
	, long _bgId
) {	preId = _preId;
	sfxId = _sfxId;
	bgId = _bgId;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)2; }

/** 空 */
public long getPreId() { return preId; }
/** 空 */
public void setPreId(long _preId) { preId = _preId; }
/** 空 */
public long getSfxId() { return sfxId; }
/** 空 */
public void setSfxId(long _sfxId) { sfxId = _sfxId; }
/** 空 */
public long getBgId() { return bgId; }
/** 空 */
public void setBgId(long _bgId) { bgId = _bgId; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) preId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sfxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bgId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(preId);
	_buf.putLong(sfxId);
	_buf.putLong(bgId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)2);
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

