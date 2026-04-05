package ToPay_R.p001_PayOp;

import java.nio.ByteBuffer;
public class ToPay_R_001_003_NotifyPayCallbackHadProcess implements ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
private boolean isDeliverySuccess;


public ToPay_R_001_003_NotifyPayCallbackHadProcess() {
	dbId = (long)0;
	isDeliverySuccess = false;
}

public ToPay_R_001_003_NotifyPayCallbackHadProcess(
	 long _dbId
	, boolean _isDeliverySuccess
) {	dbId = _dbId;
	isDeliverySuccess = _isDeliverySuccess;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

public long getDbId() { return dbId; }
public void setDbId(long _dbId) { dbId = _dbId; }
public boolean getIsDeliverySuccess() { return isDeliverySuccess; }
public void setIsDeliverySuccess(boolean _isDeliverySuccess) { isDeliverySuccess = _isDeliverySuccess; }


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
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDeliverySuccess = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.put(isDeliverySuccess?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

