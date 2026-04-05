package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消总分数变更
 **/
public class GS2GC_201_055_OnTileMatchTotalScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 总分数 */
private long totalScore;


public GS2GC_201_055_OnTileMatchTotalScoreChg() {
	totalScore = (long)0;
}

public GS2GC_201_055_OnTileMatchTotalScoreChg(
	 long _totalScore
) {	totalScore = _totalScore;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)55; }

/** 总分数 */
public long getTotalScore() { return totalScore; }
/** 总分数 */
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(totalScore);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)55);
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

