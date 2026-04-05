package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 副本怪物数据变化
 **/
public class GS2GC_037_052_OnDungeonMonsterChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会副本实例ID */
private long id;
/** 怪物ID */
private long monsterId;
/** 当前血量 */
private long hp;


public GS2GC_037_052_OnDungeonMonsterChg() {
	id = (long)0;
	monsterId = (long)0;
	hp = (long)0;
}

public GS2GC_037_052_OnDungeonMonsterChg(
	 long _id
	, long _monsterId
	, long _hp
) {	id = _id;
	monsterId = _monsterId;
	hp = _hp;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)52; }

/** 公会副本实例ID */
public long getId() { return id; }
/** 公会副本实例ID */
public void setId(long _id) { id = _id; }
/** 怪物ID */
public long getMonsterId() { return monsterId; }
/** 怪物ID */
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
/** 当前血量 */
public long getHp() { return hp; }
/** 当前血量 */
public void setHp(long _hp) { hp = _hp; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) monsterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(monsterId);
	_buf.putLong(hp);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)52);
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

