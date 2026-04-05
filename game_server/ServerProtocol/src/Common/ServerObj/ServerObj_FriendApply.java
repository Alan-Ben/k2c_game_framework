package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 好友申请数据
 **/
public class ServerObj_FriendApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请玩家CID */
private long applyCid;
/** 申请时间戳（秒） */
private int applyTs;


public ServerObj_FriendApply() {
	applyCid = (long)0;
	applyTs = 0;
}

public ServerObj_FriendApply(
	 long _applyCid
	, int _applyTs
) {	applyCid = _applyCid;
	applyTs = _applyTs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 申请时间戳（秒） */
public int getApplyTs() { return applyTs; }
/** 申请时间戳（秒） */
public void setApplyTs(int _applyTs) { applyTs = _applyTs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyTs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putInt(applyTs);
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

