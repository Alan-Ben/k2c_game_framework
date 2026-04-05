using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.NumMergeObj
{

/// <summary>
/// 数字合并-棋子列表
/// </summary>
public class NumMerge_BlockList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 棋子列表
/// </summary>
private List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> blocks;


public NumMerge_BlockList() {
	blocks = new List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase>();
}

public NumMerge_BlockList(
	List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> _blocks
) {	blocks = _blocks;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 棋子列表
/// </summary>
public List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> getBlocks() { return blocks; }
/// <summary>
/// 棋子列表
/// </summary>
public void addBlocks(Hotfix.Common.NumMergeObj.NumMerge_BlockBase _blocks) { blocks.Add(_blocks); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (blocks.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (blocks.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _blocksCount = _buf.getShort();
	for(int _i = 0; _i < _blocksCount; _i++) { 
		Hotfix.Common.NumMergeObj.NumMerge_BlockBase _blocks = new Hotfix.Common.NumMergeObj.NumMerge_BlockBase();
		int __blocksCustLen = _buf.getInt();
	int __blocksCurPos = _buf.getCurPos();
	_blocks.ReadUnzipBuf(_buf, __blocksCurPos + __blocksCustLen);
	_buf.setPosition(__blocksCurPos + __blocksCustLen);

		blocks.Add(_blocks);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)blocks.Count);
	for(int _i = 0; _i < blocks.Count; _i++) { 
		_buf.putInt(blocks[_i].GetBufSize());
	blocks[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("blocks").Append(":").Append(blocks.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

