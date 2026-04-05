using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-掉落逻辑
/// </summary>
public class TileMatch_Drop : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置变更列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg> posChgList;
/// <summary>
/// 生成方块信息列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> genBlockList;


public TileMatch_Drop() {
	posChgList = new List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg>();
	genBlockList = new List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo>();
}

public TileMatch_Drop(
	List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg> _posChgList
	, List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList
) {	posChgList = _posChgList;
	genBlockList = _genBlockList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 位置变更列表
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg> getPosChgList() { return posChgList; }
/// <summary>
/// 位置变更列表
/// </summary>
public void addPosChgList(Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg _posChgList) { posChgList.Add(_posChgList); }
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
	_size += 2 + (posChgList.Count * 12);
	_size += 2 + (genBlockList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (posChgList.Count * 12);
	_size += 2 + (genBlockList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _posChgListCount = _buf.getShort();
	for(int _i = 0; _i < _posChgListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg _posChgList = new Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg();
		int __posChgListCustLen = _buf.getInt();
	int __posChgListCurPos = _buf.getCurPos();
	_posChgList.ReadUnzipBuf(_buf, __posChgListCurPos + __posChgListCustLen);
	_buf.setPosition(__posChgListCurPos + __posChgListCustLen);

		posChgList.Add(_posChgList);
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
	_buf.putShort((short)posChgList.Count);
	for(int _i = 0; _i < posChgList.Count; _i++) { 
		_buf.putInt(posChgList[_i].GetBufSize());
	posChgList[_i].PutUnzipBuf(_buf);
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
	builder.Append("posChgList").Append(":").Append(posChgList.ToString()).Append(", ");
	builder.Append("genBlockList").Append(":").Append(genBlockList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

