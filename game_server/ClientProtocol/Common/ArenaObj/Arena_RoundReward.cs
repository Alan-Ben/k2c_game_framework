using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场回合奖励
/// </summary>
public class Arena_RoundReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 获得道具列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> gainItem;


public Arena_RoundReward() {
	gainItem = new List<NPCommon.NPCommon_ItemInfo>();
}

public Arena_RoundReward(
	List<NPCommon.NPCommon_ItemInfo> _gainItem
) {	gainItem = _gainItem;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 获得道具列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getGainItem() { return gainItem; }
/// <summary>
/// 获得道具列表
/// </summary>
public void addGainItem(NPCommon.NPCommon_ItemInfo _gainItem) { gainItem.Add(_gainItem); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < gainItem.Count; _i++) {
	_size += 4 + gainItem[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < gainItem.Count; _i++) {
	_size += 4 + gainItem[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainItemCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItem = new NPCommon.NPCommon_ItemInfo();
		int __gainItemCustLen = _buf.getInt();
	int __gainItemCurPos = _buf.getCurPos();
	_gainItem.ReadUnzipBuf(_buf, __gainItemCurPos + __gainItemCustLen);
	_buf.setPosition(__gainItemCurPos + __gainItemCustLen);

		gainItem.Add(_gainItem);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)gainItem.Count);
	for(int _i = 0; _i < gainItem.Count; _i++) { 
		_buf.putInt(gainItem[_i].GetBufSize());
	gainItem[_i].PutUnzipBuf(_buf);
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
	builder.Append("gainItem").Append(":").Append(gainItem.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

