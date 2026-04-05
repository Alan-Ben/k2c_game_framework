package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本数据
 **/
public class GuildDungeon_SetInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会副本ID */
private long dungeonId;
/** 公会副本等级 */
private int lvl;


public GuildDungeon_SetInfo() {
	dungeonId = (long)0;
	lvl = 0;
}

public GuildDungeon_SetInfo(
	 long _dungeonId
	, int _lvl
) {	dungeonId = _dungeonId;
	lvl = _lvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 公会副本ID */
public long getDungeonId() { return dungeonId; }
/** 公会副本ID */
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/** 公会副本等级 */
public int getLvl() { return lvl; }
/** 公会副本等级 */
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
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dungeonId);
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

