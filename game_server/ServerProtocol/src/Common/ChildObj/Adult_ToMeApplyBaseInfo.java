package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 向玩家请求指定联姻基础数据
 **/
public class Adult_ToMeApplyBaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 请求子嗣实例ID */
private long applyAdultId;
/** 请求过期截至时间（秒） */
private int expiredTs;


public Adult_ToMeApplyBaseInfo() {
	applyAdultId = (long)0;
	expiredTs = 0;
}

public Adult_ToMeApplyBaseInfo(
	 long _applyAdultId
	, int _expiredTs
) {	applyAdultId = _applyAdultId;
	expiredTs = _expiredTs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 请求子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 请求子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }
/** 请求过期截至时间（秒） */
public int getExpiredTs() { return expiredTs; }
/** 请求过期截至时间（秒） */
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }


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
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyAdultId);
	_buf.putInt(expiredTs);
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

