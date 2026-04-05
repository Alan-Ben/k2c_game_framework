package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-子任务信息
 **/
public class TileMatch_TaskSubInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标方块id */
private int blockId;
/** 达成数量 */
private int doneNum;


public TileMatch_TaskSubInfo() {
	blockId = 0;
	doneNum = 0;
}

public TileMatch_TaskSubInfo(
	 int _blockId
	, int _doneNum
) {	blockId = _blockId;
	doneNum = _doneNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标方块id */
public int getBlockId() { return blockId; }
/** 目标方块id */
public void setBlockId(int _blockId) { blockId = _blockId; }
/** 达成数量 */
public int getDoneNum() { return doneNum; }
/** 达成数量 */
public void setDoneNum(int _doneNum) { doneNum = _doneNum; }


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
	if(_buf.remaining() > 0) blockId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) doneNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(blockId);
	_buf.putInt(doneNum);
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

