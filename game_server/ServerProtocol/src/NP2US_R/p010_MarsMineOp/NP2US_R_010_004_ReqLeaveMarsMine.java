package NP2US_R.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_R_010_004_ReqLeaveMarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例ID */
private long instanceId;
/** 玩家CID */
private long cid;
/** 队伍ID */
private long teamId;


public NP2US_R_010_004_ReqLeaveMarsMine() {
	instanceId = (long)0;
	cid = (long)0;
	teamId = (long)0;
}

public NP2US_R_010_004_ReqLeaveMarsMine(
	 long _instanceId
	, long _cid
	, long _teamId
) {	instanceId = _instanceId;
	cid = _cid;
	teamId = _teamId;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)4; }

/** 矿实例ID */
public long getInstanceId() { return instanceId; }
/** 矿实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(cid);
	_buf.putLong(teamId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)4);
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

