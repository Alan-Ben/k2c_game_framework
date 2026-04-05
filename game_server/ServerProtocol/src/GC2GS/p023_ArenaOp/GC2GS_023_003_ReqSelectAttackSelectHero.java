package GC2GS.p023_ArenaOp;

import java.nio.ByteBuffer;
/*********
 * 指定攻击选择出战大臣
 **/
public class GC2GS_023_003_ReqSelectAttackSelectHero implements ALBasicProtocolPack._IALProtocolStructure {
private long opponentCid;
/** 道具id */
private long itemId;
private long heroId;
private long buffId;


public GC2GS_023_003_ReqSelectAttackSelectHero() {
	opponentCid = (long)0;
	itemId = (long)0;
	heroId = (long)0;
	buffId = (long)0;
}

public GC2GS_023_003_ReqSelectAttackSelectHero(
	 long _opponentCid
	, long _itemId
	, long _heroId
	, long _buffId
) {	opponentCid = _opponentCid;
	itemId = _itemId;
	heroId = _heroId;
	buffId = _buffId;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)3; }

public long getOpponentCid() { return opponentCid; }
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/** 道具id */
public long getItemId() { return itemId; }
/** 道具id */
public void setItemId(long _itemId) { itemId = _itemId; }
public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buffId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(opponentCid);
	_buf.putLong(itemId);
	_buf.putLong(heroId);
	_buf.putLong(buffId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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

