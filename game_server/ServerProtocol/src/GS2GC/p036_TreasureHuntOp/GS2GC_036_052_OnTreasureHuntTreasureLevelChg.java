package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-奇物等级变更
 **/
public class GS2GC_036_052_OnTreasureHuntTreasureLevelChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 奇物ID */
private long treasureId;
/** 奇物等级 */
private int level;


public GS2GC_036_052_OnTreasureHuntTreasureLevelChg() {
	treasureId = (long)0;
	level = 0;
}

public GS2GC_036_052_OnTreasureHuntTreasureLevelChg(
	 long _treasureId
	, int _level
) {	treasureId = _treasureId;
	level = _level;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)52; }

/** 奇物ID */
public long getTreasureId() { return treasureId; }
/** 奇物ID */
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/** 奇物等级 */
public int getLevel() { return level; }
/** 奇物等级 */
public void setLevel(int _level) { level = _level; }


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
	if(_buf.remaining() > 0) treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(treasureId);
	_buf.putInt(level);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
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

