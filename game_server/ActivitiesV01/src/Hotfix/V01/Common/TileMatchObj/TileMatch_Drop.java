package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-掉落逻辑
 **/
public class TileMatch_Drop implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置变更列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg> posChgList;
/** 生成方块信息列表 */
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> genBlockList;


public TileMatch_Drop() {
	posChgList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg>();
	genBlockList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo>();
}

public TileMatch_Drop(
	 java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg> _posChgList
	, java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList
) {	posChgList = _posChgList;
	genBlockList = _genBlockList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 位置变更列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg> getPosChgList() { return posChgList; }
/** 位置变更列表 */
public void addPosChgList(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg _posChgList) { posChgList.add(_posChgList); }
/** 生成方块信息列表 */
public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo> getGenBlockList() { return genBlockList; }
/** 生成方块信息列表 */
public void addGenBlockList(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockInfo _genBlockList) { genBlockList.add(_genBlockList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (posChgList.size() * 12);
	_size += 2 + (genBlockList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (posChgList.size() * 12);
	_size += 2 + (genBlockList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _posChgListCount = _buf.getShort();
	for(int _i = 0; _i < _posChgListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg _posChgList = new Hotfix.V01.Common.TileMatchObj.TileMatch_BlockPosChg();
		if(_buf.remaining() <= 0) return;
	int __posChgListCustLen = _buf.getInt();
	int __posChgListCurPos = _buf.position();
	_posChgList.ReadUnzipBuf(_buf, __posChgListCurPos + __posChgListCustLen);
	_buf.position(__posChgListCurPos + __posChgListCustLen);

		posChgList.add(_posChgList);
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
	_buf.putShort((short)posChgList.size());
	for(int _i = 0; _i < posChgList.size(); _i++) { 
		_buf.putInt(posChgList.get(_i).GetBufSize());
	posChgList.get(_i).PutUnzipBuf(_buf);
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

