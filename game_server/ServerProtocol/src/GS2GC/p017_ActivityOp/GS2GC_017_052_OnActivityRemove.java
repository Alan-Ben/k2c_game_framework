package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动移除推送
 **/
public class GS2GC_017_052_OnActivityRemove implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;


public GS2GC_017_052_OnActivityRemove() {
	instanceId = (long)0;
}

public GS2GC_017_052_OnActivityRemove(
	 long _instanceId
) {	instanceId = _instanceId;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)52; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
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
	_buf.put((byte)17);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)52);
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

