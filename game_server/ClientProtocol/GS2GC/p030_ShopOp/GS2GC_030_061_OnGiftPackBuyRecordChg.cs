using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p030_ShopOp
{

/// <summary>
/// 礼包变更推送
/// </summary>
public class GS2GC_030_061_OnGiftPackBuyRecordChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包信息
/// </summary>
private Common.CommonFuncObj.GiftPack_Info giftPackInfo;


public GS2GC_030_061_OnGiftPackBuyRecordChg() {
	giftPackInfo = new Common.CommonFuncObj.GiftPack_Info();
}

public GS2GC_030_061_OnGiftPackBuyRecordChg(
	Common.CommonFuncObj.GiftPack_Info _giftPackInfo
) {	giftPackInfo = _giftPackInfo;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 礼包信息
/// </summary>
public Common.CommonFuncObj.GiftPack_Info getGiftPackInfo() { return giftPackInfo; }
/// <summary>
/// 礼包信息
/// </summary>
public void setGiftPackInfo(Common.CommonFuncObj.GiftPack_Info _giftPackInfo) { giftPackInfo = _giftPackInfo; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _giftPackInfoCustLen = _buf.getInt();
	int _giftPackInfoCurPos = _buf.getCurPos();
	giftPackInfo.ReadUnzipBuf(_buf, _giftPackInfoCurPos + _giftPackInfoCustLen);
	_buf.setPosition(_giftPackInfoCurPos + _giftPackInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(giftPackInfo.GetBufSize());
	giftPackInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)61);
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
	builder.Append("giftPackInfo").Append(":").Append(giftPackInfo == null ? "null" : giftPackInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

