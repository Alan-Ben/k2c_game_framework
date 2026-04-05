package GC2GS.p024_DungeonOp;

import java.nio.ByteBuffer;
/*********
 * 午间副本借用大臣攻击
 **/
public class GC2GS_024_002_ReqMiddayDungeonBorrowAttack implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家ID */
private long cid;
/** 大臣ID */
private long heroId;


public GC2GS_024_002_ReqMiddayDungeonBorrowAttack() {
	cid = (long)0;
	heroId = (long)0;
}

public GC2GS_024_002_ReqMiddayDungeonBorrowAttack(
	 long _cid
	, long _heroId
) {	cid = _cid;
	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)2; }

/** 玩家ID */
public long getCid() { return cid; }
/** 玩家ID */
public void setCid(long _cid) { cid = _cid; }
/** 大臣ID */
public long getHeroId() { return heroId; }
/** 大臣ID */
public void setHeroId(long _heroId) { heroId = _heroId; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
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

