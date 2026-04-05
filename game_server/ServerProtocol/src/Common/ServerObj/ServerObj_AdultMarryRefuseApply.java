package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣联姻拒绝请求数据
 **/
public class ServerObj_AdultMarryRefuseApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标玩家CID */
private long targetCid;
/** 发起请求的玩家CID */
private long applyCid;
/** 发起请求的子嗣实例ID */
private long applyAdultId;


public ServerObj_AdultMarryRefuseApply() {
	targetCid = (long)0;
	applyCid = (long)0;
	applyAdultId = (long)0;
}

public ServerObj_AdultMarryRefuseApply(
	 long _targetCid
	, long _applyCid
	, long _applyAdultId
) {	targetCid = _targetCid;
	applyCid = _applyCid;
	applyAdultId = _applyAdultId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/** 发起请求的玩家CID */
public long getApplyCid() { return applyCid; }
/** 发起请求的玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 发起请求的子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 发起请求的子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetCid);
	_buf.putLong(applyCid);
	_buf.putLong(applyAdultId);
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

