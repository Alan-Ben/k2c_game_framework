package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_PlayerGoodsInfo implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

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


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tabId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) goodsId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) amount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buyTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sId);
	_buf.putInt(shopType);
	_buf.putInt(tabId);
	_buf.putLong(goodsId);
	_buf.putInt(amount);
	_buf.putInt(buyTimes);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

