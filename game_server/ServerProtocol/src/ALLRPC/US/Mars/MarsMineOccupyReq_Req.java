package ALLRPC.US.Mars;

import java.nio.ByteBuffer;
public class MarsMineOccupyReq_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long mineInstanceId;
private boolean isOtherTeamForward;
/** 玩家派遣信息 */
private Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer playerInfo;
private long startCollectMs;
private long collectSpeed;


public MarsMineOccupyReq_Req() {
	mineInstanceId = (long)0;
	isOtherTeamForward = false;
	playerInfo = new Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer();
	startCollectMs = (long)0;
	collectSpeed = (long)0;
}

public MarsMineOccupyReq_Req(
	 long _mineInstanceId
	, boolean _isOtherTeamForward
	, Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer _playerInfo
	, long _startCollectMs
	, long _collectSpeed
) {	mineInstanceId = _mineInstanceId;
	isOtherTeamForward = _isOtherTeamForward;
	playerInfo = _playerInfo;
	startCollectMs = _startCollectMs;
	collectSpeed = _collectSpeed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getMineInstanceId() { return mineInstanceId; }
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
public boolean getIsOtherTeamForward() { return isOtherTeamForward; }
public void setIsOtherTeamForward(boolean _isOtherTeamForward) { isOtherTeamForward = _isOtherTeamForward; }
/** 玩家派遣信息 */
public Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer getPlayerInfo() { return playerInfo; }
/** 玩家派遣信息 */
public void setPlayerInfo(Common.ServerObj.ServerObj_MarsTeam_OccupyMinePlayer _playerInfo) { playerInfo = _playerInfo; }
public long getStartCollectMs() { return startCollectMs; }
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }
public long getCollectSpeed() { return collectSpeed; }
public void setCollectSpeed(long _collectSpeed) { collectSpeed = _collectSpeed; }


public final int GetBufSize() {
	int _size = 25;
	_size += 4 + playerInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + playerInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOtherTeamForward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerInfoCustLen = _buf.getInt();
	int _playerInfoCurPos = _buf.position();
	playerInfo.ReadUnzipBuf(_buf, _playerInfoCurPos + _playerInfoCustLen);
	_buf.position(_playerInfoCurPos + _playerInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectSpeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mineInstanceId);
	_buf.put(isOtherTeamForward?(byte)1:(byte)0);
	_buf.putInt(playerInfo.GetBufSize());
	playerInfo.PutUnzipBuf(_buf);
	_buf.putLong(startCollectMs);
	_buf.putLong(collectSpeed);
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

