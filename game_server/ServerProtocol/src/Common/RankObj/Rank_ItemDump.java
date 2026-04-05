package Common.RankObj;

import java.nio.ByteBuffer;
/*********
 * 排行榜基础数据
 **/
public class Rank_ItemDump implements ALBasicProtocolPack._IALProtocolStructure {
private long key;
/** 团体id */
private long groupId;
/** 排名 */
private int rank;
/** 分数 */
private long score;


public Rank_ItemDump() {
	key = (long)0;
	groupId = (long)0;
	rank = 0;
	score = (long)0;
}

public Rank_ItemDump(
	 long _key
	, long _groupId
	, int _rank
	, long _score
) {	key = _key;
	groupId = _groupId;
	rank = _rank;
	score = _score;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
/** 团体id */
public long getGroupId() { return groupId; }
/** 团体id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 排名 */
public int getRank() { return rank; }
/** 排名 */
public void setRank(int _rank) { rank = _rank; }
/** 分数 */
public long getScore() { return score; }
/** 分数 */
public void setScore(long _score) { score = _score; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(key);
	_buf.putLong(groupId);
	_buf.putInt(rank);
	_buf.putLong(score);
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

