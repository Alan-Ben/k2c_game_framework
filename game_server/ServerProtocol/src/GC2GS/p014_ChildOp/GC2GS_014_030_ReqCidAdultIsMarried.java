package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 请求玩家子嗣是否结婚的信息
 **/
public class GC2GS_014_030_ReqCidAdultIsMarried implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long adultId;


public GC2GS_014_030_ReqCidAdultIsMarried() {
	cid = (long)0;
	adultId = (long)0;
}

public GC2GS_014_030_ReqCidAdultIsMarried(
	 long _cid
	, long _adultId
) {	cid = _cid;
	adultId = _adultId;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)30; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getAdultId() { return adultId; }
public void setAdultId(long _adultId) { adultId = _adultId; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(adultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)30);
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

