using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-方块信息
/// </summary>
public class TileMatch_BlockBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 方块id
/// </summary>
private int blockId;
/// <summary>
/// 原始方块id
/// </summary>
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 方块id
/// </summary>
public int getBlockId() { return blockId; }
/// <summary>
/// 方块id
/// </summary>
public void setBlockId(int _blockId) { blockId = _blockId; }
/// <summary>
/// 原始方块id
/// </summary>
public int getOriginBlockId() { return originBlockId; }
/// <summary>
/// 原始方块id
/// </summary>
public void setOriginBlockId(int _originBlockId) { originBlockId = _originBlockId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	blockId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	originBlockId = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(blockId);
	_buf.putInt(originBlockId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("blockId").Append(":").Append(blockId.ToString()).Append(", ");
	builder.Append("originBlockId").Append(":").Append(originBlockId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

