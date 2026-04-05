package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 刷新活动钻石礼包
 **/
public class GC2GS_017_016_ReqRefreshActivityCrystalGiftPack implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 钻石礼包组id */
private long groupId;


public GC2GS_017_016_ReqRefreshActivityCrystalGiftPack() {
	instanceId = (long)0;
	groupId = (long)0;
}

public GC2GS_017_016_ReqRefreshActivityCrystalGiftPack(
	 long _instanceId
	, long _groupId
) {	instanceId = _instanceId;
	groupId = _groupId;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)16; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 钻石礼包组id */
public long getGroupId() { return groupId; }
/** 钻石礼包组id */
public void setGroupId(long _groupId) { groupId = _groupId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(groupId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)16);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)16);
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

