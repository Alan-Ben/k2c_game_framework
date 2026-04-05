package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 联盟副本攻击信息
 **/
public class GuildOp_037_005_AttackDungeonInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private long power;
/** 攻击次数用于计算贡献 */
private int fightedCount;


public GuildOp_037_005_AttackDungeonInfo() {
	heroId = (long)0;
	power = (long)0;
	fightedCount = 0;
}

public GuildOp_037_005_AttackDungeonInfo(
	 long _heroId
	, long _power
	, int _fightedCount
) {	heroId = _heroId;
	power = _power;
	fightedCount = _fightedCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getPower() { return power; }
public void setPower(long _power) { power = _power; }
/** 攻击次数用于计算贡献 */
public int getFightedCount() { return fightedCount; }
/** 攻击次数用于计算贡献 */
public void setFightedCount(int _fightedCount) { fightedCount = _fightedCount; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) power = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fightedCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(power);
	_buf.putInt(fightedCount);
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

