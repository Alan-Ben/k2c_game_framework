package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会玩家交互记录
 **/
public class Dinner_JoinerLogList implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 己方参加玩家宴会次数 */
private int joinedCount;
/** 玩家参加己方宴会次数 */
private int beJoinedCount;


public Dinner_JoinerLogList() {
	cid = (long)0;
	joinedCount = 0;
	beJoinedCount = 0;
}

public Dinner_JoinerLogList(
	 long _cid
	, int _joinedCount
	, int _beJoinedCount
) {	cid = _cid;
	joinedCount = _joinedCount;
	beJoinedCount = _beJoinedCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 己方参加玩家宴会次数 */
public int getJoinedCount() { return joinedCount; }
/** 己方参加玩家宴会次数 */
public void setJoinedCount(int _joinedCount) { joinedCount = _joinedCount; }
/** 玩家参加己方宴会次数 */
public int getBeJoinedCount() { return beJoinedCount; }
/** 玩家参加己方宴会次数 */
public void setBeJoinedCount(int _beJoinedCount) { beJoinedCount = _beJoinedCount; }


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
	if(_buf.remaining() > 0) joinedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) beJoinedCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(joinedCount);
	_buf.putInt(beJoinedCount);
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

