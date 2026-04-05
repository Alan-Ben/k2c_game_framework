using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-消除逻辑
/// </summary>
public class TileMatch_Remove : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 触发方块索引
/// </summary>
private int triggerBlockIndex;
/// <summary>
/// 消除方块索引列表
/// </summary>
private List<int> removeBlockIndexList;


public TileMatch_Remove() {
	triggerBlockIndex = 0;
	removeBlockIndexList = new List<int>();
}

public TileMatch_Remove(
	int _triggerBlockIndex
	, List<int> _removeBlockIndexList
) {	triggerBlockIndex = _triggerBlockIndex;
	removeBlockIndexList = _removeBlockIndexList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 触发方块索引
/// </summary>
public int getTriggerBlockIndex() { return triggerBlockIndex; }
/// <summary>
/// 触发方块索引
/// </summary>
public void setTriggerBlockIndex(int _triggerBlockIndex) { triggerBlockIndex = _triggerBlockIndex; }
/// <summary>
/// 消除方块索引列表
/// </summary>
public List<int> getRemoveBlockIndexList() { return removeBlockIndexList; }
/// <summary>
/// 消除方块索引列表
/// </summary>
public void addRemoveBlockIndexList(int _removeBlockIndexList) { removeBlockIndexList.Add(_removeBlockIndexList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (removeBlockIndexList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (removeBlockIndexList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	triggerBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _removeBlockIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _removeBlockIndexListCount; _i++) { 
		int _removeBlockIndexList = 0;
		_removeBlockIndexList = _buf.getInt();
		removeBlockIndexList.Add(_removeBlockIndexList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(triggerBlockIndex);
	_buf.putShort((short)removeBlockIndexList.Count);
	for(int _i = 0; _i < removeBlockIndexList.Count; _i++) { 
		_buf.putInt(removeBlockIndexList[_i]);
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
	builder.Append("triggerBlockIndex").Append(":").Append(triggerBlockIndex.ToString()).Append(", ");
	builder.Append("removeBlockIndexList").Append(":").Append(removeBlockIndexList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

