package Hotfix.V01.GC2GS.p200_HotSimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 万能活动商店购买物品
 **/
public class GC2GS_200_002_ReqRegularActivityShopBuyItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 物品ID */
private long itemId;
/** 购买数量 */
private int num;


public GC2GS_200_002_ReqRegularActivityShopBuyItem() {
	activityInstanceId = (long)0;
	itemId = (long)0;
	num = 0;
}

public GC2GS_200_002_ReqRegularActivityShopBuyItem(
	 long _activityInstanceId
	, long _itemId
	, int _num
) {	activityInstanceId = _activityInstanceId;
	itemId = _itemId;
	num = _num;
}

public final byte getMainOrder() { return (byte)200; }

public final byte getSubOrder() { return (byte)2; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 物品ID */
public long getItemId() { return itemId; }
/** 物品ID */
public void setItemId(long _itemId) { itemId = _itemId; }
/** 购买数量 */
public int getNum() { return num; }
/** 购买数量 */
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(itemId);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
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

