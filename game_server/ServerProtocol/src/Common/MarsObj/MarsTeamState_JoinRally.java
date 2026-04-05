package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星队伍-加入集结行军状态数据
 **/
public class MarsTeamState_JoinRally implements ALBasicProtocolPack._IALProtocolStructure {
/** 联盟ID */
private long guildId;
/** 集结ID */
private long rallyId;


public MarsTeamState_JoinRally() {
	guildId = (long)0;
	rallyId = (long)0;
}

public MarsTeamState_JoinRally(
	 long _guildId
	, long _rallyId
) {	guildId = _guildId;
	rallyId = _rallyId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 联盟ID */
public long getGuildId() { return guildId; }
/** 联盟ID */
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 集结ID */
public long getRallyId() { return rallyId; }
/** 集结ID */
public void setRallyId(long _rallyId) { rallyId = _rallyId; }


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
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rallyId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildId);
	_buf.putLong(rallyId);
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

