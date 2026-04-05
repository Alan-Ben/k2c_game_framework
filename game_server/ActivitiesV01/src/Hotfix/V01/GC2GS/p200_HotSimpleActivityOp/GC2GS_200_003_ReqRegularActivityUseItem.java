package Hotfix.V01.GC2GS.p200_HotSimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 万能活动使用物品
 **/
public class GC2GS_200_003_ReqRegularActivityUseItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 物品ID */
private long itemId;
/** 是否十连 */
private boolean isTen;


public GC2GS_200_003_ReqRegularActivityUseItem() {
	activityInstanceId = (long)0;
	itemId = (long)0;
	isTen = false;
}

public GC2GS_200_003_ReqRegularActivityUseItem(
	 long _activityInstanceId
	, long _itemId
	, boolean _isTen
) {	activityInstanceId = _activityInstanceId;
	itemId = _itemId;
	isTen = _isTen;
}

public final byte getMainOrder() { return (byte)200; }

public final byte getSubOrder() { return (byte)3; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 物品ID */
public long getItemId() { return itemId; }
/** 物品ID */
public void setItemId(long _itemId) { itemId = _itemId; }
/** 是否十连 */
public boolean getIsTen() { return isTen; }
/** 是否十连 */
public void setIsTen(boolean _isTen) { isTen = _isTen; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(itemId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)3);
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

