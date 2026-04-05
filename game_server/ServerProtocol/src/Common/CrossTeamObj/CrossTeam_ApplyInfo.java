package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 组队申请数据
 **/
public class CrossTeam_ApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请玩家CID */
private long applyCid;
/** 请求时间（毫秒） */
private long applyMs;


public CrossTeam_ApplyInfo() {
	applyCid = (long)0;
	applyMs = (long)0;
}

public CrossTeam_ApplyInfo(
	 long _applyCid
	, long _applyMs
) {	applyCid = _applyCid;
	applyMs = _applyMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 请求时间（毫秒） */
public long getApplyMs() { return applyMs; }
/** 请求时间（毫秒） */
public void setApplyMs(long _applyMs) { applyMs = _applyMs; }


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
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(applyMs);
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

