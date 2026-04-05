using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_045_RetGraveConNewInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/// <summary>
/// 领取的物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> gainItemList;


public GS2GC_004_045_RetGraveConNewInfo() {
	cid = (long)0;
	gainItemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_004_045_RetGraveConNewInfo(
	long _cid
	, List<NPCommon.NPCommon_ItemInfo> _gainItemList
) {	cid = _cid;
	gainItemList = _gainItemList;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)45; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 领取的物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/// <summary>
/// 领取的物品列表
/// </summary>
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.Add(_gainItemList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < gainItemList.Count; _i++) {
	_size += 4 + gainItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
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
	_buf.putLong(cid);
	_buf.putShort((short)gainItemList.Count);
	for(int _i = 0; _i < gainItemList.Count; _i++) { 
		_buf.putInt(gainItemList[_i].GetBufSize());
	gainItemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)45);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)45);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("gainItemList").Append(":").Append(gainItemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

