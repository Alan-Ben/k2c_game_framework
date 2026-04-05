package ALLRPC.US.Team;

import java.nio.ByteBuffer;
public class UsNotifySyncGroupTeam_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 群组ID */
private long groupId;
/** 队伍ID */
private long teamId;


public UsNotifySyncGroupTeam_Req() {
	groupId = (long)0;
	teamId = (long)0;
}

public UsNotifySyncGroupTeam_Req(
	 long _groupId
	, long _teamId
) {	groupId = _groupId;
	teamId = _teamId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 群组ID */
public long getGroupId() { return groupId; }
/** 群组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(teamId);
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

