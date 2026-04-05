package Common.ShopObj;

import java.nio.ByteBuffer;
/*********
 * 商店数据
 **/
public class Shop_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店配置id */
private long shopRefId;
/** 下一次刷新时间戳 */
private long nextRefreshTimeMs;
/** 已刷新次数 */
private int refreshNum;
/** 商品列表 */
private java.util.ArrayList<Common.ShopObj.Shop_ItemInfo> goodsList;


public Shop_Info() {
	shopRefId = (long)0;
	nextRefreshTimeMs = (long)0;
	refreshNum = 0;
	goodsList = new java.util.ArrayList<Common.ShopObj.Shop_ItemInfo>();
}

public Shop_Info(
	 long _shopRefId
	, long _nextRefreshTimeMs
	, int _refreshNum
	, java.util.ArrayList<Common.ShopObj.Shop_ItemInfo> _goodsList
) {	shopRefId = _shopRefId;
	nextRefreshTimeMs = _nextRefreshTimeMs;
	refreshNum = _refreshNum;
	goodsList = _goodsList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商店配置id */
public long getShopRefId() { return shopRefId; }
/** 商店配置id */
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/** 下一次刷新时间戳 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下一次刷新时间戳 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 已刷新次数 */
public int getRefreshNum() { return refreshNum; }
/** 已刷新次数 */
public void setRefreshNum(int _refreshNum) { refreshNum = _refreshNum; }
/** 商品列表 */
public java.util.ArrayList<Common.ShopObj.Shop_ItemInfo> getGoodsList() { return goodsList; }
/** 商品列表 */
public void addGoodsList(Common.ShopObj.Shop_ItemInfo _goodsList) { goodsList.add(_goodsList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (goodsList.size() * 52);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (goodsList.size() * 52);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refreshNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _goodsListCount = _buf.getShort();
	for(int _i = 0; _i < _goodsListCount; _i++) { 
		Common.ShopObj.Shop_ItemInfo _goodsList = new Common.ShopObj.Shop_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __goodsListCustLen = _buf.getInt();
	int __goodsListCurPos = _buf.position();
	_goodsList.ReadUnzipBuf(_buf, __goodsListCurPos + __goodsListCustLen);
	_buf.position(__goodsListCurPos + __goodsListCustLen);

		goodsList.add(_goodsList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shopRefId);
	_buf.putLong(nextRefreshTimeMs);
	_buf.putInt(refreshNum);
	_buf.putShort((short)goodsList.size());
	for(int _i = 0; _i < goodsList.size(); _i++) { 
		_buf.putInt(goodsList.get(_i).GetBufSize());
	goodsList.get(_i).PutUnzipBuf(_buf);
	}
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

