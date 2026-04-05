package NP2CRS_R.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_R_001_002_UnregCrossInstance implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组ID */
private long crossInstanceId;
/** us服务器ID */
private long joinerId;


public NP2CRS_R_001_002_UnregCrossInstance() {
	crossInstanceId = (long)0;
	joinerId = (long)0;
}

public NP2CRS_R_001_002_UnregCrossInstance(
	 long _crossInstanceId
	, long _joinerId
) {	crossInstanceId = _crossInstanceId;
	joinerId = _joinerId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 跨服分组ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服分组ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** us服务器ID */
public long getJoinerId() { return joinerId; }
/** us服务器ID */
public void setJoinerId(long _joinerId) { joinerId = _joinerId; }


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
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
	_buf.putLong(joinerId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

