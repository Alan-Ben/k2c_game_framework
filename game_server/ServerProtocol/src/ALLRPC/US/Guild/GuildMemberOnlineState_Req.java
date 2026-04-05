package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildMemberOnlineState_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long guildId;
private long cid;
private boolean isOnLine;
private long timeMS;


public GuildMemberOnlineState_Req() {
	guildId = (long)0;
	cid = (long)0;
	isOnLine = false;
	timeMS = (long)0;
}

public GuildMemberOnlineState_Req(
	 long _guildId
	, long _cid
	, boolean _isOnLine
	, long _timeMS
) {	guildId = _guildId;
	cid = _cid;
	isOnLine = _isOnLine;
	timeMS = _timeMS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public boolean getIsOnLine() { return isOnLine; }
public void setIsOnLine(boolean _isOnLine) { isOnLine = _isOnLine; }
public long getTimeMS() { return timeMS; }
public void setTimeMS(long _timeMS) { timeMS = _timeMS; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOnLine = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guildId);
	_buf.putLong(cid);
	_buf.put(isOnLine?(byte)1:(byte)0);
	_buf.putLong(timeMS);
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

