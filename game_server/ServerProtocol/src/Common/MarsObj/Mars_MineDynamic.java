package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿产动态数据
 **/
public class Mars_MineDynamic implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 数据Id */
private long refId;
/** 矿状态序列号 */
private long serialize;
/** 占据玩家CID */
private long occupiedCid;
/** 占据队伍ID */
private long occupiedTeamId;
/** 占据时间 */
private long occupiedMs;
/** 占据队伍采集速度 */
private long collectSpeed;
/** 剩余资源量 */
private long remainNum;
/** 占据队伍带兵量 */
private long troopNum;
/** 占据队伍实力 */
private long teamPower;


public Mars_MineDynamic() {
	id = (long)0;
	refId = (long)0;
	serialize = (long)0;
	occupiedCid = (long)0;
	occupiedTeamId = (long)0;
	occupiedMs = (long)0;
	collectSpeed = (long)0;
	remainNum = (long)0;
	troopNum = (long)0;
	teamPower = (long)0;
}

public Mars_MineDynamic(
	 long _id
	, long _refId
	, long _serialize
	, long _occupiedCid
	, long _occupiedTeamId
	, long _occupiedMs
	, long _collectSpeed
	, long _remainNum
	, long _troopNum
	, long _teamPower
) {	id = _id;
	refId = _refId;
	serialize = _serialize;
	occupiedCid = _occupiedCid;
	occupiedTeamId = _occupiedTeamId;
	occupiedMs = _occupiedMs;
	collectSpeed = _collectSpeed;
	remainNum = _remainNum;
	troopNum = _troopNum;
	teamPower = _teamPower;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 数据Id */
public long getRefId() { return refId; }
/** 数据Id */
public void setRefId(long _refId) { refId = _refId; }
/** 矿状态序列号 */
public long getSerialize() { return serialize; }
/** 矿状态序列号 */
public void setSerialize(long _serialize) { serialize = _serialize; }
/** 占据玩家CID */
public long getOccupiedCid() { return occupiedCid; }
/** 占据玩家CID */
public void setOccupiedCid(long _occupiedCid) { occupiedCid = _occupiedCid; }
/** 占据队伍ID */
public long getOccupiedTeamId() { return occupiedTeamId; }
/** 占据队伍ID */
public void setOccupiedTeamId(long _occupiedTeamId) { occupiedTeamId = _occupiedTeamId; }
/** 占据时间 */
public long getOccupiedMs() { return occupiedMs; }
/** 占据时间 */
public void setOccupiedMs(long _occupiedMs) { occupiedMs = _occupiedMs; }
/** 占据队伍采集速度 */
public long getCollectSpeed() { return collectSpeed; }
/** 占据队伍采集速度 */
public void setCollectSpeed(long _collectSpeed) { collectSpeed = _collectSpeed; }
/** 剩余资源量 */
public long getRemainNum() { return remainNum; }
/** 剩余资源量 */
public void setRemainNum(long _remainNum) { remainNum = _remainNum; }
/** 占据队伍带兵量 */
public long getTroopNum() { return troopNum; }
/** 占据队伍带兵量 */
public void setTroopNum(long _troopNum) { troopNum = _troopNum; }
/** 占据队伍实力 */
public long getTeamPower() { return teamPower; }
/** 占据队伍实力 */
public void setTeamPower(long _teamPower) { teamPower = _teamPower; }


public final int GetBufSize() {
	int _size = 80;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 82;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) occupiedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) occupiedTeamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) occupiedMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) troopNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamPower = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putLong(serialize);
	_buf.putLong(occupiedCid);
	_buf.putLong(occupiedTeamId);
	_buf.putLong(occupiedMs);
	_buf.putLong(collectSpeed);
	_buf.putLong(remainNum);
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

