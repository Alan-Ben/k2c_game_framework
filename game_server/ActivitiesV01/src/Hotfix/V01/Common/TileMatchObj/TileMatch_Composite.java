package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-合成逻辑
 **/
public class TileMatch_Composite implements ALBasicProtocolPack._IALProtocolStructure {
/** 消除方块索引列表 */
private java.util.ArrayList<Integer> removeBlockIndexList;
/** 生成方块信息列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> genBlockList;


public TileMatch_Composite() {
	removeBlockIndexList = new java.util.ArrayList<Integer>();
	genBlockList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo>();
}

public TileMatch_Composite(
	 java.util.ArrayList<Integer> _removeBlockIndexList
	, java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList
) {	removeBlockIndexList = _removeBlockIndexList;
	genBlockList = _genBlockList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 消除方块索引列表 */
public java.util.ArrayList<Integer> getRemoveBlockIndexList() { return removeBlockIndexList; }
/** 消除方块索引列表 */
public void addRemoveBlockIndexList(int _removeBlockIndexList) { removeBlockIndexList.add(_removeBlockIndexList); }
/** 生成方块信息列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> getGenBlockList() { return genBlockList; }
/** 生成方块信息列表 */
public void addGenBlockList(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo _genBlockList) { genBlockList.add(_genBlockList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (removeBlockIndexList.size() * 4);
	_size += 2 + (genBlockList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (removeBlockIndexList.size() * 4);
	_size += 2 + (genBlockList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _removeBlockIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _removeBlockIndexListCount; _i++) { 
		int _removeBlockIndexList = 0;
		if(_buf.remaining() > 0) _removeBlockIndexList = _buf.getInt();
		removeBlockIndexList.add(_removeBlockIndexList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _genBlockListCount = _buf.getShort();
	for(int _i = 0; _i < _genBlockListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo _genBlockList = new Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo();
		if(_buf.remaining() <= 0) return;
	int __genBlockListCustLen = _buf.getInt();
	int __genBlockListCurPos = _buf.position();
	_genBlockList.ReadUnzipBuf(_buf, __genBlockListCurPos + __genBlockListCustLen);
	_buf.position(__genBlockListCurPos + __genBlockListCustLen);

		genBlockList.add(_genBlockList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)removeBlockIndexList.size());
	for(int _i = 0; _i < removeBlockIndexList.size(); _i++) { 
		_buf.putInt(removeBlockIndexList.get(_i));
	}
	_buf.putShort((short)genBlockList.size());
	for(int _i = 0; _i < genBlockList.size(); _i++) { 
		_buf.putInt(genBlockList.get(_i).GetBufSize());
	genBlockList.get(_i).PutUnzipBuf(_buf);
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

