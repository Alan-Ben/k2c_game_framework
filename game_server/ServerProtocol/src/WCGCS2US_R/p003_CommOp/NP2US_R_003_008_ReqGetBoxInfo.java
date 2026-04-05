package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 请求指定宝箱数据
 **/
public class NP2US_R_003_008_ReqGetBoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long instanceId;
private long cid;


public NP2US_R_003_008_ReqGetBoxInfo() {
	instanceId = (long)0;
	cid = (long)0;
}

public NP2US_R_003_008_ReqGetBoxInfo(
	 long _instanceId
	, long _cid
) {	instanceId = _instanceId;
	cid = _cid;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)8; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

