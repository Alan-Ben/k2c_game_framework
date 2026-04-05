package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-矿数据
 **/
public class ServerObj_MarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 归属玩家CID */
private long cid;
/** 配置ID */
private long refId;
/** 状态序列号 */
private long serialize;
/** 结束展示时间（毫秒） */
private long endShowMs;
/** 剩余资源量 */
private long remainNum;
/** 当前队伍ID */
private long teamId;
/** 开启征收时间（毫秒） */
private long startCollectMs;
/** 玩家结束采集速度（秒） */
private long collectSpeed;
/** 联盟ID */
private long guildId;
/** 占据队伍带兵量 */
private long troopNum;
/** 占据队伍实力 */
private long teamPower;


public ServerObj_MarsMine() {
	id = (long)0;
	cid = (long)0;
	refId = (long)0;
	serialize = (long)0;
	endShowMs = (long)0;
	remainNum = (long)0;
	teamId = (long)0;
	startCollectMs = (long)0;
	collectSpeed = (long)0;
	guildId = (long)0;
	troopNum = (long)0;
	teamPower = (long)0;
}

public ServerObj_MarsMine(
	 long _id
	, long _cid
	, long _refId
	, long _serialize
	, long _endShowMs
	, long _remainNum
	, long _teamId
	, long _startCollectMs
	, long _collectSpeed
	, long _guildId
	, long _troopNum
	, long _teamPower
) {	id = _id;
	cid = _cid;
	refId = _refId;
	serialize = _serialize;
	endShowMs = _endShowMs;
	remainNum = _remainNum;
	teamId = _teamId;
	startCollectMs = _startCollectMs;
	collectSpeed = _collectSpeed;
	guildId = _guildId;
	troopNum = _troopNum;
	teamPower = _teamPower;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 归属玩家CID */
public long getCid() { return cid; }
/** 归属玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 配置ID */
public long getRefId() { return refId; }
/** 配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 状态序列号 */
public long getSerialize() { return serialize; }
/** 状态序列号 */
public void setSerialize(long _serialize) { serialize = _serialize; }
/** 结束展示时间（毫秒） */
public long getEndShowMs() { return endShowMs; }
/** 结束展示时间（毫秒） */
public void setEndShowMs(long _endShowMs) { endShowMs = _endShowMs; }
/** 剩余资源量 */
public long getRemainNum() { return remainNum; }
/** 剩余资源量 */
public void setRemainNum(long _remainNum) { remainNum = _remainNum; }
/** 当前队伍ID */
public long getTeamId() { return teamId; }
/** 当前队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 开启征收时间（毫秒） */
public long getStartCollectMs() { return startCollectMs; }
/** 开启征收时间（毫秒） */
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }
/** 玩家结束采集速度（秒） */
public long getCollectSpeed() { return collectSpeed; }
/** 玩家结束采集速度（秒） */
public void setCollectSpeed(long _collectSpeed) { collectSpeed = _collectSpeed; }
/** 联盟ID */
public long getGuildId() { return guildId; }
/** 联盟ID */
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 占据队伍带兵量 */
public long getTroopNum() { return troopNum; }
/** 占据队伍带兵量 */
public void setTroopNum(long _troopNum) { troopNum = _troopNum; }
/** 占据队伍实力 */
public long getTeamPower() { return teamPower; }
/** 占据队伍实力 */
public void setTeamPower(long _teamPower) { teamPower = _teamPower; }


public final int GetBufSize() {
	int _size = 96;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 98;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endShowMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) troopNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamPower = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(cid);
	_buf.putLong(refId);
	_buf.putLong(serialize);
	_buf.putLong(endShowMs);
	_buf.putLong(remainNum);
	_buf.putLong(teamId);
	_buf.putLong(startCollectMs);
	_buf.putLong(collectSpeed);
	_buf.putLong(guildId);
	_buf.putLong(troopNum);
	_buf.putLong(teamPower);
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

