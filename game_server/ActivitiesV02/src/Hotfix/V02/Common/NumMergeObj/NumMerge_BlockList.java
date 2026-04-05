package Hotfix.V02.Common.NumMergeObj;

import java.nio.ByteBuffer;
/*********
 * 数字合并-棋子列表
 **/
public class NumMerge_BlockList implements ALBasicProtocolPack._IALProtocolStructure {
/** 棋子列表 */
private java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> blocks;


public NumMerge_BlockList() {
	blocks = new java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase>();
}

public NumMerge_BlockList(
	 java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> _blocks
) {	blocks = _blocks;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 棋子列表 */
public java.util.ArrayList<Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase> getBlocks() { return blocks; }
/** 棋子列表 */
public void addBlocks(Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase _blocks) { blocks.add(_blocks); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (blocks.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (blocks.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _blocksCount = _buf.getShort();
	for(int _i = 0; _i < _blocksCount; _i++) { 
		Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase _blocks = new Hotfix.V02.Common.NumMergeObj.NumMerge_BlockBase();
		if(_buf.remaining() <= 0) return;
	int __blocksCustLen = _buf.getInt();
	int __blocksCurPos = _buf.position();
	_blocks.ReadUnzipBuf(_buf, __blocksCurPos + __blocksCustLen);
	_buf.position(__blocksCurPos + __blocksCustLen);

		blocks.add(_blocks);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)blocks.size());
	for(int _i = 0; _i < blocks.size(); _i++) { 
		_buf.putInt(blocks.get(_i).GetBufSize());
	blocks.get(_i).PutUnzipBuf(_buf);
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

