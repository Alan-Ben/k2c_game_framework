package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 排行榜子对象数据
 **/
public class ServerObj_RankSubObjInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 子对象ID */
private long subObjId;
/** 分数来源ID */
private long scoreSourceId;
/** 分数 */
private long score;
/** 更新时间（毫秒） */
private long updatedMs;


public ServerObj_RankSubObjInfo() {
	subObjId = (long)0;
	scoreSourceId = (long)0;
	score = (long)0;
	updatedMs = (long)0;
}

public ServerObj_RankSubObjInfo(
	 long _subObjId
	, long _scoreSourceId
	, long _score
	, long _updatedMs
) {	subObjId = _subObjId;
	scoreSourceId = _scoreSourceId;
	score = _score;
	updatedMs = _updatedMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子对象ID */
public long getSubObjId() { return subObjId; }
/** 子对象ID */
public void setSubObjId(long _subObjId) { subObjId = _subObjId; }
/** 分数来源ID */
public long getScoreSourceId() { return scoreSourceId; }
/** 分数来源ID */
public void setScoreSourceId(long _scoreSourceId) { scoreSourceId = _scoreSourceId; }
/** 分数 */
public long getScore() { return score; }
/** 分数 */
public void setScore(long _score) { score = _score; }
/** 更新时间（毫秒） */
public long getUpdatedMs() { return updatedMs; }
/** 更新时间（毫秒） */
public void setUpdatedMs(long _updatedMs) { updatedMs = _updatedMs; }


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
	if(_buf.remaining() > 0) subObjId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scoreSourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) updatedMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(subObjId);
	_buf.putLong(scoreSourceId);
	_buf.putLong(score);
	_buf.putLong(updatedMs);
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

