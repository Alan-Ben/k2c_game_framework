package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 组合称号-称号前缀
 **/
public class PlayerInfo_ComboTitlePre implements ALBasicProtocolPack._IALProtocolStructure {
/** 称号前缀 */
private long preId;
/** 是否查看过 */
private boolean viewed;


public PlayerInfo_ComboTitlePre() {
	preId = (long)0;
	viewed = false;
}

public PlayerInfo_ComboTitlePre(
	 long _preId
	, boolean _viewed
) {	preId = _preId;
	viewed = _viewed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 称号前缀 */
public long getPreId() { return preId; }
/** 称号前缀 */
public void setPreId(long _preId) { preId = _preId; }
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
	if(_buf.remaining() > 0) preId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) viewed = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(preId);
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

