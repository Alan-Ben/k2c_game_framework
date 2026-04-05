using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-合成逻辑
/// </summary>
public class TileMatch_Composite : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 消除方块索引列表
/// </summary>
private List<int> removeBlockIndexList;
/// <summary>
/// 生成方块信息列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> genBlockList;


public TileMatch_Composite() {
	removeBlockIndexList = new List<int>();
	genBlockList = new List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo>();
}

public TileMatch_Composite(
	List<int> _removeBlockIndexList
	, List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList
) {	removeBlockIndexList = _removeBlockIndexList;
	genBlockList = _genBlockList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 消除方块索引列表
/// </summary>
public List<int> getRemoveBlockIndexList() { return removeBlockIndexList; }
/// <summary>
/// 消除方块索引列表
/// </summary>
public void addRemoveBlockIndexList(int _removeBlockIndexList) { removeBlockIndexList.Add(_removeBlockIndexList); }
/// <summary>
/// 生成方块信息列表
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> getGenBlockList() { return genBlockList; }
/// <summary>
/// 生成方块信息列表
/// </summary>
public void addGenBlockList(Hotfix.Common.TileMatchObj.TileMatch_BlockInfo _genBlockList) { genBlockList.Add(_genBlockList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (removeBlockIndexList.Count * 4);
	_size += 2 + (genBlockList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (removeBlockIndexList.Count * 4);
	_size += 2 + (genBlockList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _removeBlockIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _removeBlockIndexListCount; _i++) { 
		int _removeBlockIndexList = 0;
		_removeBlockIndexList = _buf.getInt();
		removeBlockIndexList.Add(_removeBlockIndexList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _genBlockListCount = _buf.getShort();
	for(int _i = 0; _i < _genBlockListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_BlockInfo _genBlockList = new Hotfix.Common.TileMatchObj.TileMatch_BlockInfo();
		int __genBlockListCustLen = _buf.getInt();
	int __genBlockListCurPos = _buf.getCurPos();
	_genBlockList.ReadUnzipBuf(_buf, __genBlockListCurPos + __genBlockListCustLen);
	_buf.setPosition(__genBlockListCurPos + __genBlockListCustLen);

		genBlockList.Add(_genBlockList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)removeBlockIndexList.Count);
	for(int _i = 0; _i < removeBlockIndexList.Count; _i++) { 
		_buf.putInt(removeBlockIndexList[_i]);
	}
	_buf.putShort((short)genBlockList.Count);
	for(int _i = 0; _i < genBlockList.Count; _i++) { 
		_buf.putInt(genBlockList[_i].GetBufSize());
	genBlockList[_i].PutUnzipBuf(_buf);
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
	builder.Append("removeBlockIndexList").Append(":").Append(removeBlockIndexList.ToString()).Append(", ");
	builder.Append("genBlockList").Append(":").Append(genBlockList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

