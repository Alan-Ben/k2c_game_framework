package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-战斗NPC信息
 **/
public class Mars_ExploreBattleNPCInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 原士兵数 */
private long oriSoldierNum;
/** 受伤士兵数 */
private long hurtSoldierNum;
/** 单个士兵战力 */
private long singleSoldierPower;


public Mars_ExploreBattleNPCInfo() {
	oriSoldierNum = (long)0;
	hurtSoldierNum = (long)0;
	singleSoldierPower = (long)0;
}

public Mars_ExploreBattleNPCInfo(
	 long _oriSoldierNum
	, long _hurtSoldierNum
	, long _singleSoldierPower
) {	oriSoldierNum = _oriSoldierNum;
	hurtSoldierNum = _hurtSoldierNum;
	singleSoldierPower = _singleSoldierPower;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 原士兵数 */
public long getOriSoldierNum() { return oriSoldierNum; }
/** 原士兵数 */
public void setOriSoldierNum(long _oriSoldierNum) { oriSoldierNum = _oriSoldierNum; }
/** 受伤士兵数 */
public long getHurtSoldierNum() { return hurtSoldierNum; }
/** 受伤士兵数 */
public void setHurtSoldierNum(long _hurtSoldierNum) { hurtSoldierNum = _hurtSoldierNum; }
/** 单个士兵战力 */
public long getSingleSoldierPower() { return singleSoldierPower; }
/** 单个士兵战力 */
public void setSingleSoldierPower(long _singleSoldierPower) { singleSoldierPower = _singleSoldierPower; }


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
	if(_buf.remaining() > 0) oriSoldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hurtSoldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) singleSoldierPower = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oriSoldierNum);
	_buf.putLong(hurtSoldierNum);
	_buf.putLong(singleSoldierPower);
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

