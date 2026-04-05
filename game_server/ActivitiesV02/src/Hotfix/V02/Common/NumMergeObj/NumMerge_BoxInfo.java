package Hotfix.V02.Common.NumMergeObj;

import java.nio.ByteBuffer;
/*********
 * 数字合并-宝箱信息
 **/
public class NumMerge_BoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 领取到的宝箱等级 */
private int level;
/** 当前剩余分数 */
private long score;


public NumMerge_BoxInfo() {
	level = 0;
	score = (long)0;
}

public NumMerge_BoxInfo(
	 int _level
	, long _score
) {	level = _level;
	score = _score;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 领取到的宝箱等级 */
public int getLevel() { return level; }
/** 领取到的宝箱等级 */
public void setLevel(int _level) { level = _level; }
/** 当前剩余分数 */
public long getScore() { return score; }
/** 当前剩余分数 */
public void setScore(long _score) { score = _score; }


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
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putLong(score);
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

