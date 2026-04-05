package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 副本升级
 **/
public class GC2GS_037_004_ReqUpgradeDungeonLvl implements ALBasicProtocolPack._IALProtocolStructure {
private long dungeonId;
/** 当前等级 */
private int curLvl;


public GC2GS_037_004_ReqUpgradeDungeonLvl() {
	dungeonId = (long)0;
	curLvl = 0;
}

public GC2GS_037_004_ReqUpgradeDungeonLvl(
	 long _dungeonId
	, int _curLvl
) {	dungeonId = _dungeonId;
	curLvl = _curLvl;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)4; }

public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/** 当前等级 */
public int getCurLvl() { return curLvl; }
/** 当前等级 */
public void setCurLvl(int _curLvl) { curLvl = _curLvl; }


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
	if(_buf.remaining() > 0) curLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dungeonId);
	_buf.putInt(curLvl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)4);
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

