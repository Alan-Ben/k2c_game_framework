package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动玩家相关信息
 **/
public class Activity_PlayerData implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 商店信息 */
private Common.ActivityObj.Activity_ShopInfo shopInfo;
/** 钻石礼包信息 */
private Common.CommonFuncObj.CrystalGiftPack_Info crystalGiftPackInfo;


public Activity_PlayerData() {
	activityInstanceId = (long)0;
	shopInfo = new Common.ActivityObj.Activity_ShopInfo();
	crystalGiftPackInfo = new Common.CommonFuncObj.CrystalGiftPack_Info();
}

public Activity_PlayerData(
	 long _activityInstanceId
	, Common.ActivityObj.Activity_ShopInfo _shopInfo
	, Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo
) {	activityInstanceId = _activityInstanceId;
	shopInfo = _shopInfo;
	crystalGiftPackInfo = _crystalGiftPackInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 商店信息 */
public Common.ActivityObj.Activity_ShopInfo getShopInfo() { return shopInfo; }
/** 商店信息 */
public void setShopInfo(Common.ActivityObj.Activity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }
/** 钻石礼包信息 */
public Common.CommonFuncObj.CrystalGiftPack_Info getCrystalGiftPackInfo() { return crystalGiftPackInfo; }
/** 钻石礼包信息 */
public void setCrystalGiftPackInfo(Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo) { crystalGiftPackInfo = _crystalGiftPackInfo; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + shopInfo.GetBufSize();
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + shopInfo.GetBufSize();
	_size += 4 + crystalGiftPackInfo.GetBufSize();

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

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _crystalGiftPackInfoCustLen = _buf.getInt();
	int _crystalGiftPackInfoCurPos = _buf.position();
	crystalGiftPackInfo.ReadUnzipBuf(_buf, _crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);
	_buf.position(_crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
	_buf.putInt(crystalGiftPackInfo.GetBufSize());
	crystalGiftPackInfo.PutUnzipBuf(_buf);
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

