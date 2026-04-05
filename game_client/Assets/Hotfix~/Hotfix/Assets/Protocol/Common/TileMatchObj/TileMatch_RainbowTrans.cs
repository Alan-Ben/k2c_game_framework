using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-彩虹转换逻辑
/// </summary>
public class TileMatch_RainbowTrans : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 触发方块索引
/// </summary>
private int triggerBlockIndex;
/// <summary>
/// 新方块id
/// </summary>
private int newBlockId;
/// <summary>
/// 被转换方块索引列表
/// </summary>
private List<int> beTransIndexList;


public TileMatch_RainbowTrans() {
	triggerBlockIndex = 0;
	newBlockId = 0;
	beTransIndexList = new List<int>();
}

public TileMatch_RainbowTrans(
	int _triggerBlockIndex
	, int _newBlockId
	, List<int> _beTransIndexList
) {	triggerBlockIndex = _triggerBlockIndex;
	newBlockId = _newBlockId;
	beTransIndexList = _beTransIndexList;
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
/// 新方块id
/// </summary>
public int getNewBlockId() { return newBlockId; }
/// <summary>
/// 新方块id
/// </summary>
public void setNewBlockId(int _newBlockId) { newBlockId = _newBlockId; }
/// <summary>
/// 被转换方块索引列表
/// </summary>
public List<int> getBeTransIndexList() { return beTransIndexList; }
/// <summary>
/// 被转换方块索引列表
/// </summary>
public void addBeTransIndexList(int _beTransIndexList) { beTransIndexList.Add(_beTransIndexList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (beTransIndexList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (beTransIndexList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	triggerBlockIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newBlockId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _beTransIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _beTransIndexListCount; _i++) { 
		int _beTransIndexList = 0;
		_beTransIndexList = _buf.getInt();
		beTransIndexList.Add(_beTransIndexList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(triggerBlockIndex);
	_buf.putInt(newBlockId);
	_buf.putShort((short)beTransIndexList.Count);
	for(int _i = 0; _i < beTransIndexList.Count; _i++) { 
		_buf.putInt(beTransIndexList[_i]);
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
	builder.Append("newBlockId").Append(":").Append(newBlockId.ToString()).Append(", ");
	builder.Append("beTransIndexList").Append(":").Append(beTransIndexList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

