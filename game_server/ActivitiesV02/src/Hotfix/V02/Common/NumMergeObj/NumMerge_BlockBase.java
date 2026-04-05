package Hotfix.V02.Common.NumMergeObj;

import java.nio.ByteBuffer;
/*********
 * 数字合并-棋子基础信息
 **/
public class NumMerge_BlockBase implements ALBasicProtocolPack._IALProtocolStructure {
/** 棋子等级 0=空 1-10=等级 */
private int level;
/** buff还有多少步消失 0=无buff */
private int buffStep;


public NumMerge_BlockBase() {
	level = 0;
	buffStep = 0;
}

public NumMerge_BlockBase(
	 int _level
	, int _buffStep
) {	level = _level;
	buffStep = _buffStep;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 棋子等级 0=空 1-10=等级 */
public int getLevel() { return level; }
/** 棋子等级 0=空 1-10=等级 */
public void setLevel(int _level) { level = _level; }
/** buff还有多少步消失 0=无buff */
public int getBuffStep() { return buffStep; }
/** buff还有多少步消失 0=无buff */
public void setBuffStep(int _buffStep) { buffStep = _buffStep; }


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
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buffStep = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putInt(buffStep);
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

