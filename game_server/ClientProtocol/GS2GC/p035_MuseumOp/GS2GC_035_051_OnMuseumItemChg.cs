using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p035_MuseumOp
{

/// <summary>
/// 博物馆物品新增推送
/// </summary>
public class GS2GC_035_051_OnMuseumItemChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 物品信息
/// </summary>
private Common.MuseumObj.Museum_ItemInfo itemInfo;


public GS2GC_035_051_OnMuseumItemChg() {
	itemInfo = new Common.MuseumObj.Museum_ItemInfo();
}

public GS2GC_035_051_OnMuseumItemChg(
	Common.MuseumObj.Museum_ItemInfo _itemInfo
) {	itemInfo = _itemInfo;
}

public byte getMainOrder() { return (byte)35; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 物品信息
/// </summary>
public Common.MuseumObj.Museum_ItemInfo getItemInfo() { return itemInfo; }
/// <summary>
/// 物品信息
/// </summary>
public void setItemInfo(Common.MuseumObj.Museum_ItemInfo _itemInfo) { itemInfo = _itemInfo; }


public int GetBufSize() {
	int _size = 25;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _itemInfoCustLen = _buf.getInt();
	int _itemInfoCurPos = _buf.getCurPos();
	itemInfo.ReadUnzipBuf(_buf, _itemInfoCurPos + _itemInfoCustLen);
	_buf.setPosition(_itemInfoCurPos + _itemInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(itemInfo.GetBufSize());
	itemInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)35);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)35);
	_recBuf.put((byte)51);
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
	builder.Append("itemInfo").Append(":").Append(itemInfo == null ? "null" : itemInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

