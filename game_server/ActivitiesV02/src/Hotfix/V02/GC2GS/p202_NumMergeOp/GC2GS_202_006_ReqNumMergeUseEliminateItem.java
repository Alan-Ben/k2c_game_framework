package Hotfix.V02.GC2GS.p202_NumMergeOp;

import java.nio.ByteBuffer;
/*********
 * 数字合并-使用消除道具
 **/
public class GC2GS_202_006_ReqNumMergeUseEliminateItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 要消除的方块索引 */
private int blockIndex;


public GC2GS_202_006_ReqNumMergeUseEliminateItem() {
	blockIndex = 0;
}

public GC2GS_202_006_ReqNumMergeUseEliminateItem(
	 int _blockIndex
) {	blockIndex = _blockIndex;
}

public final byte getMainOrder() { return (byte)202; }

public final byte getSubOrder() { return (byte)6; }

/** 要消除的方块索引 */
public int getBlockIndex() { return blockIndex; }
/** 要消除的方块索引 */
public void setBlockIndex(int _blockIndex) { blockIndex = _blockIndex; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) blockIndex = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(blockIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)6);
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

