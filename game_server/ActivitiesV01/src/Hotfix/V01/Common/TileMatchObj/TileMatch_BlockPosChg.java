package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-格子位置变更
 **/
public class TileMatch_BlockPosChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 原始方块索引 */
private int oriBlockIndex;
/** 目标方块索引 */
private int tarBlockIndex;


public TileMatch_BlockPosChg() {
	oriBlockIndex = 0;
	tarBlockIndex = 0;
}

public TileMatch_BlockPosChg(
	 int _oriBlockIndex
	, int _tarBlockIndex
) {	oriBlockIndex = _oriBlockIndex;
	tarBlockIndex = _tarBlockIndex;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 原始方块索引 */
public int getOriBlockIndex() { return oriBlockIndex; }
/** 原始方块索引 */
public void setOriBlockIndex(int _oriBlockIndex) { oriBlockIndex = _oriBlockIndex; }
/** 目标方块索引 */
public int getTarBlockIndex() { return tarBlockIndex; }
/** 目标方块索引 */
public void setTarBlockIndex(int _tarBlockIndex) { tarBlockIndex = _tarBlockIndex; }


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
	if(_buf.remaining() > 0) oriBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tarBlockIndex = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(oriBlockIndex);
	_buf.putInt(tarBlockIndex);
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

