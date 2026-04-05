package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 同意对玩家的指定联姻请求
 **/
public class GC2GS_014_011_ReqAgreeToMeApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 匹配的子嗣实例ID */
private long adultId;
/** 发起请求的子嗣实例ID */
private long applyAdultId;


public GC2GS_014_011_ReqAgreeToMeApply() {
	adultId = (long)0;
	applyAdultId = (long)0;
}

public GC2GS_014_011_ReqAgreeToMeApply(
	 long _adultId
	, long _applyAdultId
) {	adultId = _adultId;
	applyAdultId = _applyAdultId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)11; }

/** 匹配的子嗣实例ID */
public long getAdultId() { return adultId; }
/** 匹配的子嗣实例ID */
public void setAdultId(long _adultId) { adultId = _adultId; }
/** 发起请求的子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 发起请求的子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


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
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(adultId);
	_buf.putLong(applyAdultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)11);
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

