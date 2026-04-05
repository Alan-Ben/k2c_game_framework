package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-采集结算数据
 **/
public class ServerObj_MarsMineSettle implements ALBasicProtocolPack._IALProtocolStructure {
/** 归属玩家CID */
private long cid;
/** 当前队伍ID */
private long teamId;
/** 实例ID */
private long id;
/** 配置ID */
private long refId;
/** 采集结束时间 */
private long endCollectMs;
/** 采集资源量 */
private long num;
/** 队伍损耗数量 */
private long lossValue;


public ServerObj_MarsMineSettle() {
	cid = (long)0;
	teamId = (long)0;
	id = (long)0;
	refId = (long)0;
	endCollectMs = (long)0;
	num = (long)0;
	lossValue = (long)0;
}

public ServerObj_MarsMineSettle(
	 long _cid
	, long _teamId
	, long _id
	, long _refId
	, long _endCollectMs
	, long _num
	, long _lossValue
) {	cid = _cid;
	teamId = _teamId;
	id = _id;
	refId = _refId;
	endCollectMs = _endCollectMs;
	num = _num;
	lossValue = _lossValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 归属玩家CID */
public long getCid() { return cid; }
/** 归属玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 当前队伍ID */
public long getTeamId() { return teamId; }
/** 当前队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 配置ID */
public long getRefId() { return refId; }
/** 配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 采集结束时间 */
public long getEndCollectMs() { return endCollectMs; }
/** 采集结束时间 */
public void setEndCollectMs(long _endCollectMs) { endCollectMs = _endCollectMs; }
/** 采集资源量 */
public long getNum() { return num; }
/** 采集资源量 */
public void setNum(long _num) { num = _num; }
/** 队伍损耗数量 */
public long getLossValue() { return lossValue; }
/** 队伍损耗数量 */
public void setLossValue(long _lossValue) { lossValue = _lossValue; }


public final int GetBufSize() {
	int _size = 56;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 58;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lossValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(teamId);
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putLong(endCollectMs);
	_buf.putLong(num);
	_buf.putLong(lossValue);
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

