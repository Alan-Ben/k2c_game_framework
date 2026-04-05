package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动排行榜分数变更
 **/
public class GS2GC_017_053_OnActivityRankScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
/** 排行榜ID */
private long rankId;
/** 原排名 */
private int oriRank;
/** 排名 */
private int curRank;
private long score;


public GS2GC_017_053_OnActivityRankScoreChg() {
	instanceId = (long)0;
	rankId = (long)0;
	oriRank = 0;
	curRank = 0;
	score = (long)0;
}

public GS2GC_017_053_OnActivityRankScoreChg(
	 long _instanceId
	, long _rankId
	, int _oriRank
	, int _curRank
	, long _score
) {	instanceId = _instanceId;
	rankId = _rankId;
	oriRank = _oriRank;
	curRank = _curRank;
	score = _score;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)53; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 排行榜ID */
public long getRankId() { return rankId; }
/** 排行榜ID */
public void setRankId(long _rankId) { rankId = _rankId; }
/** 原排名 */
public int getOriRank() { return oriRank; }
/** 原排名 */
public void setOriRank(int _oriRank) { oriRank = _oriRank; }
/** 排名 */
public int getCurRank() { return curRank; }
/** 排名 */
public void setCurRank(int _curRank) { curRank = _curRank; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
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
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.putInt(oriRank);
	_buf.putInt(curRank);
	_buf.putLong(score);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)53);
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

