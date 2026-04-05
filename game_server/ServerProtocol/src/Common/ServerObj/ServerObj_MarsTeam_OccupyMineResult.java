package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-占领火星矿结果
 **/
public class ServerObj_MarsTeam_OccupyMineResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 玩家队伍ID */
private long teamId;
/** 玩家队伍损耗量 */
private long teamLossValue;
/** 火星矿实例ID */
private long mineInstanceId;
/** 开始采集的时间 */
private long startCollectMs;
/** 采集结束时间 */
private long endCollectMs;
/** 战斗发生时间 */
private long battleTimeMs;
/** 成功占领 */
private boolean isWin;


public ServerObj_MarsTeam_OccupyMineResult() {
	cid = (long)0;
	teamId = (long)0;
	teamLossValue = (long)0;
	mineInstanceId = (long)0;
	startCollectMs = (long)0;
	endCollectMs = (long)0;
	battleTimeMs = (long)0;
	isWin = false;
}

public ServerObj_MarsTeam_OccupyMineResult(
	 long _cid
	, long _teamId
	, long _teamLossValue
	, long _mineInstanceId
	, long _startCollectMs
	, long _endCollectMs
	, long _battleTimeMs
	, boolean _isWin
) {	cid = _cid;
	teamId = _teamId;
	teamLossValue = _teamLossValue;
	mineInstanceId = _mineInstanceId;
	startCollectMs = _startCollectMs;
	endCollectMs = _endCollectMs;
	battleTimeMs = _battleTimeMs;
	isWin = _isWin;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 玩家队伍ID */
public long getTeamId() { return teamId; }
/** 玩家队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 玩家队伍损耗量 */
public long getTeamLossValue() { return teamLossValue; }
/** 玩家队伍损耗量 */
public void setTeamLossValue(long _teamLossValue) { teamLossValue = _teamLossValue; }
/** 火星矿实例ID */
public long getMineInstanceId() { return mineInstanceId; }
/** 火星矿实例ID */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/** 开始采集的时间 */
public long getStartCollectMs() { return startCollectMs; }
/** 开始采集的时间 */
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }
/** 采集结束时间 */
public long getEndCollectMs() { return endCollectMs; }
/** 采集结束时间 */
public void setEndCollectMs(long _endCollectMs) { endCollectMs = _endCollectMs; }
/** 战斗发生时间 */
public long getBattleTimeMs() { return battleTimeMs; }
/** 战斗发生时间 */
public void setBattleTimeMs(long _battleTimeMs) { battleTimeMs = _battleTimeMs; }
/** 成功占领 */
public boolean getIsWin() { return isWin; }
/** 成功占领 */
public void setIsWin(boolean _isWin) { isWin = _isWin; }


public final int GetBufSize() {
	int _size = 57;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 59;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamLossValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isWin = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(teamId);
	_buf.putLong(teamLossValue);
	_buf.putLong(mineInstanceId);
	_buf.putLong(startCollectMs);
	_buf.putLong(endCollectMs);
	_buf.putLong(battleTimeMs);
	_buf.put(isWin?(byte)1:(byte)0);
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

