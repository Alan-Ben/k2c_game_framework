package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_002_015_OnEnterStateCustomTeaming implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long teamId;
private long dungeonId;


public WCGCS2US_002_015_OnEnterStateCustomTeaming() {
	uid = (long)0;
	teamId = (long)0;
	dungeonId = (long)0;
}

public WCGCS2US_002_015_OnEnterStateCustomTeaming(
	 long _uid
	, long _teamId
	, long _dungeonId
) {	uid = _uid;
	teamId = _teamId;
	dungeonId = _dungeonId;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)15; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }


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
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putLong(teamId);
	_buf.putLong(dungeonId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)15);
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

