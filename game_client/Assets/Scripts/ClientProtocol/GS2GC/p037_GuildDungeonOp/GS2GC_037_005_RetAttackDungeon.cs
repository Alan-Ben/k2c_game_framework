using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_005_RetAttackDungeon : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否击杀怪物
/// </summary>
private bool isKilled;
/// <summary>
/// 获得物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> gainItemList;


public GS2GC_037_005_RetAttackDungeon() {
	isKilled = false;
	gainItemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_037_005_RetAttackDungeon(
	bool _isKilled
	, List<NPCommon.NPCommon_ItemInfo> _gainItemList
) {	isKilled = _isKilled;
	gainItemList = _gainItemList;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 是否击杀怪物
/// </summary>
public bool getIsKilled() { return isKilled; }
/// <summary>
/// 是否击杀怪物
/// </summary>
public void setIsKilled(bool _isKilled) { isKilled = _isKilled; }
/// <summary>
/// 获得物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/// <summary>
/// 获得物品列表
/// </summary>
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.Add(_gainItemList); }


public int GetBufSize() {
	int _size = 1;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isKilled = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainItemListCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItemList = new NPCommon.NPCommon_ItemInfo();
		int __gainItemListCustLen = _buf.getInt();
	int __gainItemListCurPos = _buf.getCurPos();
	_gainItemList.ReadUnzipBuf(_buf, __gainItemListCurPos + __gainItemListCustLen);
	_buf.setPosition(__gainItemListCurPos + __gainItemListCustLen);

		gainItemList.Add(_gainItemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isKilled?(byte)1:(byte)0);
	_buf.putShort((short)gainItemList.Count);
	for(int _i = 0; _i < gainItemList.Count; _i++) { 
		_buf.putInt(gainItemList[_i].GetBufSize());
	gainItemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)5);
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
	builder.Append("isKilled").Append(":").Append(isKilled.ToString()).Append(", ");
	builder.Append("gainItemList").Append(":").Append(gainItemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

