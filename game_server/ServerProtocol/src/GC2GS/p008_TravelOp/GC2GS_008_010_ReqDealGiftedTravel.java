package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理增加必生卷王游历事件
 **/
public class GC2GS_008_010_ReqDealGiftedTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件ID */
private long instanceId;


public GC2GS_008_010_ReqDealGiftedTravel() {
	instanceId = (long)0;
}

public GC2GS_008_010_ReqDealGiftedTravel(
	 long _instanceId
) {	instanceId = _instanceId;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)10; }

/** 游历事件ID */
public long getInstanceId() { return instanceId; }
/** 游历事件ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)10);
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

