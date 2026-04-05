package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-方块信息
 **/
public class TileMatch_BlockBaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 方块id */
private int blockId;
/** 原始方块id */
private int originBlockId;


public TileMatch_BlockBaseInfo() {
	blockId = 0;
	originBlockId = 0;
}

public TileMatch_BlockBaseInfo(
	 int _blockId
	, int _originBlockId
) {	blockId = _blockId;
	originBlockId = _originBlockId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 方块id */
public int getBlockId() { return blockId; }
/** 方块id */
public void setBlockId(int _blockId) { blockId = _blockId; }
/** 原始方块id */
public int getOriginBlockId() { return originBlockId; }
/** 原始方块id */
public void setOriginBlockId(int _originBlockId) { originBlockId = _originBlockId; }


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
	if(_buf.remaining() > 0) originBlockId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(blockId);
	_buf.putInt(originBlockId);
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

