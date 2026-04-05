package GC2GS.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 请求创建集结
 **/
public class GC2GS_042_060_ReqCreateRally implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍ID */
private long teamId;


public GC2GS_042_060_ReqCreateRally() {
	teamId = (long)0;
}

public GC2GS_042_060_ReqCreateRally(
	 long _teamId
) {	teamId = _teamId;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)60; }

/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)60);
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

