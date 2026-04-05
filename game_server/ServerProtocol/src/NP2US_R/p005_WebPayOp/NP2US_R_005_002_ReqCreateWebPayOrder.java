package NP2US_R.p005_WebPayOp;

import java.nio.ByteBuffer;
/*********
 * 创建网页支付订单
 **/
public class NP2US_R_005_002_ReqCreateWebPayOrder implements ALBasicProtocolPack._IALProtocolStructure {
/** 角色ID */
private long cid;
/** 商品ID列表 */
private java.util.ArrayList<Long> goodsIds;
/** 渠道标识 */
private String channelCode;


public NP2US_R_005_002_ReqCreateWebPayOrder() {
	cid = (long)0;
	goodsIds = new java.util.ArrayList<Long>();
	channelCode = "";
}

public NP2US_R_005_002_ReqCreateWebPayOrder(
	 long _cid
	, java.util.ArrayList<Long> _goodsIds
	, String _channelCode
) {	cid = _cid;
	goodsIds = _goodsIds;
	channelCode = _channelCode;
}

public final byte getMainOrder() { return (byte)5; }

public final byte getSubOrder() { return (byte)2; }

/** 角色ID */
public long getCid() { return cid; }
/** 角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 商品ID列表 */
public java.util.ArrayList<Long> getGoodsIds() { return goodsIds; }
/** 商品ID列表 */
public void addGoodsIds(long _goodsIds) { goodsIds.add(_goodsIds); }
/** 渠道标识 */
public String getChannelCode() { return channelCode; }
/** 渠道标识 */
public void setChannelCode(String _channelCode) { channelCode = _channelCode; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (goodsIds.size() * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (goodsIds.size() * 8);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _goodsIdsCount = _buf.getShort();
	for(int _i = 0; _i < _goodsIdsCount; _i++) { 
		long _goodsIds = (long)0;
		if(_buf.remaining() > 0) _goodsIds = _buf.getLong();
		goodsIds.add(_goodsIds);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) channelCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putShort((short)goodsIds.size());
	for(int _i = 0; _i < goodsIds.size(); _i++) { 
		_buf.putLong(goodsIds.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, channelCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)5);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)5);
	_recBuf.put((byte)2);
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

