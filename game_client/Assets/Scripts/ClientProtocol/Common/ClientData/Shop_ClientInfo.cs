using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 商店-客户端数据
/// </summary>
public class Shop_ClientInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店信息数据
/// </summary>
private List<Common.ClientData.Shop_ClientInfoData> ShopInfoData;


public Shop_ClientInfo() {
	ShopInfoData = new List<Common.ClientData.Shop_ClientInfoData>();
}

public Shop_ClientInfo(
	List<Common.ClientData.Shop_ClientInfoData> _ShopInfoData
) {	ShopInfoData = _ShopInfoData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 商店信息数据
/// </summary>
public List<Common.ClientData.Shop_ClientInfoData> getShopInfoData() { return ShopInfoData; }
/// <summary>
/// 商店信息数据
/// </summary>
public void addShopInfoData(Common.ClientData.Shop_ClientInfoData _ShopInfoData) { ShopInfoData.Add(_ShopInfoData); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (ShopInfoData.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (ShopInfoData.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _ShopInfoDataCount = _buf.getShort();
	for(int _i = 0; _i < _ShopInfoDataCount; _i++) { 
		Common.ClientData.Shop_ClientInfoData _ShopInfoData = new Common.ClientData.Shop_ClientInfoData();
		int __ShopInfoDataCustLen = _buf.getInt();
	int __ShopInfoDataCurPos = _buf.getCurPos();
	_ShopInfoData.ReadUnzipBuf(_buf, __ShopInfoDataCurPos + __ShopInfoDataCustLen);
	_buf.setPosition(__ShopInfoDataCurPos + __ShopInfoDataCustLen);

		ShopInfoData.Add(_ShopInfoData);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)ShopInfoData.Count);
	for(int _i = 0; _i < ShopInfoData.Count; _i++) { 
		_buf.putInt(ShopInfoData[_i].GetBufSize());
	ShopInfoData[_i].PutUnzipBuf(_buf);
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
	builder.Append("ShopInfoData").Append(":").Append(ShopInfoData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

