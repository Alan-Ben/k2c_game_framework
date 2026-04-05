using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_PlayerGoodsInfo : ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private int shopType;
private int tabId;
private long goodsId;
private int amount;
private int buyTimes;


public WCGGS2GC_PlayerGoodsInfo() {
	sId = (long)0;
	shopType = 0;
	tabId = 0;
	goodsId = (long)0;
	amount = 0;
	buyTimes = 0;
}

public WCGGS2GC_PlayerGoodsInfo(
	long _sId
	, int _shopType
	, int _tabId
	, long _goodsId
	, int _amount
	, int _buyTimes
) {	sId = _sId;
	shopType = _shopType;
	tabId = _tabId;
	goodsId = _goodsId;
	amount = _amount;
	buyTimes = _buyTimes;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public int getShopType() { return shopType; }
public void setShopType(int _shopType) { shopType = _shopType; }
public int getTabId() { return tabId; }
public void setTabId(int _tabId) { tabId = _tabId; }
public long getGoodsId() { return goodsId; }
public void setGoodsId(long _goodsId) { goodsId = _goodsId; }
public int getAmount() { return amount; }
public void setAmount(int _amount) { amount = _amount; }
public int getBuyTimes() { return buyTimes; }
public void setBuyTimes(int _buyTimes) { buyTimes = _buyTimes; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	tabId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	goodsId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	amount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buyTimes = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(sId);
	_buf.putInt(shopType);
	_buf.putInt(tabId);
	_buf.putLong(goodsId);
	_buf.putInt(amount);
	_buf.putInt(buyTimes);
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
	builder.Append("sId").Append(":").Append(sId.ToString()).Append(", ");
	builder.Append("shopType").Append(":").Append(shopType.ToString()).Append(", ");
	builder.Append("tabId").Append(":").Append(tabId.ToString()).Append(", ");
	builder.Append("goodsId").Append(":").Append(goodsId.ToString()).Append(", ");
	builder.Append("amount").Append(":").Append(amount.ToString()).Append(", ");
	builder.Append("buyTimes").Append(":").Append(buyTimes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

