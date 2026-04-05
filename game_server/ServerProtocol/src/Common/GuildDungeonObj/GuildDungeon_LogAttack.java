package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本日志-攻击
 **/
public class GuildDungeon_LogAttack implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 怪物ID */
private long monsterId;
/** 伤害数值 */
private long damage;


public GuildDungeon_LogAttack() {
	cid = (long)0;
	monsterId = (long)0;
	damage = (long)0;
}

public GuildDungeon_LogAttack(
	 long _cid
	, long _monsterId
	, long _damage
) {	cid = _cid;
	monsterId = _monsterId;
	damage = _damage;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 怪物ID */
public long getMonsterId() { return monsterId; }
/** 怪物ID */
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
/** 伤害数值 */
public long getDamage() { return damage; }
/** 伤害数值 */
public void setDamage(long _damage) { damage = _damage; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) monsterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) damage = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(monsterId);
	_buf.putLong(damage);
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

