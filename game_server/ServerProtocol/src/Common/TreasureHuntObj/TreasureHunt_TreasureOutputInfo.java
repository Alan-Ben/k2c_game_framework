package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-奇物产出信息
 **/
public class TreasureHunt_TreasureOutputInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 奇物ID */
private long treasureId;
/** 下次可领取钻石时间 ms */
private long nextCanDrawGemTimeMs;
/** 下次可领取钻石数量 */
private int nextCanDrawGemNum;


public TreasureHunt_TreasureOutputInfo() {
	treasureId = (long)0;
	nextCanDrawGemTimeMs = (long)0;
	nextCanDrawGemNum = 0;
}

public TreasureHunt_TreasureOutputInfo(
	 long _treasureId
	, long _nextCanDrawGemTimeMs
	, int _nextCanDrawGemNum
) {	treasureId = _treasureId;
	nextCanDrawGemTimeMs = _nextCanDrawGemTimeMs;
	nextCanDrawGemNum = _nextCanDrawGemNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 奇物ID */
public long getTreasureId() { return treasureId; }
/** 奇物ID */
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/** 下次可领取钻石时间 ms */
public long getNextCanDrawGemTimeMs() { return nextCanDrawGemTimeMs; }
/** 下次可领取钻石时间 ms */
public void setNextCanDrawGemTimeMs(long _nextCanDrawGemTimeMs) { nextCanDrawGemTimeMs = _nextCanDrawGemTimeMs; }
/** 下次可领取钻石数量 */
public int getNextCanDrawGemNum() { return nextCanDrawGemNum; }
/** 下次可领取钻石数量 */
public void setNextCanDrawGemNum(int _nextCanDrawGemNum) { nextCanDrawGemNum = _nextCanDrawGemNum; }


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
	if(_buf.remaining() > 0) treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextCanDrawGemTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextCanDrawGemNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(treasureId);
	_buf.putLong(nextCanDrawGemTimeMs);
	_buf.putInt(nextCanDrawGemNum);
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

