package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-方块列表
 **/
public class TileMatch_BlockBaseList implements ALBasicProtocolPack._IALProtocolStructure {
/** 方块信息列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> blockList;


public TileMatch_BlockBaseList() {
	blockList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo>();
}

public TileMatch_BlockBaseList(
	 java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> _blockList
) {	blockList = _blockList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 方块信息列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo> getBlockList() { return blockList; }
/** 方块信息列表 */
public void addBlockList(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList) { blockList.add(_blockList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (blockList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (blockList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _blockListCount = _buf.getShort();
	for(int _i = 0; _i < _blockListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList = new Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __blockListCustLen = _buf.getInt();
	int __blockListCurPos = _buf.position();
	_blockList.ReadUnzipBuf(_buf, __blockListCurPos + __blockListCustLen);
	_buf.position(__blockListCurPos + __blockListCustLen);

		blockList.add(_blockList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)blockList.size());
	for(int _i = 0; _i < blockList.size(); _i++) { 
		_buf.putInt(blockList.get(_i).GetBufSize());
	blockList.get(_i).PutUnzipBuf(_buf);
	}
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

