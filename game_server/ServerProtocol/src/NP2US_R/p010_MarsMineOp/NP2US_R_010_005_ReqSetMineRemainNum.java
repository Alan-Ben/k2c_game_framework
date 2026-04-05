package NP2US_R.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_R_010_005_ReqSetMineRemainNum implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例ID */
private long instanceId;
/** 剩余资源数量 */
private long remainNum;


public NP2US_R_010_005_ReqSetMineRemainNum() {
	instanceId = (long)0;
	remainNum = (long)0;
}

public NP2US_R_010_005_ReqSetMineRemainNum(
	 long _instanceId
	, long _remainNum
) {	instanceId = _instanceId;
	remainNum = _remainNum;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)5; }

/** 矿实例ID */
public long getInstanceId() { return instanceId; }
/** 矿实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 剩余资源数量 */
public long getRemainNum() { return remainNum; }
/** 剩余资源数量 */
public void setRemainNum(long _remainNum) { remainNum = _remainNum; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(remainNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
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

