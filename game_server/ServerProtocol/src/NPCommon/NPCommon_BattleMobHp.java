package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 战斗怪物血量信息
 **/
public class NPCommon_BattleMobHp implements ALBasicProtocolPack._IALProtocolStructure {
/** mission_lineup 配表id */
private int lineupMobId;
/** 血量万分比 */
private short hp;
/** 最大血量 */
private long hpMax;


public NPCommon_BattleMobHp() {
	lineupMobId = 0;
	hp = (short)0;
	hpMax = (long)0;
}

public NPCommon_BattleMobHp(
	 int _lineupMobId
	, short _hp
	, long _hpMax
) {	lineupMobId = _lineupMobId;
	hp = _hp;
	hpMax = _hpMax;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** mission_lineup 配表id */
public int getLineupMobId() { return lineupMobId; }
/** mission_lineup 配表id */
public void setLineupMobId(int _lineupMobId) { lineupMobId = _lineupMobId; }
/** 血量万分比 */
public short getHp() { return hp; }
/** 血量万分比 */
public void setHp(short _hp) { hp = _hp; }
/** 最大血量 */
public long getHpMax() { return hpMax; }
/** 最大血量 */
public void setHpMax(long _hpMax) { hpMax = _hpMax; }


public final int GetBufSize() {
	int _size = 14;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lineupMobId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hp = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hpMax = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lineupMobId);
	_buf.putShort(hp);
	_buf.putLong(hpMax);
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

