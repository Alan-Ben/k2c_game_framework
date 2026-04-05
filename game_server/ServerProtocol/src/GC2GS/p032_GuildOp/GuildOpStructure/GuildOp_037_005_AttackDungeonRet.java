package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 攻击副本的返回信息
 **/
public class GuildOp_037_005_AttackDungeonRet implements ALBasicProtocolPack._IALProtocolStructure {
private long dungeonId;
private long gainGuildExp;
private long gainGuildCointCount;
private long guildDevoteCount;
private boolean isKilled;


public GuildOp_037_005_AttackDungeonRet() {
	dungeonId = (long)0;
	gainGuildExp = (long)0;
	gainGuildCointCount = (long)0;
	guildDevoteCount = (long)0;
	isKilled = false;
}

public GuildOp_037_005_AttackDungeonRet(
	 long _dungeonId
	, long _gainGuildExp
	, long _gainGuildCointCount
	, long _guildDevoteCount
	, boolean _isKilled
) {	dungeonId = _dungeonId;
	gainGuildExp = _gainGuildExp;
	gainGuildCointCount = _gainGuildCointCount;
	guildDevoteCount = _guildDevoteCount;
	isKilled = _isKilled;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
public long getGainGuildExp() { return gainGuildExp; }
public void setGainGuildExp(long _gainGuildExp) { gainGuildExp = _gainGuildExp; }
public long getGainGuildCointCount() { return gainGuildCointCount; }
public void setGainGuildCointCount(long _gainGuildCointCount) { gainGuildCointCount = _gainGuildCointCount; }
public long getGuildDevoteCount() { return guildDevoteCount; }
public void setGuildDevoteCount(long _guildDevoteCount) { guildDevoteCount = _guildDevoteCount; }
public boolean getIsKilled() { return isKilled; }
public void setIsKilled(boolean _isKilled) { isKilled = _isKilled; }


public final int GetBufSize() {
	int _size = 33;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainGuildExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainGuildCointCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildDevoteCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isKilled = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dungeonId);
	_buf.putLong(gainGuildExp);
	_buf.putLong(gainGuildCointCount);
	_buf.putLong(guildDevoteCount);
	_buf.put(isKilled?(byte)1:(byte)0);
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

