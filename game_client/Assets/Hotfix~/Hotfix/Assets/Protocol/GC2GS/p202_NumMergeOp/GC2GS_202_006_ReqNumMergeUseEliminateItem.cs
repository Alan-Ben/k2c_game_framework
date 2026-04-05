using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p202_NumMergeOp
{

/// <summary>
/// 数字合并-使用消除道具
/// </summary>
public class GC2GS_202_006_ReqNumMergeUseEliminateItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 要消除的方块索引
/// </summary>
private int blockIndex;


public GC2GS_202_006_ReqNumMergeUseEliminateItem() {
	blockIndex = 0;
}

public GC2GS_202_006_ReqNumMergeUseEliminateItem(
	int _blockIndex
) {	blockIndex = _blockIndex;
}

public byte getMainOrder() { return (byte)202; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 要消除的方块索引
/// </summary>
public int getBlockIndex() { return blockIndex; }
/// <summary>
/// 要消除的方块索引
/// </summary>
public void setBlockIndex(int _blockIndex) { blockIndex = _blockIndex; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	blockIndex = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(blockIndex);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)6);
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
	builder.Append("blockIndex").Append(":").Append(blockIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

