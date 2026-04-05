package Common.RankObj;

import java.nio.ByteBuffer;
/*********
 * 排行榜基础数据
 **/
public class Rank_BaseItem implements ALBasicProtocolPack._IALProtocolStructure {
private long key;
/** 分数来源id 针对大臣.... */
private long sourceId;
private long score;
/** 排名 */
private int rank;


public Rank_BaseItem() {
	key = (long)0;
	sourceId = (long)0;
	score = (long)0;
	rank = 0;
}

public Rank_BaseItem(
	 long _key
	, long _sourceId
	, long _score
	, int _rank
) {	key = _key;
	sourceId = _sourceId;
	score = _score;
	rank = _rank;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
/** 分数来源id 针对大臣.... */
public long getSourceId() { return sourceId; }
/** 分数来源id 针对大臣.... */
public void setSourceId(long _sourceId) { sourceId = _sourceId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }
/** 排名 */
public int getRank() { return rank; }
/** 排名 */
public void setRank(int _rank) { rank = _rank; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(key);
	_buf.putLong(sourceId);
	_buf.putLong(score);
	_buf.putInt(rank);
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

