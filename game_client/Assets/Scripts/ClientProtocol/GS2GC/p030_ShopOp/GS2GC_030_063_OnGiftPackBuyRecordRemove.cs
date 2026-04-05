using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p030_ShopOp
{

/// <summary>
/// 礼包记录移除推送
/// </summary>
public class GS2GC_030_063_OnGiftPackBuyRecordRemove : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包id列表
/// </summary>
private List<long> giftPackIdList;


public GS2GC_030_063_OnGiftPackBuyRecordRemove() {
	giftPackIdList = new List<long>();
}

public GS2GC_030_063_OnGiftPackBuyRecordRemove(
	List<long> _giftPackIdList
) {	giftPackIdList = _giftPackIdList;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)63; }

/// <summary>
/// 礼包id列表
/// </summary>
public List<long> getGiftPackIdList() { return giftPackIdList; }
/// <summary>
/// 礼包id列表
/// </summary>
public void addGiftPackIdList(long _giftPackIdList) { giftPackIdList.Add(_giftPackIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPackIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPackIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _giftPackIdListCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackIdListCount; _i++) { 
		long _giftPackIdList = (long)0;
		_giftPackIdList = _buf.getLong();
		giftPackIdList.Add(_giftPackIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)giftPackIdList.Count);
	for(int _i = 0; _i < giftPackIdList.Count; _i++) { 
		_buf.putLong(giftPackIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)63);
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
	builder.Append("giftPackIdList").Append(":").Append(giftPackIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

