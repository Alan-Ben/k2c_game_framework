package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 组合称号-称号后缀
 **/
public class PlayerInfo_ComboTitleSfx implements ALBasicProtocolPack._IALProtocolStructure {
/** 称号后缀 */
private long sfxId;
/** 是否查看过 */
private boolean viewed;


public PlayerInfo_ComboTitleSfx() {
	sfxId = (long)0;
	viewed = false;
}

public PlayerInfo_ComboTitleSfx(
	 long _sfxId
	, boolean _viewed
) {	sfxId = _sfxId;
	viewed = _viewed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 称号后缀 */
public long getSfxId() { return sfxId; }
/** 称号后缀 */
public void setSfxId(long _sfxId) { sfxId = _sfxId; }
/** 是否查看过 */
public boolean getViewed() { return viewed; }
/** 是否查看过 */
public void setViewed(boolean _viewed) { viewed = _viewed; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sfxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) viewed = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sfxId);
	_buf.put(viewed?(byte)1:(byte)0);
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

