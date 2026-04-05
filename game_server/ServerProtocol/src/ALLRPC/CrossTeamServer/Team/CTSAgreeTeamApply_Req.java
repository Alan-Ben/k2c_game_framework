package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSAgreeTeamApply_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 玩家CID */
private long cid;
/** 申请玩家CID */
private long applyCid;


public CTSAgreeTeamApply_Req() {
	teamId = (long)0;
	cid = (long)0;
	applyCid = (long)0;
}

public CTSAgreeTeamApply_Req(
	 long _teamId
	, long _cid
	, long _applyCid
) {	teamId = _teamId;
	cid = _cid;
	applyCid = _applyCid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(cid);
	_buf.putLong(applyCid);
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

