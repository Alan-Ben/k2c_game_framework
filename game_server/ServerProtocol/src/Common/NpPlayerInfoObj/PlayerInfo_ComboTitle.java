package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 组合称号
 **/
public class PlayerInfo_ComboTitle implements ALBasicProtocolPack._IALProtocolStructure {
/** 称号前缀 */
private long preId;
/** 称号后缀 */
private long sfxId;
/** 称号底色 */
private long bgId;


public PlayerInfo_ComboTitle() {
	preId = (long)0;
	sfxId = (long)0;
	bgId = (long)0;
}

public PlayerInfo_ComboTitle(
	 long _preId
	, long _sfxId
	, long _bgId
) {	preId = _preId;
	sfxId = _sfxId;
	bgId = _bgId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 称号前缀 */
public long getPreId() { return preId; }
/** 称号前缀 */
public void setPreId(long _preId) { preId = _preId; }
/** 称号后缀 */
public long getSfxId() { return sfxId; }
/** 称号后缀 */
public void setSfxId(long _sfxId) { sfxId = _sfxId; }
/** 称号底色 */
public long getBgId() { return bgId; }
/** 称号底色 */
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
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

