package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本怪物数据
 **/
public class GuildDungeon_Monster implements ALBasicProtocolPack._IALProtocolStructure {
/** 怪物ID */
private long monsterId;
/** 有奖励的怪物 */
private boolean isReward;
/** 当前怪物血量 */
private long hp;


public GuildDungeon_Monster() {
	monsterId = (long)0;
	isReward = false;
	hp = (long)0;
}

public GuildDungeon_Monster(
	 long _monsterId
	, boolean _isReward
	, long _hp
) {	monsterId = _monsterId;
	isReward = _isReward;
	hp = _hp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 怪物ID */
public long getMonsterId() { return monsterId; }
/** 怪物ID */
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
/** 有奖励的怪物 */
public boolean getIsReward() { return isReward; }
/** 有奖励的怪物 */
public void setIsReward(boolean _isReward) { isReward = _isReward; }
/** 当前怪物血量 */
public long getHp() { return hp; }
/** 当前怪物血量 */
public void setHp(long _hp) { hp = _hp; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) monsterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(monsterId);
	_buf.put(isReward?(byte)1:(byte)0);
	_buf.putLong(hp);
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

