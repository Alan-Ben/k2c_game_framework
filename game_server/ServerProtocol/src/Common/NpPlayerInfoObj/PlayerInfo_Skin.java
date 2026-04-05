package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 玩家皮肤
 **/
public class PlayerInfo_Skin implements ALBasicProtocolPack._IALProtocolStructure {
/** 皮肤ID */
private long skinId;
/** 皮肤等级 */
private int lvl;


public PlayerInfo_Skin() {
	skinId = (long)0;
	lvl = 0;
}

public PlayerInfo_Skin(
	 long _skinId
	, int _lvl
) {	skinId = _skinId;
	lvl = _lvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 皮肤ID */
public long getSkinId() { return skinId; }
/** 皮肤ID */
public void setSkinId(long _skinId) { skinId = _skinId; }
/** 皮肤等级 */
public int getLvl() { return lvl; }
/** 皮肤等级 */
public void setLvl(int _lvl) { lvl = _lvl; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(skinId);
	_buf.putInt(lvl);
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

