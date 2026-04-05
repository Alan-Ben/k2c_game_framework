package GC2GS.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
/*********
 * 踢出队伍玩家
 **/
public class GC2GS_012_007_ReqKickActivityTeamPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 被踢玩家CID */
private long kickCid;


public GC2GS_012_007_ReqKickActivityTeamPlayer() {
	teamId = (long)0;
	kickCid = (long)0;
}

public GC2GS_012_007_ReqKickActivityTeamPlayer(
	 long _teamId
	, long _kickCid
) {	teamId = _teamId;
	kickCid = _kickCid;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)7; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 被踢玩家CID */
public long getKickCid() { return kickCid; }
/** 被踢玩家CID */
public void setKickCid(long _kickCid) { kickCid = _kickCid; }


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
	if(_buf.remaining() > 0) kickCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(kickCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)7);
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

