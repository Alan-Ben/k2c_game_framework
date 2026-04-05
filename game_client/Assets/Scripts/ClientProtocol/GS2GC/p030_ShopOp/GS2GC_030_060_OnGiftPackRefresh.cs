using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p030_ShopOp
{

/// <summary>
/// 礼包刷新推送
/// </summary>
public class GS2GC_030_060_OnGiftPackRefresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包信息列表
/// </summary>
private List<Common.CommonFuncObj.GiftPack_Info> giftPack;


public GS2GC_030_060_OnGiftPackRefresh() {
	giftPack = new List<Common.CommonFuncObj.GiftPack_Info>();
}

public GS2GC_030_060_OnGiftPackRefresh(
	List<Common.CommonFuncObj.GiftPack_Info> _giftPack
) {	giftPack = _giftPack;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 礼包信息列表
/// </summary>
public List<Common.CommonFuncObj.GiftPack_Info> getGiftPack() { return giftPack; }
/// <summary>
/// 礼包信息列表
/// </summary>
public void addGiftPack(Common.CommonFuncObj.GiftPack_Info _giftPack) { giftPack.Add(_giftPack); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPack.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPack.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _giftPackCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackCount; _i++) { 
		Common.CommonFuncObj.GiftPack_Info _giftPack = new Common.CommonFuncObj.GiftPack_Info();
		int __giftPackCustLen = _buf.getInt();
	int __giftPackCurPos = _buf.getCurPos();
	_giftPack.ReadUnzipBuf(_buf, __giftPackCurPos + __giftPackCustLen);
	_buf.setPosition(__giftPackCurPos + __giftPackCustLen);

		giftPack.Add(_giftPack);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)giftPack.Count);
	for(int _i = 0; _i < giftPack.Count; _i++) { 
		_buf.putInt(giftPack[_i].GetBufSize());
	giftPack[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)60);
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
	builder.Append("giftPack").Append(":").Append(giftPack.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

