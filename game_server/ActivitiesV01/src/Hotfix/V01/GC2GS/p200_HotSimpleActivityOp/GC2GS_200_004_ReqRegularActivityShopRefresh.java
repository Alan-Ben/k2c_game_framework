package Hotfix.V01.GC2GS.p200_HotSimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 万能活动刷新商店
 **/
public class GC2GS_200_004_ReqRegularActivityShopRefresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;


public GC2GS_200_004_ReqRegularActivityShopRefresh() {
	activityInstanceId = (long)0;
}

public GC2GS_200_004_ReqRegularActivityShopRefresh(
	 long _activityInstanceId
) {	activityInstanceId = _activityInstanceId;
}

public final byte getMainOrder() { return (byte)200; }

public final byte getSubOrder() { return (byte)4; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)4);
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

