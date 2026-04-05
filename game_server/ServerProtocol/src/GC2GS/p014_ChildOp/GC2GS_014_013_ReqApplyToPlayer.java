package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-对指定玩家发起请求
 **/
public class GC2GS_014_013_ReqApplyToPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 匹配的子嗣实例ID */
private long adultId;
/** 目标玩家CID */
private long targetCid;


public GC2GS_014_013_ReqApplyToPlayer() {
	adultId = (long)0;
	targetCid = (long)0;
}

public GC2GS_014_013_ReqApplyToPlayer(
	 long _adultId
	, long _targetCid
) {	adultId = _adultId;
	targetCid = _targetCid;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)13; }

/** 匹配的子嗣实例ID */
public long getAdultId() { return adultId; }
/** 匹配的子嗣实例ID */
public void setAdultId(long _adultId) { adultId = _adultId; }
/** 目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }


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
	if(_buf.remaining() > 0) adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(adultId);
	_buf.putLong(targetCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)13);
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

