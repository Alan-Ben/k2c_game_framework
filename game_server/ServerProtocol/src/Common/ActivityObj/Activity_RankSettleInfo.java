package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动排行榜结算信息
 **/
public class Activity_RankSettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 排行榜ID */
private long rankId;
/** 排名 */
private long rank;
/** 是否已领奖 */
private boolean hadDraw;
/** 分数 */
private long score;


public Activity_RankSettleInfo() {
	rankId = (long)0;
	rank = (long)0;
	hadDraw = false;
	score = (long)0;
}

public Activity_RankSettleInfo(
	 long _rankId
	, long _rank
	, boolean _hadDraw
	, long _score
) {	rankId = _rankId;
	rank = _rank;
	hadDraw = _hadDraw;
	score = _score;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 排行榜ID */
public long getRankId() { return rankId; }
/** 排行榜ID */
public void setRankId(long _rankId) { rankId = _rankId; }
/** 排名 */
public long getRank() { return rank; }
/** 排名 */
public void setRank(long _rank) { rank = _rank; }
/** 是否已领奖 */
public boolean getHadDraw() { return hadDraw; }
/** 是否已领奖 */
public void setHadDraw(boolean _hadDraw) { hadDraw = _hadDraw; }
/** 分数 */
public long getScore() { return score; }
/** 分数 */
public void setScore(long _score) { score = _score; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rankId);
	_buf.putLong(rank);
	_buf.put(hadDraw?(byte)1:(byte)0);
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

