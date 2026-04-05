package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 攻击副本怪物
 **/
public class GC2GS_037_005_ReqAttackDungeon implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会副本实例ID */
private long id;
private long monsterId;
private long heroId;


public GC2GS_037_005_ReqAttackDungeon() {
	id = (long)0;
	monsterId = (long)0;
	heroId = (long)0;
}

public GC2GS_037_005_ReqAttackDungeon(
	 long _id
	, long _monsterId
	, long _heroId
) {	id = _id;
	monsterId = _monsterId;
	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)5; }

/** 公会副本实例ID */
public long getId() { return id; }
/** 公会副本实例ID */
public void setId(long _id) { id = _id; }
public long getMonsterId() { return monsterId; }
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(monsterId);
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)5);
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

