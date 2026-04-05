package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsGetDinnerInfoBySort_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long groupId;
private long instanceId;
private long cid;


public DnsGetDinnerInfoBySort_Req() {
	groupId = (long)0;
	instanceId = (long)0;
	cid = (long)0;
}

public DnsGetDinnerInfoBySort_Req(
	 long _groupId
	, long _instanceId
	, long _cid
) {	groupId = _groupId;
	instanceId = _instanceId;
	cid = _cid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGroupId() { return groupId; }
public void setGroupId(long _groupId) { groupId = _groupId; }
public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(instanceId);
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

