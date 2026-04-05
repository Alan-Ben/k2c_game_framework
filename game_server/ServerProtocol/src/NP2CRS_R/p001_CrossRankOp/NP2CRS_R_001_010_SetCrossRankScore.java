package NP2CRS_R.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_R_001_010_SetCrossRankScore implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组ID */
private long crossInstanceId;
/** 排行Id */
private long rankId;
/** 排行榜元素KEY */
private long objId;
/** 当前分数来源 */
private long scoreSourceId;
/** 当前分数 */
private long score;
/** 更新时间 */
private long updateTimeMs;


public NP2CRS_R_001_010_SetCrossRankScore() {
	crossInstanceId = (long)0;
	rankId = (long)0;
	objId = (long)0;
	scoreSourceId = (long)0;
	score = (long)0;
	updateTimeMs = (long)0;
}

public NP2CRS_R_001_010_SetCrossRankScore(
	 long _crossInstanceId
	, long _rankId
	, long _objId
	, long _scoreSourceId
	, long _score
	, long _updateTimeMs
) {	crossInstanceId = _crossInstanceId;
	rankId = _rankId;
	objId = _objId;
	scoreSourceId = _scoreSourceId;
	score = _score;
	updateTimeMs = _updateTimeMs;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

/** 跨服分组ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服分组ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排行Id */
public long getRankId() { return rankId; }
/** 排行Id */
public void setRankId(long _rankId) { rankId = _rankId; }
/** 排行榜元素KEY */
public long getObjId() { return objId; }
/** 排行榜元素KEY */
public void setObjId(long _objId) { objId = _objId; }
/** 当前分数来源 */
public long getScoreSourceId() { return scoreSourceId; }
/** 当前分数来源 */
public void setScoreSourceId(long _scoreSourceId) { scoreSourceId = _scoreSourceId; }
/** 当前分数 */
public long getScore() { return score; }
/** 当前分数 */
public void setScore(long _score) { score = _score; }
/** 更新时间 */
public long getUpdateTimeMs() { return updateTimeMs; }
/** 更新时间 */
public void setUpdateTimeMs(long _updateTimeMs) { updateTimeMs = _updateTimeMs; }


public final int GetBufSize() {
	int _size = 48;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scoreSourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) updateTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(crossInstanceId);
	_buf.putLong(rankId);
	_buf.putLong(objId);
	_buf.putLong(scoreSourceId);
	_buf.putLong(score);
	_buf.putLong(updateTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)10);
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

