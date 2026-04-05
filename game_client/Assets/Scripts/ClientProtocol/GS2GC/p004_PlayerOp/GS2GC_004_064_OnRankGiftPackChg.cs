using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

/// <summary>
/// 冲榜礼包信息变更
/// </summary>
public class GS2GC_004_064_OnRankGiftPackChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 冲榜礼包信息
/// </summary>
private Common.RankGiftPackObj.RankGiftPack_Info giftPackInfo;


public GS2GC_004_064_OnRankGiftPackChg() {
	giftPackInfo = new Common.RankGiftPackObj.RankGiftPack_Info();
}

public GS2GC_004_064_OnRankGiftPackChg(
	Common.RankGiftPackObj.RankGiftPack_Info _giftPackInfo
) {	giftPackInfo = _giftPackInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)64; }

/// <summary>
/// 冲榜礼包信息
/// </summary>
public Common.RankGiftPackObj.RankGiftPack_Info getGiftPackInfo() { return giftPackInfo; }
/// <summary>
/// 冲榜礼包信息
/// </summary>
public void setGiftPackInfo(Common.RankGiftPackObj.RankGiftPack_Info _giftPackInfo) { giftPackInfo = _giftPackInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + giftPackInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + giftPackInfo.GetBufSize();

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
	_buf.put((byte)4);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)64);
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

