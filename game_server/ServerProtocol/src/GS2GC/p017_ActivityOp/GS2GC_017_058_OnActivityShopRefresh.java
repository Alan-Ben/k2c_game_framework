package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动商店刷新推送
 **/
public class GS2GC_017_058_OnActivityShopRefresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 活动商店信息 */
private Common.ActivityObj.Activity_ShopInfo shopInfo;


public GS2GC_017_058_OnActivityShopRefresh() {
	instanceId = (long)0;
	shopInfo = new Common.ActivityObj.Activity_ShopInfo();
}

public GS2GC_017_058_OnActivityShopRefresh(
	 long _instanceId
	, Common.ActivityObj.Activity_ShopInfo _shopInfo
) {	instanceId = _instanceId;
	shopInfo = _shopInfo;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)58; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 活动商店信息 */
public Common.ActivityObj.Activity_ShopInfo getShopInfo() { return shopInfo; }
/** 活动商店信息 */
public void setShopInfo(Common.ActivityObj.Activity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + shopInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + shopInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _shopInfoCustLen = _buf.getInt();
	int _shopInfoCurPos = _buf.position();
	shopInfo.ReadUnzipBuf(_buf, _shopInfoCurPos + _shopInfoCustLen);
	_buf.position(_shopInfoCurPos + _shopInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)58);
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

