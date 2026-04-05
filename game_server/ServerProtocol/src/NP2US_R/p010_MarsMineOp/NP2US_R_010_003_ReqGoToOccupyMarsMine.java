package NP2US_R.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_R_010_003_ReqGoToOccupyMarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例ID */
private long instanceId;
/** 是否有其他玩家前往 */
private boolean isOtherTeamForward;
/** 占领玩家信息 */
private Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer occupyPlayer;
/** 开始采集时间（毫秒） */
private long startCollectMs;
/** 采集速度（秒） */
private long collectSpeed;


public NP2US_R_010_003_ReqGoToOccupyMarsMine() {
	instanceId = (long)0;
	isOtherTeamForward = false;
	occupyPlayer = new Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer();
	startCollectMs = (long)0;
	collectSpeed = (long)0;
}

public NP2US_R_010_003_ReqGoToOccupyMarsMine(
	 long _instanceId
	, boolean _isOtherTeamForward
	, Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer _occupyPlayer
	, long _startCollectMs
	, long _collectSpeed
) {	instanceId = _instanceId;
	isOtherTeamForward = _isOtherTeamForward;
	occupyPlayer = _occupyPlayer;
	startCollectMs = _startCollectMs;
	collectSpeed = _collectSpeed;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)3; }

/** 矿实例ID */
public long getInstanceId() { return instanceId; }
/** 矿实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 是否有其他玩家前往 */
public boolean getIsOtherTeamForward() { return isOtherTeamForward; }
/** 是否有其他玩家前往 */
public void setIsOtherTeamForward(boolean _isOtherTeamForward) { isOtherTeamForward = _isOtherTeamForward; }
/** 占领玩家信息 */
public Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer getOccupyPlayer() { return occupyPlayer; }
/** 占领玩家信息 */
public void setOccupyPlayer(Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer _occupyPlayer) { occupyPlayer = _occupyPlayer; }
/** 开始采集时间（毫秒） */
public long getStartCollectMs() { return startCollectMs; }
/** 开始采集时间（毫秒） */
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }
/** 采集速度（秒） */
public long getCollectSpeed() { return collectSpeed; }
/** 采集速度（秒） */
public void setCollectSpeed(long _collectSpeed) { collectSpeed = _collectSpeed; }


public final int GetBufSize() {
	int _size = 25;
	_size += 4 + occupyPlayer.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + occupyPlayer.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOtherTeamForward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _occupyPlayerCustLen = _buf.getInt();
	int _occupyPlayerCurPos = _buf.position();
	occupyPlayer.ReadUnzipBuf(_buf, _occupyPlayerCurPos + _occupyPlayerCustLen);
	_buf.position(_occupyPlayerCurPos + _occupyPlayerCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectSpeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.put(isOtherTeamForward?(byte)1:(byte)0);
	_buf.putInt(occupyPlayer.GetBufSize());
	occupyPlayer.PutUnzipBuf(_buf);
	_buf.putLong(startCollectMs);
	_buf.putLong(collectSpeed);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)3);
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

