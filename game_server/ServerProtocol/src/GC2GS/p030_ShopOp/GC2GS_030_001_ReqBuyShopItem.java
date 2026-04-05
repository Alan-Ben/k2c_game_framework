package GC2GS.p030_ShopOp;

import java.nio.ByteBuffer;
public class GC2GS_030_001_ReqBuyShopItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店配置id */
private long shopRefId;
/** 商品id */
private long instanceId;
/** 数量 */
private long count;


public GC2GS_030_001_ReqBuyShopItem() {
	shopRefId = (long)0;
	instanceId = (long)0;
	count = (long)0;
}

public GC2GS_030_001_ReqBuyShopItem(
	 long _shopRefId
	, long _instanceId
	, long _count
) {	shopRefId = _shopRefId;
	instanceId = _instanceId;
	count = _count;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)1; }

/** 商店配置id */
public long getShopRefId() { return shopRefId; }
/** 商店配置id */
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/** 商品id */
public long getInstanceId() { return instanceId; }
/** 商品id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 数量 */
public long getCount() { return count; }
/** 数量 */
public void setCount(long _count) { count = _count; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shopRefId);
	_buf.putLong(instanceId);
	_buf.putLong(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)1);
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

