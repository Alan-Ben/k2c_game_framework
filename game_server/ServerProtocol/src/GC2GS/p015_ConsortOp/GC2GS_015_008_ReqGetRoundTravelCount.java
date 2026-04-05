package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-获取周期出游次数
 **/
public class GC2GS_015_008_ReqGetRoundTravelCount implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人邀约类型ID */
private long consortTravelId;


public GC2GS_015_008_ReqGetRoundTravelCount() {
	consortTravelId = (long)0;
}

public GC2GS_015_008_ReqGetRoundTravelCount(
	 long _consortTravelId
) {	consortTravelId = _consortTravelId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)8; }

/** 家人邀约类型ID */
public long getConsortTravelId() { return consortTravelId; }
/** 家人邀约类型ID */
public void setConsortTravelId(long _consortTravelId) { consortTravelId = _consortTravelId; }


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
	if(_buf.remaining() > 0) consortTravelId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortTravelId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)8);
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

