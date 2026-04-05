package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付商品信息
 **/
public class ServerObj_WebPayGoodsInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 商品ID */
private long goodsId;
/** sdk档位ID */
private String sdkPayId;
/** 价格 */
private float amount;
/** 限购信息 */
private Common.ServerObj.ServerObj_WebPayGoodsLimitInfo limit;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> item;
/** 赠品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gift;


public ServerObj_WebPayGoodsInfo() {
	goodsId = (long)0;
	sdkPayId = "";
	amount = 0f;
	limit = new Common.ServerObj.ServerObj_WebPayGoodsLimitInfo();
	item = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	gift = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public ServerObj_WebPayGoodsInfo(
	 long _goodsId
	, String _sdkPayId
	, float _amount
	, Common.ServerObj.ServerObj_WebPayGoodsLimitInfo _limit
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _item
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gift
) {	goodsId = _goodsId;
	sdkPayId = _sdkPayId;
	amount = _amount;
	limit = _limit;
	item = _item;
	gift = _gift;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商品ID */
public long getGoodsId() { return goodsId; }
/** 商品ID */
public void setGoodsId(long _goodsId) { goodsId = _goodsId; }
/** sdk档位ID */
public String getSdkPayId() { return sdkPayId; }
/** sdk档位ID */
public void setSdkPayId(String _sdkPayId) { sdkPayId = _sdkPayId; }
/** 价格 */
public float getAmount() { return amount; }
/** 价格 */
public void setAmount(float _amount) { amount = _amount; }
/** 限购信息 */
public Common.ServerObj.ServerObj_WebPayGoodsLimitInfo getLimit() { return limit; }
/** 限购信息 */
public void setLimit(Common.ServerObj.ServerObj_WebPayGoodsLimitInfo _limit) { limit = _limit; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItem() { return item; }
/** 物品列表 */
public void addItem(NPCommon.NPCommon_ItemInfo _item) { item.add(_item); }
/** 赠品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGift() { return gift; }
/** 赠品列表 */
public void addGift(NPCommon.NPCommon_ItemInfo _gift) { gift.add(_gift); }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);
	_size += 2;
	for(int _i = 0; _i < item.size(); _i++) {
	_size += 4 + item.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < gift.size(); _i++) {
	_size += 4 + gift.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);
	_size += 2;
	for(int _i = 0; _i < item.size(); _i++) {
	_size += 4 + item.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < gift.size(); _i++) {
	_size += 4 + gift.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) goodsId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkPayId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) amount = _buf.getFloat();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _limitCustLen = _buf.getInt();
	int _limitCurPos = _buf.position();
	limit.ReadUnzipBuf(_buf, _limitCurPos + _limitCustLen);
	_buf.position(_limitCurPos + _limitCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemCount = _buf.getShort();
	for(int _i = 0; _i < _itemCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _item = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemCustLen = _buf.getInt();
	int __itemCurPos = _buf.position();
	_item.ReadUnzipBuf(_buf, __itemCurPos + __itemCustLen);
	_buf.position(__itemCurPos + __itemCustLen);

		item.add(_item);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _giftCount = _buf.getShort();
	for(int _i = 0; _i < _giftCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gift = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __giftCustLen = _buf.getInt();
	int __giftCurPos = _buf.position();
	_gift.ReadUnzipBuf(_buf, __giftCurPos + __giftCustLen);
	_buf.position(__giftCurPos + __giftCustLen);

		gift.add(_gift);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(goodsId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkPayId);
	_buf.putFloat(amount);
	_buf.putInt(limit.GetBufSize());
	limit.PutUnzipBuf(_buf);
	_buf.putShort((short)item.size());
	for(int _i = 0; _i < item.size(); _i++) { 
		_buf.putInt(item.get(_i).GetBufSize());
	item.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)gift.size());
	for(int _i = 0; _i < gift.size(); _i++) { 
		_buf.putInt(gift.get(_i).GetBufSize());
	gift.get(_i).PutUnzipBuf(_buf);
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

