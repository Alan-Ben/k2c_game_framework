package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-获取推荐玩家列表
 **/
public class GC2GS_014_012_ReqGetRecommendPlayerList implements ALBasicProtocolPack._IALProtocolStructure {
/** 匹配的子嗣实例ID */
private long adultId;


public GC2GS_014_012_ReqGetRecommendPlayerList() {
	adultId = (long)0;
}

public GC2GS_014_012_ReqGetRecommendPlayerList(
	 long _adultId
) {	adultId = _adultId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)12; }

/** 匹配的子嗣实例ID */
public long getAdultId() { return adultId; }
/** 匹配的子嗣实例ID */
public void setAdultId(long _adultId) { adultId = _adultId; }


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
	if(_buf.remaining() > 0) adultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(adultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)12);
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

