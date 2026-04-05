using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p201_TileMatchOp
{

public class GS2GC_201_003_RetTileMatchGameOver : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 方块数据
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo> blockList;


public GS2GC_201_003_RetTileMatchGameOver() {
	blockList = new List<Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo>();
}

public GS2GC_201_003_RetTileMatchGameOver(
	List<Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo> _blockList
) {	blockList = _blockList;
}

public byte getMainOrder() { return (byte)201; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 方块数据
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo> getBlockList() { return blockList; }
/// <summary>
/// 方块数据
/// </summary>
public void addBlockList(Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList) { blockList.Add(_blockList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (blockList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (blockList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _blockListCount = _buf.getShort();
	for(int _i = 0; _i < _blockListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo _blockList = new Hotfix.Common.TileMatchObj.TileMatch_BlockBaseInfo();
		int __blockListCustLen = _buf.getInt();
	int __blockListCurPos = _buf.getCurPos();
	_blockList.ReadUnzipBuf(_buf, __blockListCurPos + __blockListCustLen);
	_buf.setPosition(__blockListCurPos + __blockListCustLen);

		blockList.Add(_blockList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)blockList.Count);
	for(int _i = 0; _i < blockList.Count; _i++) { 
		_buf.putInt(blockList[_i].GetBufSize());
	blockList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)3);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("blockList").Append(":").Append(blockList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

