package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 玩家组队申请数据
 **/
public class CrossTeam_PlayerApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请队伍实例ID */
private long teamId;
/** 请求时间（毫秒） */
private long applyMs;


public CrossTeam_PlayerApplyInfo() {
	teamId = (long)0;
	applyMs = (long)0;
}

public CrossTeam_PlayerApplyInfo(
	 long _teamId
	, long _applyMs
) {	teamId = _teamId;
	applyMs = _applyMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请队伍实例ID */
public long getTeamId() { return teamId; }
/** 申请队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 请求时间（毫秒） */
public long getApplyMs() { return applyMs; }
/** 请求时间（毫秒） */
public void setApplyMs(long _applyMs) { applyMs = _applyMs; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(applyMs);
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

