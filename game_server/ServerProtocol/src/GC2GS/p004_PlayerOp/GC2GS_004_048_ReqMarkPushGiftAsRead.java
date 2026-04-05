package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 标记推送礼包为已读
 **/
public class GC2GS_004_048_ReqMarkPushGiftAsRead implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组id */
private long groupId;
/** 推送礼包id */
private long pushGiftId;


public GC2GS_004_048_ReqMarkPushGiftAsRead() {
	groupId = (long)0;
	pushGiftId = (long)0;
}

public GC2GS_004_048_ReqMarkPushGiftAsRead(
	 long _groupId
	, long _pushGiftId
) {	groupId = _groupId;
	pushGiftId = _pushGiftId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)48; }

/** 礼包组id */
public long getGroupId() { return groupId; }
/** 礼包组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 推送礼包id */
public long getPushGiftId() { return pushGiftId; }
/** 推送礼包id */
public void setPushGiftId(long _pushGiftId) { pushGiftId = _pushGiftId; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pushGiftId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(pushGiftId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)48);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)48);
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

