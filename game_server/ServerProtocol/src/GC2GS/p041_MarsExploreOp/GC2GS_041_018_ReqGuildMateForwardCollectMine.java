package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 请求攻击联盟分享矿
 **/
public class GC2GS_041_018_ReqGuildMateForwardCollectMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 分享矿数据库ID */
private long shareMineDbId;
/** 派遣队伍ID */
private long teamId;
/** 是否Pvp行为 */
private boolean isPvpOp;
/** 是否已知有其他队伍前往 */
private boolean isOtherTeamForward;


public GC2GS_041_018_ReqGuildMateForwardCollectMine() {
	shareMineDbId = (long)0;
	teamId = (long)0;
	isPvpOp = false;
	isOtherTeamForward = false;
}

public GC2GS_041_018_ReqGuildMateForwardCollectMine(
	 long _shareMineDbId
	, long _teamId
	, boolean _isPvpOp
	, boolean _isOtherTeamForward
) {	shareMineDbId = _shareMineDbId;
	teamId = _teamId;
	isPvpOp = _isPvpOp;
	isOtherTeamForward = _isOtherTeamForward;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)18; }

/** 分享矿数据库ID */
public long getShareMineDbId() { return shareMineDbId; }
/** 分享矿数据库ID */
public void setShareMineDbId(long _shareMineDbId) { shareMineDbId = _shareMineDbId; }
/** 派遣队伍ID */
public long getTeamId() { return teamId; }
/** 派遣队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 是否Pvp行为 */
public boolean getIsPvpOp() { return isPvpOp; }
/** 是否Pvp行为 */
public void setIsPvpOp(boolean _isPvpOp) { isPvpOp = _isPvpOp; }
/** 是否已知有其他队伍前往 */
public boolean getIsOtherTeamForward() { return isOtherTeamForward; }
/** 是否已知有其他队伍前往 */
public void setIsOtherTeamForward(boolean _isOtherTeamForward) { isOtherTeamForward = _isOtherTeamForward; }


public final int GetBufSize() {
	int _size = 18;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shareMineDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isPvpOp = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOtherTeamForward = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shareMineDbId);
	_buf.putLong(teamId);
	_buf.put(isPvpOp?(byte)1:(byte)0);
	_buf.put(isOtherTeamForward?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)18);
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

