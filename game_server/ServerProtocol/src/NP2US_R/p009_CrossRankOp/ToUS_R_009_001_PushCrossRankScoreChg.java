package NP2US_R.p009_CrossRankOp;

import java.nio.ByteBuffer;
/*********
 * 跨服排行榜分数变更推送
 **/
public class ToUS_R_009_001_PushCrossRankScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 跨服活动实例ID */
private long crossInstanceId;
/** 排行榜ID */
private long rankId;
/** 原排名 */
private int oriRank;
/** 当前排名 */
private int curRank;
/** 当前分数 */
private long score;


public ToUS_R_009_001_PushCrossRankScoreChg() {
	cid = (long)0;
	crossInstanceId = (long)0;
	rankId = (long)0;
	oriRank = 0;
	curRank = 0;
	score = (long)0;
}

public ToUS_R_009_001_PushCrossRankScoreChg(
	 long _cid
	, long _crossInstanceId
	, long _rankId
	, int _oriRank
	, int _curRank
	, long _score
) {	cid = _cid;
	crossInstanceId = _crossInstanceId;
	rankId = _rankId;
	oriRank = _oriRank;
	curRank = _curRank;
	score = _score;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)1; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 跨服活动实例ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服活动实例ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排行榜ID */
public long getRankId() { return rankId; }
/** 排行榜ID */
public void setRankId(long _rankId) { rankId = _rankId; }
/** 原排名 */
public int getOriRank() { return oriRank; }
/** 原排名 */
public void setOriRank(int _oriRank) { oriRank = _oriRank; }
/** 当前排名 */
public int getCurRank() { return curRank; }
/** 当前排名 */
public void setCurRank(int _curRank) { curRank = _curRank; }
/** 当前分数 */
public long getScore() { return score; }
/** 当前分数 */
public void setScore(long _score) { score = _score; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oriRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(crossInstanceId);
	_buf.putLong(rankId);
	_buf.putInt(oriRank);
	_buf.putInt(curRank);
	_buf.putLong(score);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)1);
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

