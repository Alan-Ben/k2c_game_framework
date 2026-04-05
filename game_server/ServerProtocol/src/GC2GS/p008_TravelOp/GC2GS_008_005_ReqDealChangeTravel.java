package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理兑换游历事件
 **/
public class GC2GS_008_005_ReqDealChangeTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件ID */
private long instanceId;
/** 是否兑换 true-兑换 */
private boolean isChange;


public GC2GS_008_005_ReqDealChangeTravel() {
	instanceId = (long)0;
	isChange = false;
}

public GC2GS_008_005_ReqDealChangeTravel(
	 long _instanceId
	, boolean _isChange
) {	instanceId = _instanceId;
	isChange = _isChange;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)5; }

/** 游历事件ID */
public long getInstanceId() { return instanceId; }
/** 游历事件ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 是否兑换 true-兑换 */
public boolean getIsChange() { return isChange; }
/** 是否兑换 true-兑换 */
public void setIsChange(boolean _isChange) { isChange = _isChange; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isChange = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.put(isChange?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)5);
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

