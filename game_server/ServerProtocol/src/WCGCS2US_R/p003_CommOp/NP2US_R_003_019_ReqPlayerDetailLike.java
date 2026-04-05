package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 玩家详情点赞
 **/
public class NP2US_R_003_019_ReqPlayerDetailLike implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long beLikeCid;
private long guid;


public NP2US_R_003_019_ReqPlayerDetailLike() {
	cid = (long)0;
	beLikeCid = (long)0;
	guid = (long)0;
}

public NP2US_R_003_019_ReqPlayerDetailLike(
	 long _cid
	, long _beLikeCid
	, long _guid
) {	cid = _cid;
	beLikeCid = _beLikeCid;
	guid = _guid;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)19; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getBeLikeCid() { return beLikeCid; }
public void setBeLikeCid(long _beLikeCid) { beLikeCid = _beLikeCid; }
public long getGuid() { return guid; }
public void setGuid(long _guid) { guid = _guid; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) beLikeCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(beLikeCid);
	_buf.putLong(guid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)19);
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

