package Hotfix.V01.GS2GC.p200_HotSimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 万能活动商店刷新推送
 **/
public class GS2GC_200_102_OnRegularActivityShopRefresh implements ALBasicProtocolPack._IALProtocolStructure {
private long activityInstanceId;
/** 万能活动商店信息 */
private Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo shopInfo;


public GS2GC_200_102_OnRegularActivityShopRefresh() {
	activityInstanceId = (long)0;
	shopInfo = new Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo();
}

public GS2GC_200_102_OnRegularActivityShopRefresh(
	 long _activityInstanceId
	, Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo _shopInfo
) {	activityInstanceId = _activityInstanceId;
	shopInfo = _shopInfo;
}

public final byte getMainOrder() { return (byte)200; }

public final byte getSubOrder() { return (byte)102; }

public long getActivityInstanceId() { return activityInstanceId; }
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 万能活动商店信息 */
public Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo getShopInfo() { return shopInfo; }
/** 万能活动商店信息 */
public void setShopInfo(Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }


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
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _shopInfoCustLen = _buf.getInt();
	int _shopInfoCurPos = _buf.position();
	shopInfo.ReadUnzipBuf(_buf, _shopInfoCurPos + _shopInfoCustLen);
	_buf.position(_shopInfoCurPos + _shopInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)102);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)102);
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

