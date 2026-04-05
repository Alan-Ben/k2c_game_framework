package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 排行榜对象数据
 **/
public class ServerObj_RankObjInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 对象ID */
private long objId;
/** 分数来源ID */
private long scoreSourceId;
/** 分数 */
private long score;
/** 更新时间（毫秒） */
private long updatedMs;
/** 子对象列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_RankSubObjInfo> subObjList;


public ServerObj_RankObjInfo() {
	objId = (long)0;
	scoreSourceId = (long)0;
	score = (long)0;
	updatedMs = (long)0;
	subObjList = new java.util.ArrayList<Common.ServerObj.ServerObj_RankSubObjInfo>();
}

public ServerObj_RankObjInfo(
	 long _objId
	, long _scoreSourceId
	, long _score
	, long _updatedMs
	, java.util.ArrayList<Common.ServerObj.ServerObj_RankSubObjInfo> _subObjList
) {	objId = _objId;
	scoreSourceId = _scoreSourceId;
	score = _score;
	updatedMs = _updatedMs;
	subObjList = _subObjList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对象ID */
public long getObjId() { return objId; }
/** 对象ID */
public void setObjId(long _objId) { objId = _objId; }
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
/** 子对象列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_RankSubObjInfo> getSubObjList() { return subObjList; }
/** 子对象列表 */
public void addSubObjList(Common.ServerObj.ServerObj_RankSubObjInfo _subObjList) { subObjList.add(_subObjList); }


public final int GetBufSize() {
	int _size = 32;
	_size += 2 + (subObjList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (subObjList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scoreSourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) updatedMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _subObjListCount = _buf.getShort();
	for(int _i = 0; _i < _subObjListCount; _i++) { 
		Common.ServerObj.ServerObj_RankSubObjInfo _subObjList = new Common.ServerObj.ServerObj_RankSubObjInfo();
		if(_buf.remaining() <= 0) return;
	int __subObjListCustLen = _buf.getInt();
	int __subObjListCurPos = _buf.position();
	_subObjList.ReadUnzipBuf(_buf, __subObjListCurPos + __subObjListCustLen);
	_buf.position(__subObjListCurPos + __subObjListCustLen);

		subObjList.add(_subObjList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(objId);
	_buf.putLong(scoreSourceId);
	_buf.putLong(score);
	_buf.putLong(updatedMs);
	_buf.putShort((short)subObjList.size());
	for(int _i = 0; _i < subObjList.size(); _i++) { 
		_buf.putInt(subObjList.get(_i).GetBufSize());
	subObjList.get(_i).PutUnzipBuf(_buf);
	}
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

